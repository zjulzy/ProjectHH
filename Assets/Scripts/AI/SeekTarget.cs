using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions
{
    // 每帧必定调用，AI敌人寻找追踪敌人或者目标
    public class SeekTarget : ActionTask
    {
        protected override string OnInit()
        {
            return null;
        }
        
        protected override void OnExecute()
        {
        }
        
        protected override void OnUpdate()
        {
            // 首先尝试寻找敌人
            
            // 如果找不到敌人，找到当前平台离自己最远的目标点
        }
        
        protected override void OnStop()
        {
        }
        
        protected override void OnPause()
        {
        }
    }
}