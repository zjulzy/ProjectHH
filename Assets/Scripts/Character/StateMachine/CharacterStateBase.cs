using Unity.VisualScripting;
using UnityEngine;
using Gizmos = Popcron.Gizmos;

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
        ClimbUp = CharacterMoveType.InAir << 4 + 3, // 跳跃过程中碰到墙的可以爬上墙
    }

    public abstract class CharacterStateBase
    {
        protected TestCharacter _character;
        protected Vector3 CharacterForward => _character.transform.forward;

        protected CharacterStateBase()
        {
            throw new System.NotImplementedException();
        }

        public CharacterStateBase(TestCharacter character)
        {
            _character = character;
        }

        public void Enter(StateTransferObjects stateTransferObj)
        {
            OnEnterState(stateTransferObj);
            _stateTransferObjects = default;
        }

        public void Update()
        {
            OnUpdate();
        }

        public void Exit()
        {
            OnExitState();
        }

        protected bool GetIsOnGround()
        {
            var groundDetectionDistance = GameInstance.Get().GetCharacterConfigByString("GroundDetectionDistance");
            var startPos = _character.transform.position;
            // 后续需要增大向下打的射线的长度，延迟更新jumpstate
            Gizmos.Line(startPos + Vector3.up * groundDetectionDistance, startPos + Vector3.down * groundDetectionDistance, Color.red);

            var result = Physics.RaycastAll(startPos + Vector3.up * groundDetectionDistance, Vector3.down, groundDetectionDistance * 2,
                layerMask: LayerMask.GetMask("Default"));
            return result.Length > 0;
        }


        #region 子类实现方法

        public struct StateTransferObjects
        {
            public float InitialVertialSpeed;
        }

        protected StateTransferObjects _stateTransferObjects;

        public abstract (CharacterMoveType, StateTransferObjects) CheckSwitchState();

        protected abstract void OnEnterState(StateTransferObjects stateTransferObj);

        protected abstract void OnUpdate();

        protected abstract void OnExitState();

        // 基于动画给出的位移，返回利用业务状态修正后的位移向量
        public abstract Vector3 OnAnimatorMove(Vector3 moveVector);

        #endregion
    }
}