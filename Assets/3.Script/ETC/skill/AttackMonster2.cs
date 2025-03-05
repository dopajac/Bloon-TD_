using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMonster2 : MonoBehaviour
{
    public GameObject target; // 가장 먼저 감지된 몬스터
    private List<GameObject> monstersInRange = new List<GameObject>(); // 감지된 몬스터 리스트
    public GameObject skillPrefab;
    public Animator Useranimator;
    private void Start()
    {
        Useranimator = transform.parent.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && !monstersInRange.Contains(collision.gameObject))
        {
            skillPrefab.SetActive(true);
            Useranimator.SetBool("isinRange", true);
            monstersInRange.Add(collision.gameObject);
            if (target == null)
            {
                target = collision.gameObject; // 가장 먼저 감지된 몬스터를 타겟으로 설정
                Debug.Log($"타겟 설정: {target.name}");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && monstersInRange.Contains(collision.gameObject))
        {
            monstersInRange.Remove(collision.gameObject);
            if (collision.gameObject == target)
            {
                target = monstersInRange.Count > 0 ? monstersInRange[0] : null;
                Debug.Log($"타겟 변경: {target?.name ?? "없음"}");
            }
        }
    }
}
