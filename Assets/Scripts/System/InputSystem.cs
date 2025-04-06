using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectHH
{
    public enum KeyState
    {
        Unregistered, // 未注册
        Pressed, // 按下
        Unpressed, // 松开
    }

    public enum ControlAction
    {
        MoveLeft,
        MoveRight,
        Jump,
        Attack,
    }

    public class InputSystem : GameSystemBase
    {
        private Dictionary<KeyCode, ControlAction> _inputMap = new();

        public Dictionary<ControlAction, KeyState> CurrentState => _currentState;
        public Dictionary<ControlAction, KeyState> _currentState = new();
        private Dictionary<ControlAction, List<Action<KeyState>>> _eventMap = new();

        protected override void OnInit()
        {
            GameInstance.Get().GetInputConfig(ref _inputMap);

            // 遍历所有的ControlAction，初始化为CurrentState
            foreach (var control in Enum.GetValues(typeof(ControlAction)))
            {
                if (_inputMap.ContainsValue((ControlAction)control))
                {
                    _currentState.Add((ControlAction)control, KeyState.Unpressed);
                }
                else
                {
                    _currentState.Add((ControlAction)control, KeyState.Unregistered);
                }
            }
        }

        protected override void OnUpdate()
        {
            foreach (var key in _inputMap.Keys)
            {
                var control = _inputMap[key];
                if (Input.GetKeyDown(key))
                {
                    Debug.Log($"Key {key} pressed");
                    if (_currentState[control] == KeyState.Unpressed)
                    {
                        InvokeEvent(control, KeyState.Pressed);
                        _currentState[control] = KeyState.Pressed;
                    }
                }

                if (Input.GetKeyUp(key))
                {
                    Debug.Log($"Key {key} unpressed");
                    if (_currentState[control] == KeyState.Pressed)
                    {
                        InvokeEvent(control, KeyState.Unpressed);
                        _currentState[control] = KeyState.Unpressed;
                    }
                }
            }
        }

        public void RegisterEvent(ControlAction control, Action<KeyState> callBack)
        {
            if (!_eventMap.ContainsKey(control))
            {
                _eventMap.Add(control, new List<Action<KeyState>>());
            }

            _eventMap[control].Add(callBack);
        }
        
        public void UnregisterEvent(ControlAction control, Action<KeyState> callBack)
        {
            if (_eventMap.ContainsKey(control))
            {
                _eventMap[control].Remove(callBack);
            }
        }

        public void InvokeEvent(ControlAction control, KeyState state)
        {
            if (_eventMap.ContainsKey(control))
            {
                foreach (var action in _eventMap[control])
                {
                    action.Invoke(state);
                }
            }
        }

        public bool GetIsActionPressed(ControlAction action)
        {
            if (_currentState.ContainsKey(action))
            {
                return _currentState[action] == KeyState.Pressed;
            }
            else
            {
                Debug.LogError($"Action {action} not registered");
                return false;
            }
        }
        
        public int GetRealHorizontalInput()
        {
            int result = 0;
            if (GetIsActionPressed(ControlAction.MoveLeft))
            {
                result -= 1;
            }

            if (GetIsActionPressed(ControlAction.MoveRight))
            {
                result += 1;
            }

            return result;
        }
    }
}