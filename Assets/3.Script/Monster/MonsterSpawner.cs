using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] public List<GameObject> MonsterPrefabs;

    private Transform parentTransform;
    private Dictionary<int, Queue<GameObject>> MonsterPools = new Dictionary<int, Queue<GameObject>>();

    [SerializeField] private int poolSize = 10; // �� ������ �̸� ������ ����
    [SerializeField] private float spawnDelay = 2.0f;
    [SerializeField] private int currentLevel = 4; // level �ʱⰪ ����
    [SerializeField] public GameObject finishobj;
    private void Awake()
    {
        parentTransform = new GameObject("Monsters").transform;
        InitializePool();
    }

    private void InitializePool()
    {
        foreach (var prefab in MonsterPrefabs)
        {
            int level = prefab.GetComponent<MonsterManager>().level;

            if (!MonsterPools.ContainsKey(level))
            {
                MonsterPools[level] = new Queue<GameObject>();
            }

            for (int i = 0; i < poolSize; i++)
            {
                GameObject Monster = Instantiate(prefab, new Vector3(-20, -20, 0), Quaternion.identity, parentTransform);
                Monster.SetActive(false);
                MonsterPools[level].Enqueue(Monster);
            }
        }
    }

    public GameObject GetPooledMonster(int level)
    {
        if (MonsterPools.ContainsKey(level) && MonsterPools[level].Count > 0)
        {
            GameObject Monster = MonsterPools[level].Dequeue();
            Monster.SetActive(true);
            return Monster;
        }
        else
        {
            return CreateNewMonster(level);
        }
    }

    private GameObject CreateNewMonster(int level)
    {
        GameObject prefab = MonsterPrefabs.Find(b => b.GetComponent<MonsterManager>().level == level);

        if (prefab != null)
        {
            GameObject newMonster = Instantiate(prefab, new Vector3(-20, -20, 0), Quaternion.identity, parentTransform);
            newMonster.SetActive(false);

            if (!MonsterPools.ContainsKey(level))
            {
                MonsterPools[level] = new Queue<GameObject>();
            }

            return newMonster; // ��ȯ�� ���� �־���!
        }
        else
        {
            return null;
        }
    }

    public void ReturnToPool(GameObject Monster)
    {
        int level = Monster.GetComponent<MonsterManager>().level;

        if (!MonsterPools.ContainsKey(level))
        {
            return;
        }

        Monster.SetActive(false);
        Monster.transform.position = new Vector3(-20, -20, 0);
        MonsterPools[level].Enqueue(Monster);
    }

    public void SpawnAllMonstersWithInterval(int level, float delay)
    {
        StartCoroutine(SpawnAllWithInterval(level, delay));
    }

    private IEnumerator SpawnAllWithInterval(int level, float delay)
    {
        int spawnCount = poolSize; // ��ü ������ŭ ����

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject monster = GetPooledMonster(level);
            if (monster != null)
            {
                monster.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0); // ��ġ ����
                monster.SetActive(true);
                yield return new WaitForSeconds(delay);
            }
        }
    }

    // UI ��ư Ŭ�� �� ������ �Լ�
    public void OnSpawnButtonClick()
    {
        SpawnAllMonstersWithInterval(currentLevel, spawnDelay);
    }
}
