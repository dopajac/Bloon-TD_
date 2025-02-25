using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloonSpawner : MonoBehaviour
{
    [SerializeField] public List<GameObject> BloonPrefabs;  // 첫 번째 그룹 (Bloons1)
    [SerializeField] public List<GameObject> BloonPrefabs2; // 두 번째 그룹 (Bloons2)

    private Transform parentTransform1, parentTransform2;
    private Dictionary<int, Queue<GameObject>> bloonPools1 = new Dictionary<int, Queue<GameObject>>(); // BloonPrefabs용 풀
    private Dictionary<int, Queue<GameObject>> bloonPools2 = new Dictionary<int, Queue<GameObject>>(); // BloonPrefabs2용 풀

    [SerializeField] private int poolSize = 10; // 한 종류당 10개씩 미리 생성

    private void Awake()
    {
        parentTransform1 = new GameObject("Bloons1").transform;
        parentTransform2 = new GameObject("Bloons2").transform;
        InitializePool(BloonPrefabs, bloonPools1, parentTransform1);
        InitializePool(BloonPrefabs2, bloonPools2, parentTransform2);
    }

    private void InitializePool(List<GameObject> prefabs, Dictionary<int, Queue<GameObject>> pool, Transform parent)
    {
        foreach (var prefab in prefabs)
        {
            int level = prefab.GetComponent<BloonManager>().level;

            if (!pool.ContainsKey(level))
            {
                pool[level] = new Queue<GameObject>();
            }

            for (int i = 0; i < poolSize; i++)
            {
                GameObject bloon = Instantiate(prefab, new Vector3(-20, -20, 0), Quaternion.identity, parent);
                bloon.SetActive(false);
                pool[level].Enqueue(bloon);
            }
        }
    }

    public GameObject GetPooledBloon(int level, bool isFromFirstGroup)
    {
        Dictionary<int, Queue<GameObject>> targetPool = isFromFirstGroup ? bloonPools1 : bloonPools2;

        if (targetPool.ContainsKey(level) && targetPool[level].Count > 0)
        {
            GameObject bloon = targetPool[level].Dequeue();
            bloon.SetActive(true);
            return bloon;
        }
        else
        {
            Debug.LogWarning($"No available bloons of level {level} in {(isFromFirstGroup ? "BloonPrefabs" : "BloonPrefabs2")}. Creating a new one.");
            return CreateNewBloon(level, isFromFirstGroup);
        }
    }

    private GameObject CreateNewBloon(int level, bool isFromFirstGroup)
    {
        List<GameObject> prefabs = isFromFirstGroup ? BloonPrefabs : BloonPrefabs2;
        Dictionary<int, Queue<GameObject>> targetPool = isFromFirstGroup ? bloonPools1 : bloonPools2;
        Transform parent = isFromFirstGroup ? parentTransform1 : parentTransform2;

        GameObject prefab = prefabs.Find(b => b.GetComponent<BloonManager>().level == level);

        if (prefab != null)
        {
            GameObject newBloon = Instantiate(prefab, new Vector3(-20, -20, 0), Quaternion.identity, parent);
            newBloon.SetActive(false);

            if (!targetPool.ContainsKey(level))
            {
                targetPool[level] = new Queue<GameObject>();
            }

            targetPool[level].Enqueue(newBloon);
            return newBloon;
        }
        else
        {
            Debug.LogError($"No prefab found for bloon level {level} in {(isFromFirstGroup ? "BloonPrefabs" : "BloonPrefabs2")}.");
            return null;
        }
    }

    public void ReturnToPool(GameObject balloon, bool isFromFirstGroup)
    {
        int level = balloon.GetComponent<BloonManager>().level;
        Dictionary<int, Queue<GameObject>> targetPool = isFromFirstGroup ? bloonPools1 : bloonPools2;

        if (!targetPool.ContainsKey(level))
        {
            Debug.LogError($"Trying to return a bloon of level {level}, but no pool exists in {(isFromFirstGroup ? "BloonPrefabs" : "BloonPrefabs2")}.");
            return;
        }

        balloon.SetActive(false);
        balloon.transform.position = new Vector3(-20, -20, 0);
        targetPool[level].Enqueue(balloon);
    }
}
