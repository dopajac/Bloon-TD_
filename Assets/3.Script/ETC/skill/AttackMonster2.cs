using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMonster2 : MonoBehaviour
{
    public GameObject target; // ���� ���� ������ ����
    private List<GameObject> monstersInRange = new List<GameObject>(); // ������ ���� ����Ʈ
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
                target = collision.gameObject; // ���� ���� ������ ���͸� Ÿ������ ����
                Debug.Log($"Ÿ�� ����: {target.name}");
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
                Debug.Log($"Ÿ�� ����: {target?.name ?? "����"}");
            }
        }
    }
}
