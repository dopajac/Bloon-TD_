using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloonSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> BloonsList;
    private Transform parentTransform;

    [SerializeField] private GameObject Start_1;
    [SerializeField] private GameObject Finish_1;

    private List<GameObject> spawnedBalloons = new List<GameObject>(); // 생성된 풍선 리스트
    private bool isGameStarted = false; // 시작 여부 체크

    private void Start()
    {
        parentTransform = new GameObject("Bloons").transform;

        Debug.Log(Finish_1.transform.position);

        for (int i = 0; i < BloonsList.Count; i++)
        {
            Transform groupParent = new GameObject(BloonsList[i].name + "s").transform;
            groupParent.SetParent(parentTransform);

            for (int j = 0; j < 10; j++)// 풍선 소환 수량
            {
                // 처음에는 화면 밖(-20, -20)에 배치
                GameObject balloon = Instantiate(BloonsList[i], new Vector3(-20, -20, 0), Quaternion.identity, groupParent);
                balloon.SetActive(false); // 처음에는 비활성화
                spawnedBalloons.Add(balloon);
            }
        }
    }

    public void StartGame()
    {
        if (isGameStarted) return; // 이미 시작했다면 중복 실행 방지

        isGameStarted = true;
        StartCoroutine(SendBalloonsToStart());
    }

    private IEnumerator SendBalloonsToStart()
    {
        foreach (GameObject balloon in spawnedBalloons)
        {
            Debug.Log("start" + Start_1.transform.position);
            balloon.transform.position = Start_1.transform.position; // Start 위치로 이동
            balloon.SetActive(true); // 활성화
        }

        yield return new WaitForSeconds(2f); // 모든 풍선이 Start 위치에 도착하는 시간

        StartCoroutine(SendBalloonsToFinish());
    }

    private IEnumerator SendBalloonsToFinish()
    {
        foreach (GameObject balloon in spawnedBalloons)
        {
            if (balloon != null)
            {
              
                BloonMovement movement = balloon.GetComponent<BloonMovement>();
                if (movement == null)
                {
                    movement = balloon.AddComponent<BloonMovement>();
                }

              
                movement.SetTarget(Finish_1.transform.position);

                yield return new WaitForSeconds(0.5f); // 간격 조절
            }
        }
    }
}

