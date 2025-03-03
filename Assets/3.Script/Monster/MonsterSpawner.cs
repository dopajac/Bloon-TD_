using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] public List<GameObject> MonsterPrefabs;

    [SerializeField] public float spawnDelay = 1.0f; // 1�ʸ��� ����
    [SerializeField] private int currentLevel;
    [SerializeField] private int monstercount;
    [SerializeField] public List<GameObject> spawnedMonsterList = new List<GameObject>(); // ��ȯ�� ���� ����Ʈ
    [SerializeField] public List<GameObject> alivemonster = new List<GameObject>(); // ����ִ� ���� ����Ʈ

    [SerializeField] public GameObject finishobj;

    public int cur_mostercount = 0;

    private bool isSpawning = false; // �ߺ� ���� ���� �÷���

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
            if (cur_mostercount < monstercount) // 10���������� ��ȯ
            {
                SpawnMonster(currentLevel);
                cur_mostercount++;
            }
            else
            {
                Debug.Log("�ִ� ���� ����(10)�� �����Ͽ� ���� ����");
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
            monster.SetActive(true);

            spawnedMonsterList.Add(monster); // ����Ʈ�� �߰�
            Debug.Log($"[MonsterSpawner] ���� ������: {monster.name}");
        }
    }

    //public void RemoveMonster(GameObject monster)
    //{
    //    if (spawnedMonsterList.Contains(monster))
    //    {
    //        spawnedMonsterList.Remove(monster);
    //    }
    //}
}
