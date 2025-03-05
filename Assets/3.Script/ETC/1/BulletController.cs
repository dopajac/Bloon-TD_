using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject target; // 따라갈 적
    public float speed; // 총알 속도
    public int damage; // 데미지
    public Transform initialPosition; // 초기 위치 (Adventurer 위치)
    private bool isActive = false; // 총알 활성화 여부
    private UserManager usermanager;
    private Animator animator; // 애니메이션 컨트롤러

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Animator 가져오기
        usermanager = transform.parent.GetComponent<UserManager>();

        
    }

    private void Update()
    {
        if (!isActive || target == null)
        {
            ResetBullet(); // target이 사라지면 자동으로 Bullet 비활성화
            return;
        }

        // 적을 향해 이동
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        // 타겟과 가까워지면 충돌 판정
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
        transform.position = initialPosition.position; // 원래 위치로 되돌리기
        gameObject.SetActive(true); // Bullet 활성화
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

        StartCoroutine(PlayHitAnimationAndDisable()); // 애니메이션 실행 후 비활성화
    }

    private IEnumerator PlayHitAnimationAndDisable()
    {
        // 애니메이션 실행
        if (animator != null)
        {
            animator.SetBool("ishit",true); // "Hit" 트리거를 실행 (애니메이션 설정 필요)
        }

        // 애니메이션이 실행될 시간을 기다림 (0.5초)
        yield return new WaitForSeconds(0.5f);

        ResetBullet(); // 애니메이션 실행 후 Bullet 비활성화
    }

    private void ResetBullet()
    {
        animator.SetBool("ishit", false);
        isActive = false;
        gameObject.SetActive(false); // Bullet 비활성화
    }
}
