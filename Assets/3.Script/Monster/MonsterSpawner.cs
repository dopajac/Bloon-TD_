using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines; // Spline ���� ���ӽ����̽� �߰�
public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] public List<GameObject> MonsterPrefabs;

    private Transform parentTransform;
    private SplineAnimate spline;
    private Dictionary<int, Queue<GameObject>> MonsterPools = new Dictionary<int, Queue<GameObject>>();

    [SerializeField] private int poolSize = 10;
    [SerializeField] public float spawnDelay = 2.0f;
    [SerializeField] private int currentLevel = 3;
    [SerializeField] public GameObject finishobj;
    
    [SerializeField] public List<GameObject> alivemonster = new List<GameObject>(); // ���� ����ִ� ���� ���
    [SerializeField] public List<GameObject> spawnedMonsterList = new List<GameObject>(); // **��ȯ�� ������ ���� ��� (���� �߰�)**

    private bool isSpawning = false; // �ߺ� ���� ���� �÷���

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

            if (!alivemonster.Contains(Monster))
            {
                alivemonster.Add(Monster);
            }

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

            MonsterPools[level].Enqueue(newMonster); // ������ ���͸� Ǯ�� �߰�
            return newMonster;
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

        // monsterlist �� alivemonster���� ����
        alivemonster.Remove(Monster);

        Monster.SetActive(false);
        Monster.transform.position = new Vector3(-20, -20, 0);
        MonsterPools[level].Enqueue(Monster);
    }

    public void SpawnAllMonstersWithInterval(int level, float delay)
    {
        if (!isSpawning)
        {
            StartCoroutine(SpawnAllWithInterval(level, delay));
        }
    }

    private IEnumerator SpawnAllWithInterval(int level, float delay)
    {
        isSpawning = true;
        spawnedMonsterList.Clear(); // ���ο� ��ȯ�� �����ϸ� �ʱ�ȭ

        int spawnCount = poolSize;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject monster = GetPooledMonster(level);
            if (monster != null)
            {
                monster.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
                monster.SetActive(true);


                if (!spawnedMonsterList.Contains(monster))
                {
                    spawnedMonsterList.Add(monster); // ��ȯ�� ���� ����Ʈ�� �߰�
                }

                yield return new WaitForSeconds(delay);
            }
        }

        isSpawning = false;
    }

    // UI ��ư Ŭ�� �� ������ �Լ�
    public void OnSpawnButtonClick()
    {
        //SpawnAllMonstersWithInterval(currentLevel, spawnDelay);
        SpawnAllMonstersAndPlaySpline(currentLevel, spawnDelay);
    }
    public void SpawnAllMonstersAndPlaySpline(int level, float delay)
    {
        if (!isSpawning)
        {
            StartCoroutine(SpawnAndPlaySplineCoroutine(level, delay));
        }
    }

    private IEnumerator SpawnAndPlaySplineCoroutine(int level, float delay)
    {
        isSpawning = true;

        // 1�ܰ�: ������ ��ȯ�� ���� ����
        if (spawnedMonsterList.Count > 0)
        {
            foreach (GameObject monster in spawnedMonsterList)
            {
                if (monster != null)
                {
                    ReturnToPool(monster); // ���� ���� Ǯ�� ��ȯ
                }
                
            }
        }

        spawnedMonsterList.Clear(); // ����Ʈ �ʱ�ȭ

        int availableMonsters = MonsterPools.ContainsKey(level) ? MonsterPools[level].Count : 0;
        int spawnCount = Mathf.Min(availableMonsters, poolSize); // �ִ� 10���� ��ȯ

        // 2�ܰ�: ���ο� ���� 10�� ���� ��ȯ
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject monster = GetPooledMonster(level);
            if (monster != null)
            {
                monster.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
                monster.SetActive(true);

                // spawnedMonsterList�� �߰�
                spawnedMonsterList.Add(monster);
            }
        }
        int ie = 0;
        // 3�ܰ�: ���� ������ �������� spline.Play() ����
        foreach (GameObject monster in spawnedMonsterList)
        {
            
            if (monster != null)
            {
                SplineAnimate spline = monster.GetComponent<SplineAnimate>();

                if (spline != null)
                {
                    spline.Play();
                    Debug.LogError($"[MonsterSpawner] {monster.name}�� SplineAnimate ������Ʈ�� �ֽ��ϴ�!" + ie);
                    
                }
                else
                {
                    Debug.LogError($"[MonsterSpawner] {monster.name}�� SplineAnimate ������Ʈ�� �����ϴ�!");
                    Debug.Log(spline.IsPlaying);

                }
                ie++;
            }
            

            yield return new WaitForSeconds(delay); // ������ ���� �� ���� ���� ����
        }

        isSpawning = false;
    }

}
