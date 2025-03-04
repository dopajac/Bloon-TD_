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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
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
        // ���� Ÿ���� ���ų� ü���� 0 �����̸� ���ο� Ÿ�� ã��
        if (target == null || IsTargetDead(target))
        {
            RemoveMonsterFromQueue(target);
            target = GetNextTarget();
        }

        if (target != null && bulletController != null)
        {
            bulletController.ActivateBullet(target); // ���� Bullet�� Ȱ��ȭ�Ͽ� ���
        }        else
        {
            // Ÿ���� ������ ���� ����
            CancelInvoke(nameof(Attack));
            animator.SetBool("isinRange", false);
        }
    }

    private bool IsTargetDead(GameObject monster)
    {
        if (monster == null) return true;

        MonsterManager monsterManager = monster.GetComponent<MonsterManager>();
        return monsterManager == null || monsterManager.hp <= 0;
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
            monstersInRange.Dequeue(); // ���� ���� ����
        }
        return null; // ���ο� Ÿ���� ����
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
}
