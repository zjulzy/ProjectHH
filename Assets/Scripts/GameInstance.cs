using UnityEngine;

// 单例游戏示例对象
public class GameInstance
{
    private static GameInstance _instance;
    private Transform _currentPlayer = null;

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

    private GameInstance()
    {
    }
    
    # region 对外接口
    public Transform GetCurrentPlayer()
    {
        if(_currentPlayer == null)
        {
            _currentPlayer = GameObject.FindWithTag("Player").transform;
        }
        return _currentPlayer;
    }
    # endregion
}