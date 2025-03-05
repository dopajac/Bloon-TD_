using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    public Animator animatorhit;
    public Animator animatorattack;
    public Animator animatorattackmotion;
    public Animator animatorusereffect;
    public float attackRadius;
    public GameObject attackEffectPrefab;
    public GameObject hitEffectPrefab;
    public GameObject UserEffectPrefab;
    public int attackDamage;
    public float attackSpeed;
    public float effectDuration = 1f;
    public CircleCollider2D circlecollider;
    public List<GameObject> monstersInRange = new List<GameObject>();

    public UserManager userManager;

    void Start()
    {
        circlecollider = GetComponent<CircleCollider2D>();
        attackRadius = circlecollider.radius;

        if (transform.parent != null)
        {
            animatorattackmotion = transform.parent.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Suncold5Attack: �θ� ������Ʈ�� �����ϴ�!");
        }

        userManager = transform.parent.GetComponent<UserManager>();

        animatorhit = hitEffectPrefab.GetComponent<Animator>();
        animatorattack = attackEffectPrefab.GetComponent<Animator>();
        animatorusereffect = UserEffectPrefab.GetComponent<Animator>();

        attackRadius = circlecollider.bounds.extents.x;

        // ���� ��� �̺�Ʈ ����
        MonsterManager.OnMonsterDeath += HandleMonsterDeath;
    }

    private void OnDestroy()
    {
        MonsterManager.OnMonsterDeath -= HandleMonsterDeath;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster") && collision.gameObject.activeSelf)
        {
            if (!monstersInRange.Contains(collision.gameObject))
            {
                monstersInRange.Add(collision.gameObject);
            }

            Debug.Log("���� ������: " + collision.gameObject.name);

            UserEffectPrefab.SetActive(true);
            animatorusereffect.SetBool("isinRange", true);

            if (animatorattackmotion != null)
            {
                animatorattackmotion.SetBool("isinRange", true);
            }

            if (monstersInRange.Count == 1)
            {
                // ���Ͱ� ó�� ������ �� ���� ����
                attackSpeed = userManager != null ? userManager.attackspeed : 1f;
                InvokeRepeating(nameof(AttackAllMonsters), 0, attackSpeed);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            RemoveMonsterFromList(collision.gameObject);

            if (monstersInRange.Count == 0)
            {
                animatorusereffect.SetBool("isinRange", false);
                animatorattackmotion.SetBool("isinRange", false);

                CancelInvoke(nameof(AttackAllMonsters)); // ��� ���Ͱ� ������ ���� ����
            }
        }
    }

    private void AttackAllMonsters()
    {
        if (monstersInRange.Count == 0)
        {
            CancelInvoke(nameof(AttackAllMonsters));
            return;
        }

        attackSpeed = userManager != null ? userManager.attackspeed : 1f;
        attackDamage = userManager != null ? userManager.attack : 10;

        foreach (GameObject monster in monstersInRange)
        {
            if (monster != null && !IsTargetDead(monster))
            {
                MonsterManager monsterManager = monster.GetComponent<MonsterManager>();
                if (monsterManager != null)
                {
                    monsterManager.TakeDamage(attackDamage);
                }

                // ���� ����Ʈ ����   
                if (attackEffectPrefab != null)
                {
                    GameObject attackEffect = Instantiate(attackEffectPrefab, monster.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    Destroy(attackEffect, effectDuration);
                }

                // �ǰ� ����Ʈ ����
                if (hitEffectPrefab != null)
                {
                    GameObject hitEffect = Instantiate(hitEffectPrefab, monster.transform.position, Quaternion.identity);
                    Destroy(hitEffect, effectDuration);
                }

                Debug.Log($"���� {monster.name}���� {attackDamage} ������!");
            }
        }
    }

    private bool IsTargetDead(GameObject monster)
    {
        if (monster == null) return true;

        MonsterManager monsterManager = monster.GetComponent<MonsterManager>();
        return monsterManager == null || monsterManager.hp <= 0 || !monster.activeSelf;
    }

    private void RemoveMonsterFromList(GameObject monster)
    {
        if (monster == null || !monstersInRange.Contains(monster)) return;
        monstersInRange.Remove(monster);
    }

    private void HandleMonsterDeath(GameObject monster)
    {
        RemoveMonsterFromList(monster);

        if (monstersInRange.Count == 0)
        {
            animatorusereffect.SetBool("isinRange", false);
            animatorattackmotion.SetBool("isinRange", false);

            CancelInvoke(nameof(AttackAllMonsters)); // ��� ���Ͱ� ������ ���� ����
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
