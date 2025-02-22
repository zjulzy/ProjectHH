using Unity.VisualScripting;
using UnityEngine.PlayerLoop;

namespace ProjectHH.StateMachine
{
    // 与状态机对应的移动种类
    public enum CharacterMoveType
    {
        None,
        Move,
        InAir
    }

    // 细分的角色运动状态
    public enum CharacterMoveState
    {
        MoveStop = CharacterMoveType.Move << 4,
        MoveWalk = CharacterMoveType.Move << 4 + 1,
        MoveRun = CharacterMoveType.Move << 4 + 2,

        StartJump = CharacterMoveType.InAir << 4,
        InAir = CharacterMoveType.InAir << 4 + 1,
        EndJump = CharacterMoveType.InAir << 4 + 2,
    }

    public abstract class CharacterStateBase
    {
        protected TestCharacter _character;
        public CharacterStateBase(TestCharacter character)
        {
            _character = character;

        }
        public void Enter()
        {
            OnEnterState();
        }

        public void Update()
        {
            OnUpdate();
        }


        #region 子类实现方法

        public abstract CharacterMoveType CheckSwitchState();

        protected abstract void OnEnterState();

        protected abstract void OnUpdate();

        #endregion
    }
}