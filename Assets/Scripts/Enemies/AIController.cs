using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class AIController : MonoBehaviour
{

    #region Events
    public event UnityAction<float> MoveEvent;
    public event UnityAction<Vector3> GetPlayerPositionEvent;
    #endregion

    #region Enemy Spawning
    [SerializeField] private float _spawnXBound = 5f;
    [SerializeField] private float _spawnZBound = 5f;
    [SerializeField] private GameObject _enemyPrefab;

    private GameObject SpawnEnemy()
    {
        Vector3 tempSpawnPos = new Vector3(this.transform.position.x, 1f, this.transform.position.z);
        return Instantiate(_enemyPrefab, tempSpawnPos, Quaternion.identity);
    }
    #endregion

    #region Enemy Subscription
    //Subscribe every enemy to the event that is invoked every update
    [SerializeField] private List<TestEnemyBehaviour> _enemies;
    private void OnEnable()
    {
        GameObject en = SpawnEnemy();
        TestEnemyBehaviour e;
        en.TryGetComponent<TestEnemyBehaviour>(out e);

        if (e == null) return;

        _enemies.Add(e);
        e.Subscribe(this.GetComponent<AIController>());
    }
    #endregion

    #region Check Player Distance
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _playerFollowRange = 4f;

    //Returns the distance of the player to the spawn center
    private float CheckPlayerDistance()
    {
        RaycastHit hit;
        Vector3 rayFrom = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
        Vector3 rayTo = new Vector3(_playerTransform.position.x, 0f, _playerTransform.position.z);
        Vector3 rayDir = rayTo - rayFrom;
        Ray ray = new Ray(rayFrom, rayDir.normalized);

        if (Physics.Raycast(ray, out hit))
        { 
            float distToPlayer = rayDir.magnitude;
        }

        return rayDir.magnitude;

    }

    private Vector3 GetPlayerPosition()
    {
        return _playerTransform.position;
    }
    #endregion


    private void Update()
    {
        float dist = CheckPlayerDistance();
        MoveEvent?.Invoke(dist);
        GetPlayerPositionEvent?.Invoke(GetPlayerPosition());


        foreach (TestEnemyBehaviour e in _enemies)
        {
            float timeSinceAttack = Time.time - e.AttackTimer;
            if (timeSinceAttack >= e.AttackIntervalTime && e.IsInAttackRange)
            {
                e.AttackPlayer(2f);
            }

        }
    }
}
