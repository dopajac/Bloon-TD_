using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMonster : MonoBehaviour
{
    private Animator animator;
    [SerializeField] public Queue<GameObject> monstersInRange = new Queue<GameObject>(); // FIFO ���� (���� ���� ���� ���� ����)
    public UserManager userManager;
    private SpriteRenderer spriteRenderer;
    public GameObject target;
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
        // ���� �������� ���� ����
        
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
            InvokeRepeating(nameof(Attack), 1, userManager.attackspeed);
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
            Debug.Log("1");
            target = monstersInRange.Peek(); // ���� ���� ���� �������� (���� X)
            Debug.Log("2");
            if (target != null)
            {
                // ���� ü�� ����
                MonsterManager monsterManager = target.GetComponent<MonsterManager>();
                if (monsterManager != null)
                {
                    monsterManager.TakeDamage(userManager.attack);
                    Debug.Log("����");
                    // ���� ü���� 0 ���϶�� ����
                    if (monsterManager.hp <= 0)
                    {
                        RemoveMonsterFromQueue(target);
                    }
                }

                // �÷��̾ ���͸� �ٶ󺸰� �¿� ���� ����
                if (spriteRenderer != null)
                {
                    float direction = target.transform.position.x - transform.position.x;
                    spriteRenderer.flipX = direction > 0; // ���Ͱ� �����̸� flipX = true, �������̸� false
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
