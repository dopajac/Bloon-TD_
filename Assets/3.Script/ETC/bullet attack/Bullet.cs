using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitPrefab;
    public GameObject targetMonster;
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
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);

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
                Debug.Log($"���� {targetMonster.name}���� {damage} ������!");
            }

            // ���� ��ġ�� �ǰ� ����Ʈ ����
            if (hitPrefab != null)
            {
                GameObject hitEffect = Instantiate(hitPrefab, targetMonster.transform.position, Quaternion.identity);
                Destroy(hitEffect, attackSpeed); 
            }
        }
        
        gameObject.transform.position = transform.parent.position;
    }
}
