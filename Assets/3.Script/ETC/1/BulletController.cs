using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject target; // ���� ��
    public float speed; // �Ѿ� �ӵ�
    public int damage; // ������
    public Transform initialPosition; // �ʱ� ��ġ (Adventurer ��ġ)
    private bool isActive = false; // �Ѿ� Ȱ��ȭ ����
    private UserManager usermanager;
    private Animator animator; // �ִϸ��̼� ��Ʈ�ѷ�

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Animator ��������
        usermanager = transform.parent.GetComponent<UserManager>();

        
    }

    private void Update()
    {
        if (!isActive || target == null)
        {
            ResetBullet(); // target�� ������� �ڵ����� Bullet ��Ȱ��ȭ
            return;
        }

        // ���� ���� �̵�
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        // Ÿ�ٰ� ��������� �浹 ����
        if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
        {
            HitTarget();
        }
        damage = usermanager.attack;
        speed = usermanager.attackspeed;
    }

    public void ActivateBullet(GameObject newTarget)
    {
        target = newTarget;
        isActive = true;
        transform.position = initialPosition.position; // ���� ��ġ�� �ǵ�����
        gameObject.SetActive(true); // Bullet Ȱ��ȭ
    }

    private void HitTarget()
    {
        if (target != null)
        {
            MonsterManager monsterManager = target.GetComponent<MonsterManager>();
            if (monsterManager != null)
            {
                
                monsterManager.TakeDamage(damage);
            }
        }

        StartCoroutine(PlayHitAnimationAndDisable()); // �ִϸ��̼� ���� �� ��Ȱ��ȭ
    }

    private IEnumerator PlayHitAnimationAndDisable()
    {
        // �ִϸ��̼� ����
        if (animator != null)
        {
            animator.SetBool("ishit",true); // "Hit" Ʈ���Ÿ� ���� (�ִϸ��̼� ���� �ʿ�)
        }

        // �ִϸ��̼��� ����� �ð��� ��ٸ� (0.5��)
        yield return new WaitForSeconds(0.5f);

        ResetBullet(); // �ִϸ��̼� ���� �� Bullet ��Ȱ��ȭ
    }

    private void ResetBullet()
    {
        animator.SetBool("ishit", false);
        isActive = false;
        gameObject.SetActive(false); // Bullet ��Ȱ��ȭ
    }
}
