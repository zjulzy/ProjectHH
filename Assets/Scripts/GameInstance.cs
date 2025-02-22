using System.Collections.Generic;
using UnityEngine;

namespace ProjectHH
{
    // 单例游戏示例对象
    public class GameInstance
    {
        private static GameInstance _instance;
        private Transform _currentPlayer = null;
        private HimeHinaConfig _mainConfig;


        // 多线程🔒
        private static readonly object lockObj = new object();

        public static GameInstance Get()
        {
            lock (lockObj)
            {
                if (_instance == null)
                {
                    _instance = new GameInstance();
                }

                return _instance;
            }
        }

        public void Update()
        {
            foreach (GameSystemBase system in _systems)
            {
                system.Update();
            }
        }

        public void Init(HimeHinaConfig config)
        {
            _mainConfig = config;

            CreateSystem(out _timerSystem);
            CreateSystem(out _characterSystem);
            CreateSystem(out _inputSystem);
        }


        #region system相关

        // 拥有的system变量
        private List<GameSystemBase> _systems = new List<GameSystemBase>();

        public TimerSystem TimerSystem => _timerSystem;
        private TimerSystem _timerSystem;

        public CharacterSystem CharacterSystem => _characterSystem;
        private CharacterSystem _characterSystem;

        public InputSystem InputSystem => _inputSystem;
        private InputSystem _inputSystem;

        private void CreateSystem<T>(out T system) where T : GameSystemBase, new()
        {
            system = new T();
            system.Init();
            _systems.Add(system);
        }

        #endregion

        # region 对外接口

        public Transform GetCurrentPlayer()
        {
            if (_currentPlayer == null)
            {
                _currentPlayer = GameObject.FindWithTag("Player").transform;
            }

            return _currentPlayer;
        }

        # endregion

        # region 读取配置接口

        public void GetInputConfig(ref Dictionary<KeyCode, ControlAction> config)
        {
            config = _mainConfig.AllowedControls;
        }

        public float GetCharacterConfigByString(string key)
        {
            float result = 0;
            _mainConfig.CharacterConfig.TryGetValue(key, out result);

            return result;
        }

        #endregion
    }
}