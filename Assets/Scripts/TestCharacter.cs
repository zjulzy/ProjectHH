using System;
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
    public struct MoveIntentData
    {
        public bool TriggerJump;
        public Vector3 MoveVelocity;
        public float LastMoveDirection;
        public bool IsSprinting;
        public bool TriggerMeleeAttack;
        public bool TriggerRangeAttack;
        public bool TriggerSoulAttack;
    }

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
        [SerializeField] public PlayableAsset PlayableAsset;

        [SerializeField] public SkillTimelineConfig SkillTimelineConfig;

        # region 常量

        private const float c_WalkSpeed = 2.0f;
        private const float c_FirstJumpForce = 6.0f;
        private const float c_SecondJumpForce = 5.0f;
        private const float c_Gravity = 9.8f;
        private const float c_RotateSpeed = 10f;
        private const float c_GroundDetectionDistance = 0.01f;

        private static int s_FirstJump = Animator.StringToHash("FirstJump");
        private static int s_DoubleJump = Animator.StringToHash("DoubleJump");
        private static int s_IsOnGround = Animator.StringToHash("IsOnGround");
        private static int s_MoveSpeed = Animator.StringToHash("MoveSpeed");
        private static int s_IsMoving = Animator.StringToHash("IsMoving");

        # endregion

        #region 变量

        // 移动相关变量
        // private Rigidbody _rigidBody;
        private PlayableDirector _playableDirector;
        private Animator _animator;
        private CharacterController _characterController;
        private MoveIntentData _moveIntent;
        public JumpState CurrentJumpState;
        public TurnState CurrentTurnState;

        [ReadOnly] public float _remainingSpeed;
        [ProgressBar(0, 1)] [ReadOnly] public float _rotateProcess = 1;

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
            // _rigidBody = _characterController.attachedRigidbody;
        }

        void Update()
        {
            var horizentalInput = Input.GetAxis("Horizontal");
            _moveIntent.TriggerMeleeAttack = Input.GetMouseButtonDown(0);
            _moveIntent.TriggerRangeAttack = Input.GetMouseButtonDown(1);

            if (Math.Abs(horizentalInput) < 0.01f || CheckBlockMoving())
            {
                _animator.SetBool(s_IsMoving, false);
                _animator.SetFloat(s_MoveSpeed, 0);
                if (!SkillBlockMoving)
                {
                    if (_rotateProcess != 0 || _rotateProcess != 1)
                    {
                        _rotateProcess = Math.Clamp(_rotateProcess + MathF.Sign(horizentalInput) * c_RotateSpeed * Time.deltaTime, 0, 1);
                        transform.rotation = Quaternion.Euler(0, (1 - _rotateProcess) * 180, 0);
                    }
                }
            }
            else
            {
                _moveIntent.LastMoveDirection = MathF.Sign(horizentalInput);
                var deltaTime = Time.deltaTime;
                if (CurrentJumpState == JumpState.OnGround)
                {
                    // 可以正常行走的情况
                    _animator.SetBool(s_IsMoving, true);
                    _animator.SetFloat(s_MoveSpeed, Math.Abs(horizentalInput) * c_WalkSpeed);
                    _rotateProcess = Math.Clamp(_rotateProcess + MathF.Sign(horizentalInput) * c_RotateSpeed * deltaTime, 0, 1);
                    transform.rotation = Quaternion.Euler(0, (1 - _rotateProcess) * 180, 0);
                    _moveIntent.MoveVelocity = Math.Abs(horizentalInput) * Vector3.Dot(transform.forward, Vector3.forward) * c_WalkSpeed *
                                               Vector3.forward;

                    if (_rotateProcess == 0 || _rotateProcess == 1)
                    {
                        CurrentTurnState = TurnState.Default;
                    }
                    else
                    {
                        CurrentTurnState = TurnState.Turning;
                    }
                }
                else
                {
                    _moveIntent.MoveVelocity = horizentalInput * c_WalkSpeed * Vector3.forward;
                }

                _characterController.Move(_moveIntent.MoveVelocity * deltaTime);
            }

            _moveIntent.TriggerJump = Input.GetButtonDown("Jump");

            // 后续需要增大向下打的射线的长度，延迟更新jumpstate
            Gizmos.Line(transform.position + Vector3.up * c_GroundDetectionDistance, transform.position + Vector3.down * c_GroundDetectionDistance,
                Color.red);
            var result = Physics.RaycastAll(transform.position + Vector3.up * c_GroundDetectionDistance, Vector3.down, c_GroundDetectionDistance * 2);
            if (result.Length > 0)
            {
                _remainingSpeed = 0;
                CurrentJumpState = JumpState.OnGround;
                _animator.SetBool(s_IsOnGround, true);
                transform.position = new Vector3(transform.position.x, result[0].point.y, transform.position.z);
            }
            else
            {
                _animator.SetBool(s_IsOnGround, false);
                if (CurrentJumpState == JumpState.OnGround)
                {
                    CurrentJumpState = JumpState.FirstJump;
                    _remainingSpeed = -Time.deltaTime * c_Gravity;
                }
            }


            if (_moveIntent.TriggerJump && !CheckBlockJump())
            {
                _moveIntent.TriggerJump = false;
                switch (CurrentJumpState)
                {
                    case JumpState.OnGround:
                        CurrentJumpState = JumpState.FirstJump;
                        _animator.SetTrigger(s_FirstJump);
                        PlayerJump(false);
                        break;
                    case JumpState.FirstJump:
                        CurrentJumpState = JumpState.SecondJump;
                        PlayerJump(true);
                        break;
                    case JumpState.SecondJump:
                        break;
                }
            }

            if (_remainingSpeed != 0)
            {
                var deltaTime = Time.deltaTime;
                _characterController.Move(Vector3.up * _remainingSpeed * deltaTime);
                _remainingSpeed -= c_Gravity * deltaTime;
            }

            // 处理技能释放
            if (_moveIntent.TriggerMeleeAttack && !CheckBlockSkill())
            {
                PlayerActivateSkill(InputType.Attack01);
            }
            else if (_moveIntent.TriggerRangeAttack && !CheckBlockSkill())
            {
                PlayerActivateSkill(InputType.Attack02);
            }
        }

        #endregion

        private void PlayerActivateSkill(InputType skillType)
        {
            TimelineAsset timelineAsset = SkillTimelineConfig.SkillMap[skillType];
            _playableDirector.Play(timelineAsset, DirectorWrapMode.None);
        }

        private void PlayerJump(bool isSecondJump = false)
        {
            if (isSecondJump)
            {
                _remainingSpeed = c_SecondJumpForce;
            }
            else
            {
                _remainingSpeed = c_FirstJumpForce;
            }
        }

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
    }
}