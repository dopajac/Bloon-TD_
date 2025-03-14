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

    public delegate void MonsterDeathEvent(GameObject monster);
    public static event MonsterDeathEvent OnMonsterDeath;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialHp = hp;
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
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        GameManager.instance.meso += level;
        GameManager.instance.StageExperience += experience;
        if (splineAnimate != null)
        {
            splineAnimate.Pause();
        }

        animator.SetBool("isDead", true);


        RemoveFromMonsterList();

        Invoke(nameof(ReturnToPool), 1.0f);

        Invoke(nameof(CheckStageClear), 0.1f);

    }

    private void ReturnToPool()
    {
        if (!isDead) return;

        hp = initialHp;
        animator.SetBool("isDead", false);

        RemoveFromMonsterList();

        if (splineAnimate != null)
        {
            transform.position = startPosition;
            splineAnimate.Restart(true);
        }

        gameObject.SetActive(false);
        isDead = false; // 여기서 상태를 변경
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (monsterSpawner == null || monsterSpawner.finishobj == null) return;

        if (collision.gameObject == monsterSpawner.finishobj)        {

            if (!monsterSpawner.alivemonster.Contains(gameObject))
            {
                monsterSpawner.alivemonster.Add(gameObject);
            }

            GameManager.instance.life--;
            Debug.Log($"몬스터가 finishobj에 도달! 남은 생명: {GameManager.instance.life}");
            if (GameManager.instance.life == 0)
            {
                CanvasObject.instance.GameOver_Panel.SetActive(true);
            }


            transform.position = new Vector3(-20, -20, 0);
            gameObject.SetActive(false);

            Invoke(nameof(CheckStageClear), 0.1f);

            monsterSpawner.spawnedMonsterList.Remove(gameObject);
        }
    }
    private void CheckStageClear()
    {
        if (monsterSpawner.cur_mostercount == 10 && monsterSpawner.spawnedMonsterList.Count == 0)
        {
            GameManager.instance.isStagefinish = true;
            Debug.Log("stage is clear");

            
            GameManager.instance.AddExperienceToUsers();
            GameManager.instance.StageExperience = 0;

            if (monsterSpawner.currentLevel == 4)
            {
                CanvasObject.instance.GameClear_Panel.SetActive(false);
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
