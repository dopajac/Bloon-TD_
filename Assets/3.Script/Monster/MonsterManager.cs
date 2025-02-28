using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines; // Spline ���� ���ӽ����̽� �߰�

public class MonsterManager : MonoBehaviour
{
    [SerializeField] public int level; // ���� ����
    [SerializeField] public int hp;
    [SerializeField] public int experience;
    private int initialHp; // �ʱ� ü�� ����
    private MonsterSpawner monsterSpawner;
    private Vector3 lastPosition;
    private SpriteRenderer spriteRenderer;
    private SplineAnimate splineAnimate; // Spline �̵��� �����ϴ� ������Ʈ
    private Vector3 startPosition; // �ʱ� ��ġ ����

    private const float directionThreshold = 0.05f; // �̵� ���� �Ӱ谪

    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;

        initialHp = hp; // �ʱ� ü�� ����
        isDead = false;

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

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        hp -= damage;
        Debug.Log("����");

        if (hp <= 0)
        {
            Die();
            GameManager.instance.Stageexperience += experience;
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        GameManager.instance.meso += level;

        if (splineAnimate != null)
        {
            splineAnimate.Pause();
        }

        animator.SetBool("isDead", true);

        // spawnedMonsterList������ ����
        RemoveFromMonsterList();

        // �ִϸ��̼��� ���� �� ReturnToPool ����
        Invoke(nameof(ReturnToPool), 1.0f);
    }

    private void ReturnToPool()
    {
        if (!isDead) return;

        isDead = false;
        hp = initialHp; // ü�� �ʱ�ȭ
        animator.SetBool("isDead", false);

        RemoveFromMonsterList();

        if (splineAnimate != null)
        {
            transform.position = startPosition; // ��ġ ����
            splineAnimate.Restart(true);
        }

        gameObject.SetActive(false);

        if (monsterSpawner != null)
        {
            monsterSpawner.ReturnToPool(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (monsterSpawner == null || monsterSpawner.finishobj == null) return;

        // finishobj�� �浹�ϸ�
        if (collision.gameObject == monsterSpawner.finishobj)
        {
            // GameManager�� life ����
            GameManager.instance.life--;
            Debug.Log($"���Ͱ� finishobj�� ����! ���� ����: {GameManager.instance.life}");

            // spawnedMonsterList������ ����
            RemoveFromMonsterList();

            // ��� ��Ȱ��ȭ�Ͽ� �ڵ����� �ٽ� �������� �ʵ��� ����
            gameObject.SetActive(false);
        }
    }

    public void Respawn()
    {
        isDead = false;
        hp = initialHp; // ü�� �ʱ�ȭ
        animator.SetBool("isDead", false);

        if (splineAnimate != null)
        {
            transform.position = startPosition; // �ʱ� ��ġ ����
            splineAnimate.Restart(true);
        }

        gameObject.SetActive(true);
    }

    private void RemoveFromMonsterList()
    {
        if (monsterSpawner != null)
        {
           
            monsterSpawner.alivemonster.Remove(gameObject);
            monsterSpawner.spawnedMonsterList.Remove(gameObject); // �߰�: spawnedMonsterList������ ����
            if (monsterSpawner.spawnedMonsterList.Count == 0)
            {
                Debug.Log("stage ��");
            }
        }

    }
}
