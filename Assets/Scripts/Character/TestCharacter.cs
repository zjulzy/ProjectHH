using System;
using System.Collections.Generic;
using ProjectHH.StateMachine;
using ProjectHH.World;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using Gizmos = Popcron.Gizmos;

namespace ProjectHH
{
    [RequireComponent(typeof(CharacterController), typeof(Animator))]
    public class TestCharacter : CharacterBase
    {
        private Dictionary<CharacterMoveType, CharacterStateBase> _stateMap = new();
        private CharacterMoveType _currentMoveType = CharacterMoveType.Move;
        private float _charaterRadius = 0.2f;
        private float _characterHeight = 1.5f;
        public bool IgnoreRootMotionCollision = false;

        [SerializeField] public PlayableAsset PlayableAsset;

        [SerializeField] public SkillTimelineConfig SkillTimelineConfig;

        # region 常量

        # region 动画状态机常量

        private static int s_FirstJump = Animator.StringToHash("FirstJump");
        private static int s_DoubleJump = Animator.StringToHash("DoubleJump");
        private static int s_IsOnGround = Animator.StringToHash("IsOnGround");
        private static int s_MoveSpeed = Animator.StringToHash("MoveSpeed");
        private static int s_IsMoving = Animator.StringToHash("IsMoving");
        private static int s_TurnAround = Animator.StringToHash("TurnAround");
        private static int s_ClimbUp = Animator.StringToHash("ClimbUp");

        #endregion

        # endregion


        #region 变量

        // 移动相关变量
        // private Rigidbody _rigidBody;
        private PlayableDirector _playableDirector;
        private Animator _animator;

        public CharacterController CharacterController => _characterController;
        private CharacterController _characterController;

        [ReadOnly] public float _remainingSpeed;

        // 战斗相关变量

        #endregion

        #region Lifecycle

        protected override void Start()
        {
            base.Start();
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
            _playableDirector = GetComponent<PlayableDirector>();
            _animator.SetBool(s_IsOnGround, true);
            _stateMap.Add(CharacterMoveType.Move, new CharacterStateMove(this));
            _stateMap.Add(CharacterMoveType.InAir, new CharacterStateInAir(this));
            _stateMap[_currentMoveType].Enter(default);
        }

        void Update()
        {
            base.Update();
            _characterController.detectCollisions = !IgnoreRootMotionCollision;


            var currentCharacterState = _stateMap[_currentMoveType];
            var (nextMoveType, stateTransferObj) = currentCharacterState.CheckSwitchState();
            if (nextMoveType != CharacterMoveType.None)
            {
                Debug.Log($"Switch to {nextMoveType}");
                _stateMap[_currentMoveType].Exit();
                _currentMoveType = nextMoveType;
                _stateMap[_currentMoveType].Enter(stateTransferObj);
                currentCharacterState = _stateMap[_currentMoveType];
            }

            currentCharacterState.Update();
        }

        #endregion

        #region Animation

        public void StartClimbOn()
        {
            _animator.SetTrigger(s_ClimbUp);
        }

        public void AnimatorStartJump()
        {
            _animator.SetBool(s_IsOnGround, false);
        }

        public void AnimatorLand()
        {
            _animator.SetBool(s_IsOnGround, true);
        }

        public void AnimatorStartMove()
        {
            _animator.SetBool(s_IsMoving, false);
            _animator.SetFloat(s_MoveSpeed, GameInstance.Get().GetCharacterConfigByString("WalkSpeed"));
        }

        public void AnimatorStopMove()
        {
            _animator.SetBool(s_IsMoving, true);
            _animator.SetFloat(s_MoveSpeed, 0);
        }

        public void AnimatorTurnBack()
        {
            _animator.SetTrigger(s_TurnAround);
        }

        private void OnAnimatorMove()
        {
            // 将动画root motion应用到character controller上
            Vector3 point1 = transform.position + _animator.deltaPosition + Vector3.up * 0.2f;
            Vector3 point2 = transform.position + _animator.deltaPosition + Vector3.up * 1.3f;
            if (IgnoreRootMotionCollision || Physics.OverlapCapsule(point1, point2, 0.2f, layerMask: LayerMask.GetMask("Default")).Length >= 1)
            {
                var moveVec = _stateMap[_currentMoveType].OnAnimatorMove(_animator.deltaPosition);
                moveVec.x = 0;
                _characterController.Move(moveVec);
            }
            else
            {
                Gizmos.Line(point1, point2, Color.red);
            }
        }

        #endregion

        #region Skill

        private void PlayerActivateSkill(InputType skillType)
        {
            TimelineAsset timelineAsset = SkillTimelineConfig.SkillMap[skillType];
            _playableDirector.Play(timelineAsset, DirectorWrapMode.None);
        }

        // private void PlayerJump(bool isSecondJump = false)
        // {
        //     if (isSecondJump)
        //     {
        //         _remainingSpeed = c_SecondJumpForce;
        //     }
        //     else
        //     {
        //         _remainingSpeed = c_FirstJumpForce;
        //     }
        // }

        #endregion
    }
}