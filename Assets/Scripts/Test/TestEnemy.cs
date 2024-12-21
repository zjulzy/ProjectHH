
using ProjectHH;
using Unity.Mathematics;
using UnityEngine;

public class TestEnemy : CharacterBase
{
    private Vector3 _direction;
    private float _speed;
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    // 由行为树调用，向目标位置移动，具体移动速度取决于敌人种类
    public bool MoveTowardsTarget(GameObject target)
    {
        _direction = target.transform.position - transform.position;
        _direction.y = 0;
        _direction = math.normalizesafe(_direction);
        if (_direction != default)
        {
            transform.rotation = Quaternion.LookRotation(_direction);
            _speed = 1;
            transform.position += _direction * _speed * Time.deltaTime;
        }
        
        return true;
    }

    public bool MoveTowardsPosition(Vector3 position)
    {
        _direction = position - transform.position;
        _direction.y = 0;
        _direction = math.normalizesafe(_direction);
        if (_direction != default)
        {
            transform.rotation = Quaternion.LookRotation(_direction);
            _speed = 1;
            transform.position += _direction * _speed * Time.deltaTime;
        }
        
        return true;
    }
}
