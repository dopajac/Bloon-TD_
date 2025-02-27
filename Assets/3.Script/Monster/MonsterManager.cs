using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines; // Spline 관련 네임스페이스 추가

public class MonsterManager : MonoBehaviour
{
    [SerializeField] public int level; // 몬스터 레벨
    [SerializeField] public int hp;

    private MonsterSpawner monsterSpawner;
    private Vector3 lastPosition;
    private SpriteRenderer spriteRenderer;
    private SplineAnimate splineAnimate; // Spline 이동을 제어하는 컴포넌트
    private Vector3 startPosition; // 초기 위치 저장

    private const float directionThreshold = 0.05f; // 이동 감지 임계값

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
        hp = level;

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

    void Update()
    {
        float moveDirection = transform.position.x - lastPosition.x;

        // 일정 임계값 이상 이동했을 때만 방향 변경
        if (Mathf.Abs(moveDirection) > directionThreshold)
        {
            spriteRenderer.flipX = moveDirection > 0;
        }

        lastPosition = transform.position;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (splineAnimate != null)
        {
            transform.position = startPosition; // Spline 시작 위치로 초기화
            splineAnimate.Restart(true); // Spline 애니메이션을 처음부터 다시 시작
        }

        gameObject.SetActive(false);

        if (monsterSpawner != null)
        {
            monsterSpawner.ReturnToPool(gameObject); // 몬스터 풀로 반환
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // finishobj에 도달하면 풀로 반환
        if (collision.gameObject == monsterSpawner.finishobj)
        {
            ReturnToPool();
        }
    }

    public void Respawn()
    {
        
        if (splineAnimate != null)
        {
            splineAnimate.Restart(true);
            transform.position = startPosition; // Spline 시작점에서 다시 시작
            
        }
        gameObject.SetActive(true);
    }
}
