using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    public Animator animator; 
    public GameObject skillPrefab; 
    public GameObject bulletPrefab; 
    public GameObject hitPrefab; 
    public Transform bulletSpawnPoint; 
    public float bulletSpeed = 10f; 
    public UserManager userManager;

    private SpriteRenderer spriteRenderer; 
    private Queue<GameObject> monsterQueue = new Queue<GameObject>(); 
    public GameObject targetMonster;

    void Start()
    {
        animator = transform.parent.GetComponent<Animator>(); 
        userManager = transform.parent.GetComponent<UserManager>();
        spriteRenderer = transform.parent.GetComponent<SpriteRenderer>(); 
        if (userManager == null)
        {
            Debug.LogError("Range: UserManager를 찾을 수 없습니다!");
        }

        skillPrefab.SetActive(false); 
    }

    void Update()
    {
        if (targetMonster == null) return;

        
        if (targetMonster.transform.position.x > transform.parent.position.x)
        {
            spriteRenderer.flipX = true; 
        }
        else
        {
            spriteRenderer.flipX = false; 
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && !monsterQueue.Contains(collision.gameObject))
        {
            monsterQueue.Enqueue(collision.gameObject); 
            UpdateTarget(); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && monsterQueue.Contains(collision.gameObject))
        {
            monsterQueue.Dequeue(); 
            UpdateTarget(); 
        }
    }

    private void UpdateTarget()
    {
        if (monsterQueue.Count > 0)
        {
            targetMonster = monsterQueue.Peek(); 
            animator.SetBool("isinRange", true);
            skillPrefab.SetActive(true);
            SetSkillDirection(targetMonster.transform);

            if (bulletPrefab != null)
            {
                FireBullet(targetMonster);
            }
            else
            {
                StartCoroutine(DamageTargetOverTime(targetMonster)); 
            }
        }
        else
        {
            targetMonster = null;
            animator.SetBool("isinRange", false);
            skillPrefab.SetActive(false);
        }
    }

    void SetSkillDirection(Transform monster)
    {
        if (monster == null) return;

        Vector3 direction = (monster.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        skillPrefab.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FireBullet(GameObject monster)
    {
        if (bulletPrefab == null || bulletSpawnPoint == null) return;

        bulletPrefab.SetActive(true);
        bulletPrefab.GetComponent<Bullet>().Initialize(monster, userManager.attack, bulletSpeed, userManager.attackspeed);
    }

    private IEnumerator DamageTargetOverTime(GameObject target)
    {
        while (target == targetMonster && target != null) 
        {
            MonsterManager monsterManager = target.GetComponent<MonsterManager>();
            if (monsterManager != null)
            {
                monsterManager.TakeDamage(userManager.attack);
                Debug.Log($"몬스터 {target.name}에게 {userManager.attack} 데미지!");

                //  피격 이펙트 생성
                if (hitPrefab != null)
                {
                    GameObject hitEffect = Instantiate(hitPrefab, target.transform.position, Quaternion.identity);
                    Destroy(hitEffect, 0.1f); 
                }
            }

            yield return new WaitForSeconds(userManager.attackspeed); 
        }
    }
}
