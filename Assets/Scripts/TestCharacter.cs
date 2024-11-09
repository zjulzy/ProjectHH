using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Gizmos = Popcron.Gizmos;

namespace ProjectHH
{
    public struct MoveIntentData
    {
        public bool TriggerJump;
        public Vector3 MoveVelocity;
        public bool IsSprinting;
        public bool TriggerMeleeAttack;
    }

    public enum TurnState
    {
        Turning,
        BlockTurning,
        Default
    }

    public enum JumpState
    {
        OnGround,
        FirstJump,
        SecondJump
    }

    public class TestCharacter : MonoBehaviour
    {
        [SerializeField]
        public PlayableAsset PlayableAsset;
        # region 常量

        private const float c_WalkSpeed = 2.0f;
        private const float c_FirstJumpForce = 5.0f;
        private const float c_SecondJumpForce = 3.0f;
        private const float c_Gravity = 9.8f;
        private const float c_RotateSpeed = 5f;
        private const float c_GroundDetectionDistance = 0.5f;

        private static int s_FirstJump = Animator.StringToHash("FirstJump");
        private static int s_DoubleJump = Animator.StringToHash("DoubleJump");
        private static int s_IsOnGround = Animator.StringToHash("IsOnGround");

        # endregion

        #region 变量

        // 移动相关变量
        // private Rigidbody _rigidBody;
        private PlayableDirector _playableDirector;
        private Animator _animator;
        private CharacterController _characterController;
        private MoveIntentData _moveIntent;
        private JumpState _jumpState;
        private TurnState _turnState;
        private float _remainingSpeed;
        private float _rotateProcess = 1;

        private static int s_IsMoving = Animator.StringToHash("IsMoving");

        // 战斗相关变量

        #endregion

        void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _characterController = GetComponent<CharacterController>();
            _playableDirector = GetComponentInChildren<PlayableDirector>();
            // _rigidBody = _characterController.attachedRigidbody;
        }

        void Update()
        {
            var horizentalInput = Input.GetAxis("Horizontal");
            _moveIntent.TriggerMeleeAttack = Input.GetMouseButtonDown(0);

            if (Math.Abs(horizentalInput) < 0.01f)
            {
                _animator.SetBool(s_IsMoving, false);
            }
            else
            {
                var deltaTime = Time.deltaTime;
                if (_jumpState == JumpState.OnGround)
                {
                    _animator.SetBool(s_IsMoving, true);
                    
                    _rotateProcess = Math.Clamp(_rotateProcess + horizentalInput * c_RotateSpeed * deltaTime, 0, 1);
                    transform.rotation = Quaternion.Euler(0, (1-_rotateProcess) * 180, 0);
                    _moveIntent.MoveVelocity = Vector3.Dot(transform.forward ,Vector3.forward) * c_WalkSpeed*Vector3.forward;
                    
                }
                else
                {
                    _moveIntent.MoveVelocity = horizentalInput * c_WalkSpeed * Vector3.forward;
                }
                _characterController.Move(_moveIntent.MoveVelocity * deltaTime);
            }

            _moveIntent.TriggerJump = Input.GetButtonDown("Jump");
            
            // 后续需要增大向下打的射线的长度，延迟更新jumpstate
            Gizmos.Line(transform.position, transform.position + Vector3.down * c_GroundDetectionDistance, Color.red);
            var result = Physics.Raycast(transform.position, Vector3.down, c_GroundDetectionDistance);
            if (result)
            {
                _remainingSpeed = 0;
                _jumpState = JumpState.OnGround;
                _animator.SetBool(s_IsOnGround, true);
            }
            else
            {
                _animator.SetBool(s_IsOnGround, false);
            }


            if (_moveIntent.TriggerJump)
            {
                _moveIntent.TriggerJump = false;
                switch (_jumpState)
                {
                    case JumpState.OnGround:
                        _jumpState = JumpState.FirstJump;
                        _animator.SetTrigger(s_FirstJump);
                        PlayerJump(false);
                        break;
                    case JumpState.FirstJump:
                        _jumpState = JumpState.SecondJump;
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

            if (_moveIntent.TriggerMeleeAttack)
            {
                Debug.Log("Attack");
                _playableDirector.Play(PlayableAsset, DirectorWrapMode.None);
            }
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
    }
}