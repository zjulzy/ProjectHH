using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ProjectHH.StateMachine
{
    public class CharacterStateInAir : CharacterStateBase
    {
        // 正为向上，负为向下
        private float _verticalSpeed;
        public override CharacterMoveType CheckSwitchState()
        {
            if (GetIsOnGround())
                return CharacterMoveType.Move;
            return CharacterMoveType.None;
        }

        protected override void OnEnterState()
        {
            _verticalSpeed = 5;
        }

        protected override void OnExitState()
        {
            _verticalSpeed = 0;
            _character.AnimatorLand();
        }

        protected override void OnUpdate()
        {
            var gravity = GameInstance.Get().GetCharacterConfigByString("Gravity");
            _verticalSpeed -= gravity * Time.deltaTime;
            _character.CharacterController.Move(Vector3.up * _verticalSpeed * Time.deltaTime);
        }
        
        
        public CharacterStateInAir(TestCharacter character) : base(character)
        {
            
        }
    }
}