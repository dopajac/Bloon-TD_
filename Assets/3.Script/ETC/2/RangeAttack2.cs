using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack2 : MonoBehaviour
{
    public Animator animatorhit;
    public Animator animatorattackmotion;
    public Animator animatorusereffect;

    public float attackRadius;

    public GameObject hitEffectPrefab;
    public GameObject UserEffectPrefab;
    public int attackDamage;
    public float attackSpeed;
    public float effectDuration = 1f;
    public CircleCollider2D circlecollider;
    public List<GameObject> monstersInRange = new List<GameObject>();
    public GameObject targetMonster; 

    public UserManager userManager;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        circlecollider = GetComponent<CircleCollider2D>();
        attackRadius = circlecollider.radius;

        if (transform.parent != null)
        {
            animatorattackmotion = transform.parent.GetComponent<Animator>();
            spriteRenderer = transform.parent.GetComponent<SpriteRenderer>(); 
        }

        animatorusereffect = animatorusereffect.transform.GetComponent<Animator>();
        animatorhit = hitEffectPrefab.transform.GetComponent<Animator>();
        
        userManager = transform.parent.GetComponent<UserManager>();

        attackRadius = circlecollider.bounds.extents.x;

        // 몬스터 사망 이벤트 구독
        MonsterManager.OnMonsterDeath += HandleMonsterDeath;
    }

    void Update()
    {
        if (targetMonster == null) return;

        // 타겟이 유저의 오른쪽에 있으면 Flip
        if (targetMonster.transform.position.x > transform.parent.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
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
                UpdateTarget(); 
            }

            Debug.Log("몬스터 감지됨: " + collision.gameObject.name);

            UserEffectPrefab.SetActive(true);

            if (animatorattackmotion != null)
            {
                animatorattackmotion.SetBool("isinRange", true);
            }

            if (monstersInRange.Count == 1)
            {
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
            UpdateTarget(); // 타겟 재설정

            if (monstersInRange.Count == 0)
            {
                animatorattackmotion.SetBool("isinRange", false);
                UserEffectPrefab.SetActive(false);
                CancelInvoke(nameof(AttackAllMonsters));
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

                // 피격 이펙트 생성
                if (hitEffectPrefab != null)
                {
                    GameObject hitEffect = Instantiate(hitEffectPrefab, monster.transform.position, Quaternion.identity);
                    Destroy(hitEffect, effectDuration);
                }

                Debug.Log($"몬스터 {monster.name}에게 {attackDamage} 데미지!");
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
        UpdateTarget();

        if (monstersInRange.Count == 0)
        {
            animatorusereffect.SetBool("isinRange", false);
            animatorattackmotion.SetBool("isinRange", false);
            CancelInvoke(nameof(AttackAllMonsters));
        }
    }

    private void UpdateTarget()
    {
        if (monstersInRange.Count > 0)
        {
            targetMonster = monstersInRange[0]; 
        }
        else
        {
            targetMonster = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
