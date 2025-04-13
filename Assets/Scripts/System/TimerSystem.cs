using System;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectHH
{
    public class Timer
    {
        public int Handle { get; set; }
        public float Time { get; set; }
        public Action Action { get; set; }
    }

    public class TimerSystem : GameSystemBase
    {
        #region 生命周期

        private Dictionary<int, Timer> _timerMap = new();
        private int _currentHandle = 0;

        protected override void OnInit()
        {
        }

        protected override void OnUpdate()
        {
            List<int> removeList = new();
            foreach (var timer in _timerMap)
            {
                Timer t = timer.Value;
                t.Time -= Time.deltaTime;
                if (t.Time <= 0)
                {
                    t.Action?.Invoke();
                    removeList.Add(t.Handle);
                }
            }

            foreach (var handle in removeList)
            {
                _timerMap.Remove(handle);
            }
        }

        #endregion


        #region 外部接口

        public int AddTimer(float time, Action action)
        {
            Timer timer = new()
            {
                Handle = _currentHandle,
                Time = time,
                Action = action
            };

            _timerMap.Add(_currentHandle, timer);
            _currentHandle++;
            return timer.Handle;
        }

        #endregion
    }
}