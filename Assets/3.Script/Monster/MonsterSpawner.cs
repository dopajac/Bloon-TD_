using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines; // Spline 관련 네임스페이스 추가
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
    
    [SerializeField] public List<GameObject> alivemonster = new List<GameObject>(); // 현재 살아있는 몬스터 목록
    [SerializeField] public List<GameObject> spawnedMonsterList = new List<GameObject>(); // **소환될 예정인 몬스터 목록 (새로 추가)**

    private bool isSpawning = false; // 중복 스폰 방지 플래그

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

            MonsterPools[level].Enqueue(newMonster); // 생성한 몬스터를 풀에 추가
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

        // monsterlist 및 alivemonster에서 제거
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
        spawnedMonsterList.Clear(); // 새로운 소환을 시작하면 초기화

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
                    spawnedMonsterList.Add(monster); // 소환될 예정 리스트에 추가
                }

                yield return new WaitForSeconds(delay);
            }
        }

        isSpawning = false;
    }

    // UI 버튼 클릭 시 실행할 함수
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

        // 1단계: 기존에 소환된 몬스터 정리
        if (spawnedMonsterList.Count > 0)
        {
            foreach (GameObject monster in spawnedMonsterList)
            {
                if (monster != null)
                {
                    ReturnToPool(monster); // 기존 몬스터 풀로 반환
                }
                
            }
        }

        spawnedMonsterList.Clear(); // 리스트 초기화

        int availableMonsters = MonsterPools.ContainsKey(level) ? MonsterPools[level].Count : 0;
        int spawnCount = Mathf.Min(availableMonsters, poolSize); // 최대 10마리 소환

        // 2단계: 새로운 몬스터 10개 먼저 소환
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject monster = GetPooledMonster(level);
            if (monster != null)
            {
                monster.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
                monster.SetActive(true);

                // spawnedMonsterList에 추가
                spawnedMonsterList.Add(monster);
            }
        }
        int ie = 0;
        // 3단계: 일정 딜레이 간격으로 spline.Play() 실행
        foreach (GameObject monster in spawnedMonsterList)
        {
            
            if (monster != null)
            {
                SplineAnimate spline = monster.GetComponent<SplineAnimate>();

                if (spline != null)
                {
                    spline.Play();
                    Debug.LogError($"[MonsterSpawner] {monster.name}에 SplineAnimate 컴포넌트가 있습니다!" + ie);
                    
                }
                else
                {
                    Debug.LogError($"[MonsterSpawner] {monster.name}에 SplineAnimate 컴포넌트가 없습니다!");
                    Debug.Log(spline.IsPlaying);

                }
                ie++;
            }
            

            yield return new WaitForSeconds(delay); // 딜레이 적용 후 다음 몬스터 실행
        }

        isSpawning = false;
    }

}
