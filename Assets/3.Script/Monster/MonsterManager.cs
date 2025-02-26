using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] public int level; // ǳ�� ����
    [SerializeField] public int hp;

    private MonsterSpawner monsterSpawner;
    private Vector3 lastPosition;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject finishobj;

    private const float directionThreshold = 0.05f; // �̵� ���� �Ӱ谪

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
        hp = level;
    }

    void Update()
    {
        float moveDirection = transform.position.x - lastPosition.x;

        // ���� �Ӱ谪 �̻� �̵����� ���� ���� ����
        if (Mathf.Abs(moveDirection) > directionThreshold)
        {
            if (moveDirection > 0) // ������ �̵�
            {
                spriteRenderer.flipX = true;
            }
            else if (moveDirection < 0) // ���� �̵�
            {
                spriteRenderer.flipX = false;
            }
        }

        lastPosition = transform.position;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} ���");
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(-20, -20, 0);
        if (monsterSpawner != null)
        {
            monsterSpawner.ReturnToPool(gameObject); // ���� Ǯ�� ��ȯ
        }
    }
}
