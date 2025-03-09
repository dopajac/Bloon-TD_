using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range2 : MonoBehaviour
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
    private Coroutine shootingCoroutine;  // 연사용 코루틴

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

        spriteRenderer.flipX = targetMonster.transform.position.x > transform.parent.position.x;
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
            List<GameObject> tempList = new List<GameObject>(monsterQueue);
            tempList.Remove(collision.gameObject);
            monsterQueue = new Queue<GameObject>(tempList);

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
            bulletPrefab.SetActive(true);
            SetSkillDirection(targetMonster.transform);

            // 연사 코루틴 시작
            if (shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(FireBulletCoroutine());
            }
        }
        else
        {
            targetMonster = null;
            animator.SetBool("isinRange", false);
            skillPrefab.SetActive(false);
            bulletPrefab.SetActive(false);
            // 몬스터가 없으면 연사 중지
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = null;
            }
        }
    }

    void SetSkillDirection(Transform monster)
    {
        if (monster == null) return;

        spriteRenderer.flipX = monster.position.x > transform.position.x;
    }

    //  연사 기능을 위한 코루틴
    private IEnumerator FireBulletCoroutine()
    {
        while (targetMonster != null)
        {
            FireBullet(targetMonster);
            yield return new WaitForSeconds(userManager.attackspeed); // 공격 속도에 따라 총알 발사 간격 설정
        }
        shootingCoroutine = null;
    }

    void FireBullet(GameObject monster)
    {
        if (bulletPrefab == null || bulletSpawnPoint == null) return;

        GameObject bulletInstance = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Initialize(monster, userManager.attack, bulletSpeed, userManager.attackspeed);
    }
}
