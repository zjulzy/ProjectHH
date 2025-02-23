using System;
using UnityEngine;

namespace ProjectHH.StateMachine
{
    public class CharacterStateMove : CharacterStateBase
    {
        public override CharacterMoveType CheckSwitchState()
        {
            return CharacterMoveType.None;
        }

        protected override void OnEnterState()
        {
            GameInstance.Get().InputSystem.RegisterEvent(ControlAction.MoveLeft, _moveLeftEvent);
            GameInstance.Get().InputSystem.RegisterEvent(ControlAction.MoveRight, _moveRightEvent);
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
                    if(GameInstance.Get().GetCurrentPlayer().transform.forward == Vector3.forward)
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
                    if(GameInstance.Get().GetCurrentPlayer().transform.forward == Vector3.back)
                    {
                        character.AnimatorTurnBack();
                    }
                }
            };
        }
    }
}