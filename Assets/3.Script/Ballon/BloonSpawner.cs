using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloonSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> BloonsList;
    private Transform parentTransform;

    [SerializeField] private GameObject Start_1;
    [SerializeField] private GameObject Finish_1;

    private List<GameObject> spawnedBalloons = new List<GameObject>(); // ������ ǳ�� ����Ʈ
    private bool isGameStarted = false; // ���� ���� üũ

    private void Start()
    {
        parentTransform = new GameObject("Bloons").transform;

        Debug.Log(Finish_1.transform.position);

        for (int i = 0; i < BloonsList.Count; i++)
        {
            Transform groupParent = new GameObject(BloonsList[i].name + "s").transform;
            groupParent.SetParent(parentTransform);

            for (int j = 0; j < 10; j++)// ǳ�� ��ȯ ����
            {
                // ó������ ȭ�� ��(-20, -20)�� ��ġ
                GameObject balloon = Instantiate(BloonsList[i], new Vector3(-20, -20, 0), Quaternion.identity, groupParent);
                balloon.SetActive(false); // ó������ ��Ȱ��ȭ
                spawnedBalloons.Add(balloon);
            }
        }
    }

    public void StartGame()
    {
        if (isGameStarted) return; // �̹� �����ߴٸ� �ߺ� ���� ����

        isGameStarted = true;
        StartCoroutine(SendBalloonsToStart());
    }

    private IEnumerator SendBalloonsToStart()
    {
        foreach (GameObject balloon in spawnedBalloons)
        {
            Debug.Log("start" + Start_1.transform.position);
            balloon.transform.position = Start_1.transform.position; // Start ��ġ�� �̵�
            balloon.SetActive(true); // Ȱ��ȭ
        }

        yield return new WaitForSeconds(2f); // ��� ǳ���� Start ��ġ�� �����ϴ� �ð�

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

                yield return new WaitForSeconds(0.5f); // ���� ����
            }
        }
    }
}

