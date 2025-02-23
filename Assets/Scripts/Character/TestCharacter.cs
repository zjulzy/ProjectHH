using System;
using System.Collections.Generic;
using ProjectHH.StateMachine;
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
    [Serializable]
    public enum TurnState : uint
    {
        Turning = 0,
        Default = 1
    }

    [Serializable]
    public enum JumpState : uint
    {
        OnGround = 0,
        FirstJump = 1,
        SecondJump = 2
    }

    [RequireComponent(typeof(CharacterController), typeof(Animator))]
    public class TestCharacter : CharacterBase
    {
        private Dictionary<CharacterMoveType, CharacterStateBase> _stateMap = new();
        private CharacterMoveType _currentMoveType = CharacterMoveType.Move;

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

        #endregion

        # endregion


        #region 变量

        // 移动相关变量
        // private Rigidbody _rigidBody;
        private PlayableDirector _playableDirector;
        private Animator _animator;

        public CharacterController CharacterController => _characterController;
        private CharacterController _characterController;
        public JumpState CurrentJumpState;
        public TurnState CurrentTurnState;

        [ReadOnly] public float _remainingSpeed;

        // 未来改成tag
        public bool IsBattle = false;
        public bool SkillBlockMoving = false;

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
            _stateMap[_currentMoveType].Enter();
        }

        void Update()
        {
            base.Update();
            var currentCharacterState = _stateMap[_currentMoveType];
            var nextMoveType = currentCharacterState.CheckSwitchState();
            if (nextMoveType != CharacterMoveType.None)
            {
                _stateMap[_currentMoveType].Exit();
                _currentMoveType = currentCharacterState.CheckSwitchState();
                _stateMap[_currentMoveType].Enter();
                currentCharacterState = _stateMap[_currentMoveType];
                _currentMoveType = nextMoveType;
            }

            currentCharacterState.Update();


            // if (_moveIntent.TriggerJump && !CheckBlockJump())
            // {
            //     _moveIntent.TriggerJump = false;
            //     switch (CurrentJumpState)
            //     {
            //         case JumpState.OnGround:
            //             CurrentJumpState = JumpState.FirstJump;
            //             _animator.SetTrigger(s_FirstJump);
            //             PlayerJump(false);
            //             break;
            //         case JumpState.FirstJump:
            //             CurrentJumpState = JumpState.SecondJump;
            //             PlayerJump(true);
            //             break;
            //         case JumpState.SecondJump:
            //             break;
            //     }
            // }
            //
            // if (_remainingSpeed != 0)
            // {
            //     var deltaTime = Time.deltaTime;
            //     _characterController.Move(Vector3.up * _remainingSpeed * deltaTime);
            //     _remainingSpeed -= c_Gravity * deltaTime;
            // }
        }

        #endregion

        #region Animation

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
            Vector3 point1 = transform.position + _animator.deltaPosition + Vector3.up * 0.3f;
            Vector3 point2 = transform.position + _animator.deltaPosition + Vector3.up * 1.3f;
            if (Physics.OverlapCapsule(point1, point2, 0.3f).Length == 1)
            {
                _characterController.Move(_animator.deltaPosition);
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

        private bool CheckBlockMoving()
        {
            return SkillBlockMoving;
        }

        private bool CheckBlockTurning()
        {
            return SkillBlockMoving || CurrentJumpState != JumpState.OnGround;
        }

        private bool CheckBlockJump()
        {
            return SkillBlockMoving || CurrentJumpState == JumpState.SecondJump || CurrentTurnState == TurnState.Turning;
        }

        private bool CheckBlockSkill()
        {
            return SkillBlockMoving || CurrentTurnState == TurnState.Turning || CurrentJumpState != JumpState.OnGround;
        }

        #endregion
    }
}