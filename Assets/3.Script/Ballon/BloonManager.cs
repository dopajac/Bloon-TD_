using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class BloonManager : MonoBehaviour
{
    [SerializeField] public int level; // 풍선 레벨
    [SerializeField] public int hp;
    [SerializeField] public float speed;

    private BloonSpawner bloonSpawner;
    [SerializeField] private SplineContainer spline;
    
    private float progress = 0f; // 현재 진행 상태 (0 ~ 1)

    // 해당 풍선이 BloonPrefabs에서 생성되었는지 여부
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

    // 체력 감소 메서드
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
            case 17: //세
                SpawnNewBloons(2, 16);
                break;
            case 16: //무
                SpawnNewBloons(2, 13);
                break;
            case 15: //납 카모
                SpawnNewBloons(2, 12);
                break;
            case 14: //납
                SpawnNewBloons(2, 11);
                break;
            case 13: //줄
                SpawnNewBloons(2, 11);
                break;
            case 12: // 검 카모
                SpawnNewBloons(2, 10);
                break;
            case 11: //검
                SpawnNewBloons(2, 9);
                break;
            case 10://분 카모
                SpawnNewBloons(1, 8);
                break;
            case 9: //분
                SpawnNewBloons(1, 7);
                break;
            case 8: //노 카모
                SpawnNewBloons(1, 6);
                break;
            case 7: //노
                SpawnNewBloons(1, 5);
                break;
            case 6: // 초 카모
                SpawnNewBloons(1, 4);
                break;
            case 5: //초
                SpawnNewBloons(1, 3);
                break;
            case 4:// 파 카모
                SpawnNewBloons(1, 2);
                break;
            case 3: // 파 
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
                newBloon.transform.position = transform.position; // 기존 위치 유지
                //newBloonManager.hp = newLevel;
                newBloonManager.progress = this.progress; // 기존 진행 상태 유지
                newBloonManager.isFromFirstGroup = this.isFromFirstGroup; // 기존 그룹 유지
                newBloon.SetActive(true);
            }
        }
        bloonSpawner.ReturnToPool(gameObject, isFromFirstGroup);
    }

    // 풍선이 스플라인을 따라 이동
    private IEnumerator MoveAlongSpline()
    {
        while (progress < 1f)
        {
            transform.position = spline.EvaluatePosition(progress);
            progress += Time.deltaTime * speed * 0.1f; // 속도 조절
            yield return null;
        }
        bloonSpawner.ReturnToPool(gameObject, isFromFirstGroup);
    }
}
