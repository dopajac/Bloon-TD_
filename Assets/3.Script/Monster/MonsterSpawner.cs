using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] public List<GameObject> MonsterPrefabs;

    [SerializeField] public float spawnDelay = 1.0f; // 1초마다 스폰
    [SerializeField] private int currentLevel;
    [SerializeField] private int monstercount;
    [SerializeField] public List<GameObject> spawnedMonsterList = new List<GameObject>(); // 소환된 몬스터 리스트
    [SerializeField] public List<GameObject> alivemonster = new List<GameObject>(); // 살아있는 몬스터 리스트

    [SerializeField] public GameObject finishobj;
    [SerializeField] private Transform monsterParent; // 몬스터 리스트 부모 오브젝트

    public int cur_mostercount = 0;

    private bool isSpawning = false; // 중복 스폰 방지 플래그

    private void Awake()
    {
        // "MonsterList"라는 이름의 부모 오브젝트를 생성 (이미 있으면 사용)
        if (monsterParent == null)
        {
            GameObject monsterListObj = GameObject.Find("MonsterList");
            if (monsterListObj == null)
            {
                monsterListObj = new GameObject("MonsterList");
            }
            monsterParent = monsterListObj.transform;
        }
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            StartCoroutine(SpawnMonsterWithDelay());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    private IEnumerator SpawnMonsterWithDelay()
    {
        isSpawning = true;

        while (isSpawning)
        {
            if (cur_mostercount < monstercount) 
            {
                SpawnMonster(currentLevel);
                cur_mostercount++;
            }
            else
            {
                Debug.Log("최대 몬스터 개수(10)에 도달하여 스폰 중지");
                StopSpawning();
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnMonster(int level)
    {
        GameObject prefab = MonsterPrefabs.Find(b => b.GetComponent<MonsterManager>().level == level);
        if (prefab != null)
        {
            GameObject monster = Instantiate(prefab, new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0), Quaternion.identity);

           
            monster.transform.SetParent(monsterParent, false);

            monster.SetActive(true);
            spawnedMonsterList.Add(monster); 
            Debug.Log($"[MonsterSpawner] 몬스터 스폰됨: {monster.name}");
        }
    }
}
