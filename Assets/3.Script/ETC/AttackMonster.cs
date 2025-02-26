using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMonster : MonoBehaviour
{
    private Animator animator;
    private Queue<GameObject> monstersInRange = new Queue<GameObject>(); // FIFO ���� (���� ���� ���� ���� ����)

    private UserManager userManager;
    private void Awake()
    {
        userManager = GetComponent<UserManager>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    { 
        // ���� �������� ���� ����
        InvokeRepeating(nameof(Attack), 0, userManager.attackspeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            if (!monstersInRange.Contains(collision.gameObject))
            {
                monstersInRange.Enqueue(collision.gameObject); // ���� ���� ���͸� ���� �����ϱ� ���� Queue ���
            }
            animator.SetBool("isinRange", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            RemoveMonsterFromQueue(collision.gameObject);

            // ť�� ������� �ִϸ��̼� ���� ����
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
            GameObject target = monstersInRange.Peek(); // ���� ���� ���� �������� (���� X)
            if (target != null)
            {
                // ���� ü�� ����
                MonsterManager monsterManager = target.GetComponent<MonsterManager>();
                if (monsterManager != null)
                {
                    monsterManager.TakeDamage(userManager.attack);

                    // ���� ü���� 0 ���϶�� ����
                    if (monsterManager.hp <= 0)
                    {
                        RemoveMonsterFromQueue(target);
                    }
                }
            }
            else
            {
                // null ������Ʈ ����
                monstersInRange.Dequeue();
            }
        }
    }

    // Ư�� ���͸� ť���� ����
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
