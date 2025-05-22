using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] _pos;

    private int _zombieSpawnCount = 50;

    void Start()
    {
        for (int i = 0; i < _zombieSpawnCount; ++i)
        {
            Transform zomPos = _pos[Random.Range(0, _pos.Length)].transform;
            CommonEnemy enemy = PoolManager.Instance.Pop(ObjectPooling.PoolingType.Enemy) as CommonEnemy;
            enemy.gameObject.transform.position = zomPos.position;
        }
        StartCoroutine(ZombieSpawn());
    }

    private IEnumerator ZombieSpawn()
    {
        yield return new WaitForSeconds(8f);
        for (int i = 0; i < 5; ++i)
        {
            Transform zomPos = _pos[Random.Range(0, _pos.Length)].transform;
            CommonEnemy enemy = PoolManager.Instance.Pop(ObjectPooling.PoolingType.Enemy) as CommonEnemy;
            enemy.gameObject.transform.position = zomPos.position;
        }
    }
}
