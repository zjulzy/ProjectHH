using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ProjectHH.StateMachine
{
    public class CharacterStateInAir : CharacterStateBase
    {
        // 正为向上，负为向下
        private float _verticalSpeed;

        public override (CharacterMoveType, StateTransferObjects) CheckSwitchState()
        {
            if (GetIsOnGround() && _verticalSpeed <= 0)
                return (CharacterMoveType.Move, _stateTransferObjects);
            return (CharacterMoveType.None, default);
        }

        protected override void OnEnterState(StateTransferObjects stateTransferObj)
        {
            _verticalSpeed = stateTransferObj.InitialVertialSpeed;
        }

        protected override void OnExitState()
        {
            _verticalSpeed = 0;
            _character.AnimatorLand();
        }

        protected override void OnUpdate()
        {
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


        public CharacterStateInAir(TestCharacter character) : base(character)
        {
        }
    }
}