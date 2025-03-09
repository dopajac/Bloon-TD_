using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2 : MonoBehaviour
{
    public GameObject hitPrefab;
    private GameObject targetMonster;
    private int damage;
    private float speed;
    private float attackSpeed;

    public void Initialize(GameObject target, int attackDamage, float bulletSpeed, float userAttackSpeed)
    {
        targetMonster = target;
        damage = attackDamage;
        speed = bulletSpeed;
        attackSpeed = userAttackSpeed;
    }

    void Update()
    {
        if (targetMonster == null)
        {
            Destroy(gameObject); // 목표가 사라지면 총알 삭제
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetMonster.transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetMonster.transform.position) < 0.1f)
        {
            OnHit();
        }
    }

    void OnHit()
    {
        if (targetMonster != null)
        {
            MonsterManager monsterManager = targetMonster.GetComponent<MonsterManager>();
            if (monsterManager != null)
            {
                monsterManager.TakeDamage(damage);
                Debug.Log($"몬스터 {targetMonster.name}에게 {damage} 데미지!");
            }

            // 피격 이펙트 생성
            if (hitPrefab != null)
            {
                GameObject hitEffect = Instantiate(hitPrefab, targetMonster.transform.position, Quaternion.identity);
                Destroy(hitEffect, 0.5f); // 이펙트를 0.5초 후 삭제
            }
        }

        Destroy(gameObject); // 총알 제거
    }
}
