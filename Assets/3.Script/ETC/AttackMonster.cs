using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMonster : MonoBehaviour
{
    private Animator animator;
    private Queue<GameObject> monstersInRange = new Queue<GameObject>(); // FIFO 구조 (먼저 들어온 몬스터 먼저 공격)
    private UserManager userManager;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        userManager = GetComponent<UserManager>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // 일정 간격으로 공격 실행
        InvokeRepeating(nameof(Attack), 0, userManager.attackspeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            if (!monstersInRange.Contains(collision.gameObject))
            {
                monstersInRange.Enqueue(collision.gameObject); // 먼저 들어온 몬스터를 먼저 공격하기 위해 Queue 사용
            }
            animator.SetBool("isinRange", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            RemoveMonsterFromQueue(collision.gameObject);

            // 큐가 비었으면 애니메이션 상태 변경
            if (monstersInRange.Count == 0)
            {
                animator.SetBool("isinRange", false);
            }
        }
    }

    private void Attack()
    {
        if (monstersInRange.Count > 0)
        {
            GameObject target = monstersInRange.Peek(); // 먼저 들어온 몬스터 가져오기 (삭제 X)
            if (target != null)
            {
                // 몬스터 체력 감소
                MonsterManager monsterManager = target.GetComponent<MonsterManager>();
                if (monsterManager != null)
                {
                    monsterManager.TakeDamage(userManager.attack);

                    // 몬스터 체력이 0 이하라면 제거
                    if (monsterManager.hp <= 0)
                    {
                        RemoveMonsterFromQueue(target);
                    }
                }

                // 플레이어가 몬스터를 바라보게 좌우 반전 적용
                if (spriteRenderer != null)
                {
                    float direction = target.transform.position.x - transform.position.x;
                    spriteRenderer.flipX = direction > 0; // 몬스터가 왼쪽이면 flipX = true, 오른쪽이면 false
                }
            }
            else
            {
                // null 오브젝트 삭제
                monstersInRange.Dequeue();
            }
        }
    }

    // 특정 몬스터를 큐에서 제거
    private void RemoveMonsterFromQueue(GameObject monster)
    {
        if (monstersInRange.Contains(monster))
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();

            while (monstersInRange.Count > 0)
            {
                GameObject m = monstersInRange.Dequeue();
                if (m != monster) newQueue.Enqueue(m);
            }

            monstersInRange = newQueue;
        }
    }
}
