using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack3 : MonoBehaviour
{
    public float effectDuration = 1f; // �ǰ� ����Ʈ ���� �ð�
    public int damage = 20; // �⺻ ���ݷ�
    public GameObject hitPrefab; // �ǰ� ����Ʈ ������
    public Animator Useranimater;
    private Transform User; // ĳ���� ��ġ
    public GameObject target; // Circle���� ������ ��ǥ ����
    public AttackMonster2 attackmonster2;
    private UserManager userManager;
    private List<GameObject> monstersInRange = new List<GameObject>(); // ���� �浹 ���� ���� ����Ʈ

    void Start()
    {
        Useranimater = transform.parent.GetComponent<Animator>();
        gameObject.SetActive(false); // ���� �� ��Ȱ��ȭ
        User = transform.parent; // �θ� ������Ʈ(ĳ����) ����
        attackmonster2 = GameObject.Find("Circle").GetComponent<AttackMonster2>();

        if (attackmonster2 == null)
        {
            Debug.LogError("RangeAttack3: Circle ������Ʈ�� ã�� �� �����ϴ�!");
            return;
        }

        userManager = User.GetComponent<UserManager>(); // UserManager ��������
        if (userManager == null)
        {
            Debug.LogError("RangeAttack3: UserManager�� ã�� �� �����ϴ�!");
            return;
        }

        StartCoroutine(DamageAllMonstersOverTime());
    }

    private void Update()
    {
        target = attackmonster2.target;

        if (target == null)
        {
            HandleTargetDeath();
        }
        else
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true); // Ÿ���� ����� Ȱ��ȭ
            SetSkillPositionAndRotation();
        }
    }

    void SetSkillPositionAndRotation()
    {
        if (target == null) return;

        Vector3 midPoint = (User.position + target.transform.position) / 2;
        transform.position = midPoint;

        Vector3 direction = (target.transform.position - User.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (angle > 90f || angle < -90f)
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && !monstersInRange.Contains(collision.gameObject))
        {
            monstersInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && monstersInRange.Contains(collision.gameObject))
        {
            monstersInRange.Remove(collision.gameObject);

            if (monstersInRange.Count == 0)
            {
                HandleTargetDeath();
            }
        }
    }

    private IEnumerator DamageAllMonstersOverTime()
    {
        while (true)
        {
            if (monstersInRange.Count > 0)
            {
                float attackSpeed = userManager != null ? userManager.attackspeed : 1f;
                int attackDamage = userManager != null ? userManager.attack : 10;

                foreach (GameObject monster in new List<GameObject>(monstersInRange))
                {
                    if (monster != null)
                    {
                        MonsterManager monsterManager = monster.GetComponent<MonsterManager>();
                        if (monsterManager != null)
                        {
                            monsterManager.TakeDamage(attackDamage);
                            Debug.Log($"���� {monster.name}���� {attackDamage} ������!");

                            if (hitPrefab != null)
                            {
                                GameObject hitEffect = Instantiate(hitPrefab, monster.transform.position, Quaternion.identity);
                                Destroy(hitEffect, effectDuration);
                            }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(1f / (userManager != null ? userManager.attackspeed : 1f));
        }
    }

    private void HandleTargetDeath()
    {
        Debug.Log("RangeAttack3: Ÿ���� ������ ��ų�� ��Ȱ��ȭ�մϴ�.");
        Useranimater.SetBool("isinRange", false);
        gameObject.SetActive(false);
    }
}