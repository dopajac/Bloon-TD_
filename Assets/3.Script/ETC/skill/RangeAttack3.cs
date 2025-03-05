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
    private Dictionary<GameObject, Coroutine> attackCoroutines = new Dictionary<GameObject, Coroutine>(); // 각 몬스터별 데미지 코루틴 관리

    void Start()
    {
        Useranimater = transform.parent.GetComponent<Animator>();
        gameObject.SetActive(false); //  시작 시 비활성화
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
            if (!gameObject.activeSelf) gameObject.SetActive(true); //  타겟이 생기면 활성화
            SetSkillPositionAndRotation();
        }
    }

    void SetSkillPositionAndRotation()
    {
        if (target == null) return;

        // 캐릭터와 타겟 몬스터의 중간 위치에 스킬 배치
        Vector3 midPoint = (User.position + target.transform.position) / 2;
        transform.position = midPoint;

        // 스킬 방향을 타겟 몬스터 방향으로 설정
        Vector3 direction = (target.transform.position - User.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 왼쪽 몬스터 공격 시 스킬을 좌우 반전
        if (angle > 90f || angle < -90f)
        {
            transform.localScale = new Vector3(1, -1, 1); // 위아래 반전
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1); // 원래 상태 유지
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && !monstersInRange.Contains(collision.gameObject))
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true); //  몬스터 감지 시 활성화
            monstersInRange.Add(collision.gameObject);
            StartDamagingMonster(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && monstersInRange.Contains(collision.gameObject))
        {
            StopDamagingMonster(collision.gameObject);
            monstersInRange.Remove(collision.gameObject);

            //  마지막 몬스터가 나가면 스킬 비활성화
            if (monstersInRange.Count == 0)
            {
                HandleTargetDeath();
            }
        }
    }

    private void StartDamagingMonster(GameObject monster)
    {
        if (monster == null || attackCoroutines.ContainsKey(monster)) return;

        Coroutine attackRoutine = StartCoroutine(DamageMonsterOverTime(monster));
        attackCoroutines.Add(monster, attackRoutine);
    }

    private void StopDamagingMonster(GameObject monster)
    {
        if (attackCoroutines.ContainsKey(monster))
        {
            StopCoroutine(attackCoroutines[monster]);
            attackCoroutines.Remove(monster);
        }
    }

    private IEnumerator DamageMonsterOverTime(GameObject monster)
    {
        while (monster != null && !IsTargetDead(monster))
        {
            float attackSpeed = userManager != null ? userManager.attackspeed : 1f;
            int attackDamage = userManager != null ? userManager.attack : 10;

            MonsterManager monsterManager = monster.GetComponent<MonsterManager>();
            if (monsterManager != null)
            {
                monsterManager.TakeDamage(attackDamage);
                Debug.Log($"몬스터 {monster.name}에게 {attackDamage} 데미지!");

                // 피격 이펙트 생성
                if (hitPrefab != null)
                {
                    GameObject hitEffect = Instantiate(hitPrefab, monster.transform.position, Quaternion.identity);
                    Destroy(hitEffect, effectDuration);
                }
            }

            yield return new WaitForSeconds(attackSpeed);
        }

        StopDamagingMonster(monster);
    }

    private bool IsTargetDead(GameObject monster)
    {
        if (monster == null) return true;

        MonsterManager monsterManager = monster.GetComponent<MonsterManager>();
        return monsterManager == null || monsterManager.hp <= 0 || !monster.activeSelf;
    }

    private void HandleTargetDeath()
    {
        Debug.Log("RangeAttack3: 타겟이 없어져 스킬을 비활성화합니다.");
        Useranimater.SetBool("isinRange", false);
        gameObject.SetActive(false);
    }
}
