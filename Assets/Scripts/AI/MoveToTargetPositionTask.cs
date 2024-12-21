using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{
    public class MoveToTargetPositionTask : ActionTask<TestEnemy>
    {
        [RequiredField] public BBParameter<Vector3> TargetPosition;
        
        
        protected override void OnExecute()
        {
            
        }

        protected override void OnUpdate()
        {
            if (TargetPosition.value == default)
            {
                EndAction(false);
            }
            else
            {
                agent.MoveTowardsPosition(TargetPosition.value);
            }
        }

        //Called when the task is disabled.
        protected override void OnStop()
        {
        }

        //Called when the task is paused.
        protected override void OnPause()
        {
        }
    }
}