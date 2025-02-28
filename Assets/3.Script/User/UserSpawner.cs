using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Prefab;

    public void UserSpawn(Vector2 spawnPosition)
    {
        Instantiate(Prefab, new Vector3(spawnPosition.x, spawnPosition.y, 0), Quaternion.identity);
        GameManager.instance.meso -= 100;
    }
}
