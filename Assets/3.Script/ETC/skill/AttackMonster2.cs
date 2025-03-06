using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMonster2 : MonoBehaviour
{
    public GameObject target; 
    private List<GameObject> monstersInRange = new List<GameObject>(); 
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
                target = collision.gameObject; 
                Debug.Log($"Å¸°Ù ¼³Á¤: {target.name}");
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
                Debug.Log($"Å¸°Ù º¯°æ: {target?.name ?? "¾øÀ½"}");
            }
        }
    }
}
