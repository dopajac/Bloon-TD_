using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] public int level; // 풍선 레벨
    [SerializeField] public int hp;

    private MonsterSpawner monsterSpawner;

    private Vector3 lastPosition;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject finishobj;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
        hp = level;
    }

    void Update()
    {
        float moveDirection = transform.position.x - lastPosition.x;

        if (moveDirection > 0) // 오른쪽 이동
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDirection < 0) // 왼쪽 이동
        {
            spriteRenderer.flipX = false;
        }

        lastPosition = transform.position;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        { Die(); }
    }
    private void Die()
    {
        Debug.Log($"{gameObject.name} 사망");
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(-20, -20, 0);
        if (monsterSpawner != null)
        {
            monsterSpawner.ReturnToPool(gameObject); // 몬스터 풀로 반환
        }
        
    }
}
