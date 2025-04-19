using System;
using UnityEngine;

namespace ProjectHH.StateMachine
{
    public class CharacterStateMove : CharacterStateBase
    {
        public override (CharacterMoveType, StateTransferObjects) CheckSwitchState()
        {
            if (!GetIsOnGround(out var height))
            {
                _switchToInAir = true;
            }
            _character.CharacterController.Move(Vector3.down * (_character.transform.position.y - height));

            if (_switchToInAir)
                return (CharacterMoveType.InAir, _stateTransferObjects);
            return (CharacterMoveType.None, default);
        }

        protected override void OnEnterState(StateTransferObjects stateTransferObj)
        {
            GameInstance.Get().InputSystem.RegisterEvent(ControlAction.MoveLeft, _moveLeftEvent);
            GameInstance.Get().InputSystem.RegisterEvent(ControlAction.MoveRight, _moveRightEvent);
            GameInstance.Get().InputSystem.RegisterEvent(ControlAction.Jump, _jumpEvent);
            _switchToInAir = false;

            // 如果是由滞空状态转化到移动状态，那么需要在这里设置初始动画速度
            if (GameInstance.Get().InputSystem.CurrentState[ControlAction.MoveLeft] !=
                GameInstance.Get().InputSystem.CurrentState[ControlAction.MoveRight])
            {
                _character.AnimatorStartMove();
            }
            else
            {
                _character.AnimatorStopMove();
            }
        }

        protected override void OnExitState()
        {
            GameInstance.Get().InputSystem.UnregisterEvent(ControlAction.MoveLeft, _moveLeftEvent);
            GameInstance.Get().InputSystem.UnregisterEvent(ControlAction.MoveRight, _moveRightEvent);
            GameInstance.Get().InputSystem.UnregisterEvent(ControlAction.Jump, _jumpEvent);
        }

        protected override void OnUpdate()
        {
            bool isLeftPressed = GameInstance.Get().InputSystem.CurrentState[ControlAction.MoveLeft] == KeyState.Pressed;
            bool isRightPressed = GameInstance.Get().InputSystem.CurrentState[ControlAction.MoveRight] == KeyState.Pressed;
            if (isLeftPressed != isRightPressed)
            {
                var moveDirection = isLeftPressed ? Vector3.back : Vector3.forward;
                _character.transform.forward = moveDirection;
                _character.CharacterController.Move(moveDirection * GameInstance.Get().GetCharacterConfigByString("WalkSpeed") * Time.deltaTime);
            }
        }


        private Action<KeyState> _moveLeftEvent;
        private Action<KeyState> _moveRightEvent;
        private Action<KeyState> _jumpEvent;
        private bool _switchToInAir = false;

        public CharacterStateMove(TestCharacter character) : base(character)
        {
            _moveLeftEvent = (state) =>
            {
                if (state == GameInstance.Get().InputSystem.CurrentState[ControlAction.MoveRight])
                {
                    character.AnimatorStopMove();
                }
                else
                {
                    character.AnimatorStartMove();
                    if (GameInstance.Get().GetCurrentPlayer().transform.forward == Vector3.forward && state == KeyState.Pressed ||
                        GameInstance.Get().GetCurrentPlayer().transform.forward == Vector3.back && state == KeyState.Unpressed)
                    {
                        character.AnimatorTurnBack();
                    }
                }
            };
            _moveRightEvent = (state) =>
            {
                if (state == GameInstance.Get().InputSystem.CurrentState[ControlAction.MoveLeft])
                {
                    character.AnimatorStopMove();
                }
                else
                {
                    character.AnimatorStartMove();
                    if (GameInstance.Get().GetCurrentPlayer().transform.forward == Vector3.back && state == KeyState.Pressed ||
                        GameInstance.Get().GetCurrentPlayer().transform.forward == Vector3.forward && state == KeyState.Unpressed)
                    {
                        character.AnimatorTurnBack();
                    }
                }
            };

            _jumpEvent = (state) =>
            {
                if (state == KeyState.Pressed)
                {
                    _switchToInAir = true;
                    _stateTransferObjects.InitialVertialSpeed = GameInstance.Get().GetCharacterConfigByString("FirstJumpForce");
                    _character.AnimatorStartJump();
                }
            };
        }

        public override Vector3 OnAnimatorMove(Vector3 moveVector)
        {
            return moveVector;
        }
    }
}