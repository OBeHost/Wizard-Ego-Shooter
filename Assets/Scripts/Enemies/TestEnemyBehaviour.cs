using Unity.VisualScripting;
using UnityEngine;

public class TestEnemyBehaviour : MonoBehaviour
{
    public float AttackTimer = 0f;
    public float AttackIntervalTime = 5f;
    public bool IsInAttackRange = false;

    [SerializeField] private PlayerWorldInfo _playerWorldInfo;
    [SerializeField] private float _playerFollowRange = 10f;
    [SerializeField] private float _minDistToPlayer = 4f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _attackRange = 5f;


    private AIController controller;
    private Vector3 _playerPos;
    private PlayerStats _playerStats;
    


    public void Subscribe(AIController controller)
    {
        this.controller = controller;
        controller.MoveEvent += Move;
        controller.GetPlayerPositionEvent += GetPlayerPosition;
    }

    private void Unsubscribe()
    {
        if (controller == null) return;
        controller.MoveEvent -= Move;
    }

    private void Awake()
    {
        this._playerStats = _playerWorldInfo.PlayerStats;
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    //TODO: Clean up this behaviour 
    public void Move(float dist)
    {
        //Right now all the agent does is move directly towards the player 
        //This is just place holder
        //Later, more complex behaviour should be determined by the distance to the player 
        Debug.DrawRay(this.transform.position, this.transform.forward * 10, Color.red);


        if (_playerPos == null) return;

        Vector3 moveDir = _playerPos - this.transform.position;
        moveDir = new Vector3(moveDir.x, 0f, moveDir.z);

        float distToPlayer = moveDir.magnitude;

        Vector3 lookAtTarget = new Vector3(_playerPos.x, this.transform.position.y, _playerPos.z);
        this.transform.LookAt(lookAtTarget);

        if (dist <= _playerFollowRange && distToPlayer >= _minDistToPlayer)
        {
            transform.Translate(moveDir.normalized * _moveSpeed * Time.deltaTime, Space.World);
        }

        if (distToPlayer <= _attackRange)
        {
            IsInAttackRange = true;
        } else
        {
            IsInAttackRange = false;
        }
    }
    private void GetPlayerPosition(Vector3 pos)
    {
        _playerPos = pos;
    }

    public void AttackPlayer(float damage)
    {
        AttackTimer = Time.time;
        _playerStats.InflictDamage(damage);
    }














    //Not used right now but might be usefull later
    private Vector3 RandomVector(float xMin, float xMax, float zMin, float zMax)
    {
        float xVal = Random.Range(xMin, xMax);
        float zVal = Random.Range(zMin, zMax);

        return new Vector3(xVal, 0f, zVal);
    }

}
