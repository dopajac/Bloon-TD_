using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMonster : MonoBehaviour
{
    private Animator animator;
    [SerializeField] public Queue<GameObject> monstersInRange = new Queue<GameObject>(); // FIFO 구조
    public UserManager userManager;
    private SpriteRenderer spriteRenderer;
    public GameObject target;
    public BulletController bulletController; // 미리 존재하는 Bullet 사용

    private void Awake()
    {
        animator = transform.parent.GetComponent<Animator>();
        spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        userManager = transform.parent.GetComponent<UserManager>();
        if (userManager == null)
        {
            Debug.LogError("UserManager를 부모에서 찾을 수 없습니다.");
        }

        // 몬스터 사망 이벤트 구독
        MonsterManager.OnMonsterDeath += HandleMonsterDeath;
    }

    private void OnDestroy()
    {
        // 이벤트 해제 (메모리 누수 방지)
        MonsterManager.OnMonsterDeath -= HandleMonsterDeath;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster") && collision.gameObject.activeSelf)
        {
            if (!monstersInRange.Contains(collision.gameObject))
            {
                monstersInRange.Enqueue(collision.gameObject);
            }

            animator.SetBool("isinRange", true);
            InvokeRepeating(nameof(Attack), 0, userManager.attackspeed); // 공격 실행
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            RemoveMonsterFromQueue(collision.gameObject);

            if (monstersInRange.Count == 0)
            {
                animator.SetBool("isinRange", false);
                CancelInvoke(nameof(Attack)); // 몬스터가 없으면 공격 중지
            }
        }
    }

    private void Attack()
    {
        if (target == null || IsTargetDead(target))
        {
            target = GetNextTarget();
        }

        if (target != null && bulletController != null)
        {
            bulletController.ActivateBullet(target);
        }
        else
        {
            CancelInvoke(nameof(Attack));
            animator.SetBool("isinRange", false);
            target = null;
        }
    }

    private bool IsTargetDead(GameObject monster)
    {
        if (monster == null) return true;

        MonsterManager monsterManager = monster.GetComponent<MonsterManager>();
        return monsterManager == null || monsterManager.hp <= 0 || !monster.activeSelf;
    }

    private GameObject GetNextTarget()
    {
        while (monstersInRange.Count > 0)
        {
            GameObject nextTarget = monstersInRange.Peek();
            if (!IsTargetDead(nextTarget))
            {
                return nextTarget;
            }
            monstersInRange.Dequeue(); //  죽은 몬스터는 완전히 제거
        }
        return null;
    }

    private void RemoveMonsterFromQueue(GameObject monster)
    {
        if (monster == null || !monstersInRange.Contains(monster)) return;

        Queue<GameObject> newQueue = new Queue<GameObject>();

        while (monstersInRange.Count > 0)
        {
            GameObject m = monstersInRange.Dequeue();
            if (m != monster) newQueue.Enqueue(m);
        }

        monstersInRange = newQueue;
    }

    // 몬스터가 사망했을 때 자동으로 제거
    private void HandleMonsterDeath(GameObject monster)
    {
        RemoveMonsterFromQueue(monster);

        if (target == monster)
        {
            target = GetNextTarget();
        }

        if (monstersInRange.Count == 0)
        {
            target = null;
            animator.SetBool("isinRange", false);
            CancelInvoke(nameof(Attack));
        }
    }
}
