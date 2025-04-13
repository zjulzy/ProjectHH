using UnityEngine;

namespace ProjectHH.StateMachine
{
    public class CharacterStateUncontrollable: CharacterStateBase
    {
        public CharacterStateUncontrollable(TestCharacter character) : base(character)
        {
        }
        
        public override (CharacterMoveType, StateTransferObjects) CheckSwitchState()
        {
            return (CharacterMoveType.None, default);
        }
        
        protected override void OnEnterState(StateTransferObjects stateTransferObj)
        {
            
        }

        protected override void OnExitState()
        {
            
        }

        protected override void OnUpdate()
        {
            return;
        }

        public override Vector3 OnAnimatorMove(Vector3 moveVector)
        {
            return moveVector;
        }
    }
}