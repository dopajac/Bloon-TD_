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
    private Coroutine shootingCoroutine;  // ����� �ڷ�ƾ

    void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
        userManager = transform.parent.GetComponent<UserManager>();
        spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();

        if (userManager == null)
        {
            Debug.LogError("Range: UserManager�� ã�� �� �����ϴ�!");
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

            // ���� �ڷ�ƾ ����
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
            // ���Ͱ� ������ ���� ����
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

    //  ���� ����� ���� �ڷ�ƾ
    private IEnumerator FireBulletCoroutine()
    {
        while (targetMonster != null)
        {
            FireBullet(targetMonster);
            yield return new WaitForSeconds(userManager.attackspeed); // ���� �ӵ��� ���� �Ѿ� �߻� ���� ����
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
