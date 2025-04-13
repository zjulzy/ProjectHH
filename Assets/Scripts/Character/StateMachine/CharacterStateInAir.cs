using ProjectHH.World;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ProjectHH.StateMachine
{
    public class CharacterStateInAir : CharacterStateBase
    {
        // 正为向上，负为向下
        private float _verticalSpeed;

        // 跳跃接翻越相关变量
        private bool _isClimbingUp;
        private const float c_ClimbAnimTime = 0.833f;
        private Vector3 _targetPos;

        private float _characterRadius = 0.2f;
        private float _characterHeight = 1.5f;

        #region lifecycle

        public override (CharacterMoveType, StateTransferObjects) CheckSwitchState()
        {
            if (GetIsOnGround() && _verticalSpeed <= 0)
                return (CharacterMoveType.Move, _stateTransferObjects);
            return (CharacterMoveType.None, default);
        }

        protected override void OnEnterState(StateTransferObjects stateTransferObj)
        {
            _verticalSpeed = stateTransferObj.InitialVertialSpeed;
            _character.AnimatorStartJump();
            _isClimbingUp = false;
        }

        protected override void OnExitState()
        {
            _verticalSpeed = 0;
            _character.AnimatorLand();
            _isClimbingUp = false;
        }


        protected override void OnUpdate()
        {
            if (_isClimbingUp)
            {
                _character.CharacterController.Move((_characterRadius + 0.1f) / c_ClimbAnimTime * CharacterForward * Time.deltaTime);
                _verticalSpeed = 0;
                return;
            }

            var overlaps = Physics.OverlapCapsule(
                _character.transform.position + Vector3.up * _characterRadius,
                _character.transform.position + Vector3.up * (_characterHeight - _characterRadius),
                _characterRadius, LayerMask.GetMask("Interact"));
            foreach (var collider in overlaps)
            {
                var interact = collider.GetComponent<Interactable>();
                if (interact && interact.Type == InteractableType.Climb && CheckCanClimbUp(collider.transform.position))
                {
                    _isClimbingUp = true;
                    _character.StartClimbOn();
                    _character.IgnoreRootMotionCollision = true;
                    _targetPos = collider.transform.position;
                    GameInstance.Get().TimerSystem.AddTimer(c_ClimbAnimTime, () =>
                        {
                            _isClimbingUp = false;
                            _character.IgnoreRootMotionCollision = false;
                        }
                    );
                    return;
                }
            }

            bool isLeftPressed = GameInstance.Get().InputSystem.CurrentState[ControlAction.MoveLeft] == KeyState.Pressed;
            bool isRightPressed = GameInstance.Get().InputSystem.CurrentState[ControlAction.MoveRight] == KeyState.Pressed;

            var gravity = GameInstance.Get().GetCharacterConfigByString("Gravity");

            // 空中位移需要加上左右移动位移，速度与平地移动相同，不会改变面朝方向
            _verticalSpeed -= gravity * Time.deltaTime;
            var moveStep = Vector3.up * _verticalSpeed * Time.deltaTime;
            if (isLeftPressed != isRightPressed)
            {
                moveStep += (isLeftPressed ? Vector3.back : Vector3.forward) * GameInstance.Get().GetCharacterConfigByString("WalkSpeed") *
                            Time.deltaTime;
            }

            _character.CharacterController.Move(moveStep);
        }

        #endregion


        public float GetVerticalSpeed()
        {
            return _verticalSpeed;
        }

        public override Vector3 OnAnimatorMove(Vector3 moveVector)
        {
            if (_isClimbingUp)
            {
                if (_targetPos.y < _character.transform.position.y + moveVector.y + 0.005f)
                {
                    moveVector.y = _targetPos.y - _character.transform.position.y + 0.005f;
                    moveVector.z = 0;
                }
            }

            return moveVector;
        }

        #region InternalFunction

        private bool CheckCanClimbUp(Vector3 colliderPos)
        {
            if ((colliderPos.z - _character.transform.position.z) * CharacterForward.z < 0)
            {
                Debug.LogError("");
                return false;
            }

            if (colliderPos.y - _character.transform.position.y > 0.5f)
            {
                Debug.LogError("太高了");
                return false;
            }


            if (CharacterForward.z * GameInstance.Get().InputSystem.GetRealHorizontalInput() <= 0)
            {
                Debug.LogError("反向移动");
                return false;
            }

            if (_verticalSpeed > 0)
            {
                Debug.LogError("目前跳跃向量朝上");
                return false;
            }

            return true;
        }

        #endregion

        public CharacterStateInAir(TestCharacter character) : base(character)
        {
            _characterRadius = character.CharacterController.radius;
            _characterHeight = character.CharacterController.height;
        }
    }
}