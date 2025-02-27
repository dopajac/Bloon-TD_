using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines; // Spline ���� ���ӽ����̽� �߰�

public class MonsterManager : MonoBehaviour
{
    [SerializeField] public int level; // ���� ����
    [SerializeField] public int hp;

    private MonsterSpawner monsterSpawner;
    private Vector3 lastPosition;
    private SpriteRenderer spriteRenderer;
    private SplineAnimate splineAnimate; // Spline �̵��� �����ϴ� ������Ʈ
    private Vector3 startPosition; // �ʱ� ��ġ ����

    private const float directionThreshold = 0.05f; // �̵� ���� �Ӱ谪

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
        hp = level;

        // MonsterSpawner �ڵ� �Ҵ� (������ ã�Ƽ� ����)
        if (monsterSpawner == null)
        {
            monsterSpawner = FindObjectOfType<MonsterSpawner>();
        }

        // SplineAnimate ã��
        splineAnimate = GetComponent<SplineAnimate>();

        if (splineAnimate != null)
        {
            startPosition = splineAnimate.Container.EvaluatePosition(0f); // Spline�� ������
        }
    }

    void Update()
    {
        float moveDirection = transform.position.x - lastPosition.x;

        // ���� �Ӱ谪 �̻� �̵����� ���� ���� ����
        if (Mathf.Abs(moveDirection) > directionThreshold)
        {
            spriteRenderer.flipX = moveDirection > 0;
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
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (splineAnimate != null)
        {
            transform.position = startPosition; // Spline ���� ��ġ�� �ʱ�ȭ
            splineAnimate.Restart(true); // Spline �ִϸ��̼��� ó������ �ٽ� ����
        }

        gameObject.SetActive(false);

        if (monsterSpawner != null)
        {
            monsterSpawner.ReturnToPool(gameObject); // ���� Ǯ�� ��ȯ
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // finishobj�� �����ϸ� Ǯ�� ��ȯ
        if (collision.gameObject == monsterSpawner.finishobj)
        {
            ReturnToPool();
        }
    }

    public void Respawn()
    {
        
        if (splineAnimate != null)
        {
            splineAnimate.Restart(true);
            transform.position = startPosition; // Spline ���������� �ٽ� ����
            
        }
        gameObject.SetActive(true);
    }
}
