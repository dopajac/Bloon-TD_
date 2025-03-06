using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipController : MonoBehaviour
{
    public Transform user; // 유저(플레이어) 캐릭터의 Transform
    public GameObject target; // 현재 타겟 (몬스터 등)
    private SpriteRenderer spriteRenderer; // 스프라이트 렌더러

    public Range range;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 가져오기

        if (user == null)
        {
            Debug.LogError("FlipController: 유저(플레이어) 오브젝트가 설정되지 않았습니다!");
        }
    }

    void Update()
    {
        if (target == null) return; // 타겟이 없으면 실행하지 않음

        // 타겟이 유저의 오른쪽에 있으면 Flip
        if (target.transform.position.x > user.position.x)
        {
            spriteRenderer.flipX = true; // 오른쪽일 때 Flip 활성화
        }
        else
        {
            spriteRenderer.flipX = false; // 왼쪽일 때 원래 상태 유지
        }
    }
}
