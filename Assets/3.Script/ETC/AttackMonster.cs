using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMonster : MonoBehaviour
{
    private Animator animator;
    [SerializeField] public Queue<GameObject> monstersInRange = new Queue<GameObject>(); // FIFO ����
    public UserManager userManager;
    private SpriteRenderer spriteRenderer;
    public GameObject target;
    public BulletController bulletController; // �̸� �����ϴ� Bullet ���

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
            Debug.LogError("UserManager�� �θ𿡼� ã�� �� �����ϴ�.");
        }

        // ���� ��� �̺�Ʈ ����
        MonsterManager.OnMonsterDeath += HandleMonsterDeath;
    }

    private void OnDestroy()
    {
        // �̺�Ʈ ���� (�޸� ���� ����)
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
            InvokeRepeating(nameof(Attack), 0, userManager.attackspeed); // ���� ����
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
                CancelInvoke(nameof(Attack)); // ���Ͱ� ������ ���� ����
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
            monstersInRange.Dequeue(); //  ���� ���ʹ� ������ ����
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

    // ���Ͱ� ������� �� �ڵ����� ����
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
