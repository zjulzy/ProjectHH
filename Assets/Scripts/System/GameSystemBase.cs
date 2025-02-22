using UnityEngine;

namespace ProjectHH
{
    // 单例游戏示例对象
    public abstract class GameSystemBase
    {
        public void Init()
        {
            OnInit();
        }
        
        public void Update()
        {
            OnUpdate();
        }

        protected abstract void OnInit();

        protected abstract void OnUpdate();


    }
}

