using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack3 : MonoBehaviour
{
    public float effectDuration = 1f; // 피격 이펙트 유지 시간
    public int damage = 20; // 기본 공격력
    public GameObject hitPrefab; // 피격 이펙트 프리팹
    public Animator Useranimater;
    private Transform User; // 캐릭터 위치
    public GameObject target; // Circle에서 설정한 목표 몬스터
    public AttackMonster2 attackmonster2;
    private UserManager userManager;
    private List<GameObject> monstersInRange = new List<GameObject>(); // 현재 충돌 중인 몬스터 리스트

    void Start()
    {
        Useranimater = transform.parent.GetComponent<Animator>();
        gameObject.SetActive(false); // 시작 시 비활성화
        User = transform.parent; // 부모 오브젝트(캐릭터) 설정
        attackmonster2 = GameObject.Find("Circle").GetComponent<AttackMonster2>();

        if (attackmonster2 == null)
        {
            Debug.LogError("RangeAttack3: Circle 오브젝트를 찾을 수 없습니다!");
            return;
        }

        userManager = User.GetComponent<UserManager>(); // UserManager 가져오기
        if (userManager == null)
        {
            Debug.LogError("RangeAttack3: UserManager를 찾을 수 없습니다!");
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
            if (!gameObject.activeSelf) gameObject.SetActive(true); // 타겟이 생기면 활성화
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
                            Debug.Log($"몬스터 {monster.name}에게 {attackDamage} 데미지!");

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
        Debug.Log("RangeAttack3: 타겟이 없어져 스킬을 비활성화합니다.");
        Useranimater.SetBool("isinRange", false);
        gameObject.SetActive(false);
    }
}