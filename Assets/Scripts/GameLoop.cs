using System;
using UnityEngine;

namespace ProjectHH
{
    public class GameLoop : MonoBehaviour
    {
        public HimeHinaConfig MainConfig;
        
        // GameLoop的awake被最先调用
        // 在GameLoop的update中调用GameInstance的update
        private void Awake()
        {
            var gameInstance = GameInstance.Get();
            gameInstance.Init(MainConfig);
        }

        private void Update()
        {
            var gameInstance = GameInstance.Get();
            gameInstance.Update();
        }
    }
 
}
