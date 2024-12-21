using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{
    // 每帧必定调用，AI敌人寻找追踪敌人或者目标
    public class SeekTarget : ActionTask<TestEnemy>
    {
        [RequiredField] public BBParameter<float> MaxSearchDistance;
        [RequiredField] public BBParameter<GameObject> Target;
        [RequiredField] public BBParameter<Vector3> TargetPosition;
        

        protected override void OnExecute()
        {
        }

        protected override void OnUpdate()
        {
            // 首先尝试寻找敌人
            Transform currentPlayer = GameInstance.Get().GetCurrentPlayer();
            if (Mathf.Abs(currentPlayer.position.y - agent.transform.position.y) < 0.1f &&
                MaxSearchDistance.value >= (currentPlayer.transform.position - agent.transform.position).magnitude)
            {
                // 通过射线检测查看是否有阻挡
                Vector3 targetPos = currentPlayer.position + Vector3.up * 0.5f;
                RaycastHit hit;
                if (Physics.Linecast(agent.transform.position, targetPos, out hit, LayerMask.GetMask("Default")))
                {
                    Target.value = null;
                }
                else
                {
                    Target.value = currentPlayer.gameObject;
                    TargetPosition.value = default;
                    return;
                }
            }
            else
            {
                Target.value = null;
            }

            // 如果找不到敌人，检测前方能不能走
            if (Target.value == null)
            {
                var startPos = agent.transform.position + Vector3.up * 0.5f;
                if (!Physics.Raycast(startPos, agent.transform.forward, out _, 0.1f, LayerMask.GetMask("Default")) &&
                    Physics.Raycast(startPos + 0.1f * agent.transform.forward, Vector3.down, 0.51f, 1 << LayerMask.NameToLayer("Default")))
                {
                    TargetPosition.value = agent.transform.position + agent.transform.forward * 0.1f;
                }
                else
                {
                    agent.transform.forward = -agent.transform.forward;
                    TargetPosition.value = agent.transform.position;
                }
            }
        }


        protected override void OnStop()
        {
        }

        protected override void OnPause()
        {
        }
    }
}