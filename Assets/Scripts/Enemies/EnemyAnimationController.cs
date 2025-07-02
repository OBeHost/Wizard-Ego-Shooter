using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator _animator;
    private TestEnemyBehaviour _enemyBehaviour;


    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _enemyBehaviour = GetComponent<TestEnemyBehaviour>();
    }

    private void Update()
    {
        if (_animator == null || _enemyBehaviour == null) return;

        if (_enemyBehaviour.IsWalking)
        {
            _animator.SetBool("IsWalking", true);
            Debug.Log("Set isWalking tot true");
        } else
        {
            _animator.SetBool("IsWalking", false);
            Debug.Log("Set isWalking tot false");
        }
    }

}
