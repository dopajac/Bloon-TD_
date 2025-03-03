using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] public int level;
    [SerializeField] public int hp;
    [SerializeField] public int experience;
    private int initialHp;
    private MonsterSpawner monsterSpawner;
    private Vector3 startPosition;

    private Animator animator;
    private bool isDead = false;
    private SplineAnimate splineAnimate;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialHp = hp*4;
        isDead = false;

        if (monsterSpawner == null)
        {
            monsterSpawner = FindObjectOfType<MonsterSpawner>();
        }

        splineAnimate = GetComponent<SplineAnimate>();

        if (splineAnimate != null)
        {
            startPosition = splineAnimate.Container.EvaluatePosition(0f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        hp -= damage;

        if (hp <= 0)
        {
            Die();
            GameManager.instance.StageExperience += experience;
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

        // `alivemonster` 및 `spawnedMonsterList`에서 제거
        RemoveFromMonsterList();
        
        // 애니메이션 후 ReturnToPool 실행
        Invoke(nameof(ReturnToPool), 1.0f);

        if (monsterSpawner.cur_mostercount == 10 && monsterSpawner.spawnedMonsterList.Count == 0)
        {
            Debug.Log("stage is clear");
        }
    }

    private void ReturnToPool()
    {
        if (!isDead) return;

        isDead = false;
        hp = initialHp;
        animator.SetBool("isDead", false);

        RemoveFromMonsterList();

        if (splineAnimate != null)
        {
            transform.position = startPosition;
            splineAnimate.Restart(true);
        }

        gameObject.SetActive(false);

        if (monsterSpawner != null)
        {
           // monsterSpawner.ReturnToPool(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (monsterSpawner == null || monsterSpawner.finishobj == null) return;

        if (collision.gameObject == monsterSpawner.finishobj)
        {
            // 몬스터가 도착했음을 `alivemonster`에 추가
            if (!monsterSpawner.alivemonster.Contains(gameObject))
            {
                monsterSpawner.alivemonster.Add(gameObject);
            }

            // GameManager의 life 감소
            GameManager.instance.life--;
            Debug.Log($"몬스터가 finishobj에 도달! 남은 생명: {GameManager.instance.life}");

            // `spawnedMonsterList`에서 제거하지만, `alivemonster`에는 남겨둠
            monsterSpawner.spawnedMonsterList.Remove(gameObject);
            transform.position = new Vector3(-20, -20, 0);
            gameObject.SetActive(false);

            if (monsterSpawner.cur_mostercount == 10 && monsterSpawner.spawnedMonsterList.Count == 0)
            {
                Debug.Log("stage is clear");
            }

        }
    }

    public void Respawn()
    {
        isDead = false;
        hp = initialHp;
        animator.SetBool("isDead", false);

        if (splineAnimate != null)
        {
            transform.position = startPosition;
            splineAnimate.Restart(true);
        }

        gameObject.SetActive(true);
    }

    private void RemoveFromMonsterList()
    {
        if (monsterSpawner != null)
        {
            monsterSpawner.alivemonster.Remove(gameObject);
            monsterSpawner.spawnedMonsterList.Remove(gameObject);
        }
    }
}
