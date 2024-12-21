using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	[Description("朝向角色移动")]
	public class MoveToCharacterTask : ActionTask<TestEnemy> {
		[RequiredField]
		public BBParameter<GameObject> Target;
		
		
		protected override void OnExecute()
		{
			
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate()
		{
			if (Target.value == null)
			{
				EndAction(false);
			}
			else
			{
				agent.MoveTowardsTarget(Target.value);
			}
			
		}

		//Called when the task is disabled.
		protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}