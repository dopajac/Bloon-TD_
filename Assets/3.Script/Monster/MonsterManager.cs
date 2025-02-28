using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines; // Spline 관련 네임스페이스 추가

public class MonsterManager : MonoBehaviour
{
    [SerializeField] public int level; // 몬스터 레벨
    [SerializeField] public int hp;
    [SerializeField] public int experience;
    private int initialHp; // 초기 체력 저장
    private MonsterSpawner monsterSpawner;
    private Vector3 lastPosition;
    private SpriteRenderer spriteRenderer;
    private SplineAnimate splineAnimate; // Spline 이동을 제어하는 컴포넌트
    private Vector3 startPosition; // 초기 위치 저장

    private const float directionThreshold = 0.05f; // 이동 감지 임계값

    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;

        initialHp = hp; // 초기 체력 저장
        isDead = false;

        // MonsterSpawner 자동 할당 (씬에서 찾아서 연결)
        if (monsterSpawner == null)
        {
            monsterSpawner = FindObjectOfType<MonsterSpawner>();
        }

        // SplineAnimate 찾기
        splineAnimate = GetComponent<SplineAnimate>();

        if (splineAnimate != null)
        {
            startPosition = splineAnimate.Container.EvaluatePosition(0f); // Spline의 시작점
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        hp -= damage;
        Debug.Log("공격");

        if (hp <= 0)
        {
            Die();
            GameManager.instance.Stageexperience += experience;
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        GameManager.instance.meso += level;

        if (splineAnimate != null)
        {
            splineAnimate.Pause();
        }

        animator.SetBool("isDead", true);

        // spawnedMonsterList에서도 제거
        RemoveFromMonsterList();

        // 애니메이션이 끝난 후 ReturnToPool 실행
        Invoke(nameof(ReturnToPool), 1.0f);
    }

    private void ReturnToPool()
    {
        if (!isDead) return;

        isDead = false;
        hp = initialHp; // 체력 초기화
        animator.SetBool("isDead", false);

        RemoveFromMonsterList();

        if (splineAnimate != null)
        {
            transform.position = startPosition; // 위치 복구
            splineAnimate.Restart(true);
        }

        gameObject.SetActive(false);

        if (monsterSpawner != null)
        {
            monsterSpawner.ReturnToPool(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (monsterSpawner == null || monsterSpawner.finishobj == null) return;

        // finishobj와 충돌하면
        if (collision.gameObject == monsterSpawner.finishobj)
        {
            // GameManager의 life 감소
            GameManager.instance.life--;
            Debug.Log($"몬스터가 finishobj에 도달! 남은 생명: {GameManager.instance.life}");

            // spawnedMonsterList에서도 제거
            RemoveFromMonsterList();

            // 즉시 비활성화하여 자동으로 다시 스폰되지 않도록 설정
            gameObject.SetActive(false);
        }
    }

    public void Respawn()
    {
        isDead = false;
        hp = initialHp; // 체력 초기화
        animator.SetBool("isDead", false);

        if (splineAnimate != null)
        {
            transform.position = startPosition; // 초기 위치 복구
            splineAnimate.Restart(true);
        }

        gameObject.SetActive(true);
    }

    private void RemoveFromMonsterList()
    {
        if (monsterSpawner != null)
        {
           
            monsterSpawner.alivemonster.Remove(gameObject);
            monsterSpawner.spawnedMonsterList.Remove(gameObject); // 추가: spawnedMonsterList에서도 제거
            if (monsterSpawner.spawnedMonsterList.Count == 0)
            {
                Debug.Log("stage 끝");
            }
        }

    }
}
