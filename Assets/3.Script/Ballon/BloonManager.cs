using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class BloonManager : MonoBehaviour
{
    [SerializeField] public int level; // ǳ�� ����
    [SerializeField] public int hp;
    [SerializeField] public float speed;

    private BloonSpawner bloonSpawner;
    [SerializeField] private SplineContainer spline;
    
    private float progress = 0f; // ���� ���� ���� (0 ~ 1)

    // �ش� ǳ���� BloonPrefabs���� �����Ǿ����� ����
    public bool isFromFirstGroup;
    [SerializeField] bool iscamo;
    [SerializeField] bool isleab;
    [SerializeField] bool ismoab;

    private void Start()
    {
        bloonSpawner = GameObject.Find("BloonSpawner").GetComponent<BloonSpawner>();
        StartCoroutine(MoveAlongSpline());
    }

    private void Update()
    {
        if (hp <= 0)
        {
            HandleBloonDestruction();
        }
    }

    // ü�� ���� �޼���
    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            HandleBloonDestruction();
        }
    }

    private void HandleBloonDestruction()
    {
        switch (level)
        {
            case 18: // MOAB
                SpawnNewBloons(4, 17);
                break;
            case 17: //��
                SpawnNewBloons(2, 16);
                break;
            case 16: //��
                SpawnNewBloons(2, 13);
                break;
            case 15: //�� ī��
                SpawnNewBloons(2, 12);
                break;
            case 14: //��
                SpawnNewBloons(2, 11);
                break;
            case 13: //��
                SpawnNewBloons(2, 11);
                break;
            case 12: // �� ī��
                SpawnNewBloons(2, 10);
                break;
            case 11: //��
                SpawnNewBloons(2, 9);
                break;
            case 10://�� ī��
                SpawnNewBloons(1, 8);
                break;
            case 9: //��
                SpawnNewBloons(1, 7);
                break;
            case 8: //�� ī��
                SpawnNewBloons(1, 6);
                break;
            case 7: //��
                SpawnNewBloons(1, 5);
                break;
            case 6: // �� ī��
                SpawnNewBloons(1, 4);
                break;
            case 5: //��
                SpawnNewBloons(1, 3);
                break;
            case 4:// �� ī��
                SpawnNewBloons(1, 2);
                break;
            case 3: // �� 
                SpawnNewBloons(1, 1);
                break;

            default:
                bloonSpawner.ReturnToPool(gameObject, isFromFirstGroup);
                break;
        }
    }

    private void SpawnNewBloons(int count, int newLevel)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newBloon = bloonSpawner.GetPooledBloon(newLevel, isFromFirstGroup);
            if (newBloon != null)
            {
                BloonManager newBloonManager = newBloon.GetComponent<BloonManager>();
                newBloon.transform.position = transform.position; // ���� ��ġ ����
                //newBloonManager.hp = newLevel;
                newBloonManager.progress = this.progress; // ���� ���� ���� ����
                newBloonManager.isFromFirstGroup = this.isFromFirstGroup; // ���� �׷� ����
                newBloon.SetActive(true);
            }
        }
        bloonSpawner.ReturnToPool(gameObject, isFromFirstGroup);
    }

    // ǳ���� ���ö����� ���� �̵�
    private IEnumerator MoveAlongSpline()
    {
        while (progress < 1f)
        {
            transform.position = spline.EvaluatePosition(progress);
            progress += Time.deltaTime * speed * 0.1f; // �ӵ� ����
            yield return null;
        }
        bloonSpawner.ReturnToPool(gameObject, isFromFirstGroup);
    }
}
