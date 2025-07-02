using Unity.VisualScripting;
using UnityEngine;

public class TestEnemyBehaviour : MonoBehaviour
{
    public float AttackTimer = 0f;
    public float AttackIntervalTime = 5f;
    public bool IsInAttackRange = false;

    private AIController controller;

    #region Movement & Attacking Fields
    [SerializeField] private float _playerFollowRange = 10f;
    [SerializeField] private float _minDistToPlayer = 4f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _attackRange = 5f;
    #endregion

    #region Player related Fields
    [SerializeField] private PlayerWorldInfo _playerWorldInfo;
    private Vector3 _playerPos;
    private PlayerStats _playerStats;
    #endregion

    //Booleans for animation
    public bool IsWalking = false;


    #region Set Up & Clean Up
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
    #endregion

    #region Movement
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
            IsWalking = true;
        } else
        {
            IsWalking = false;
        }

        IsInAttackRange = distToPlayer <= _attackRange ? true : false;

    }
    private void GetPlayerPosition(Vector3 pos)
    {
        _playerPos = pos;
    }
    #endregion


    #region Attack
    public void AttackPlayer(float damage)
    {
        AttackTimer = Time.time;
        _playerStats.InflictDamage(damage);
    }
    #endregion














    //Not used right now but might be usefull later
    private Vector3 RandomVector(float xMin, float xMax, float zMin, float zMax)
    {
        float xVal = Random.Range(xMin, xMax);
        float zVal = Random.Range(zMin, zMax);

        return new Vector3(xVal, 0f, zVal);
    }

}
