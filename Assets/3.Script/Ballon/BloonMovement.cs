using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Splines;


public class BloonMovement : MonoBehaviour
{

    [SerializeField] public GameObject Finish;

    private void Start()
    {
        Collider2D myCollider = GetComponent<Collider2D>();
        

        Finish = GameObject.Find("Finish");

        if (myCollider == null) return;

        GameObject[] bloons = GameObject.FindGameObjectsWithTag("Bloon");

        foreach (GameObject bloon in bloons)
        {
            if (bloon != gameObject) // 자기 자신 제외
            {
                Collider2D otherCollider = bloon.GetComponent<Collider2D>();
                if (otherCollider != null)
                {
                    Physics2D.IgnoreCollision(myCollider, otherCollider);
                }
            }
        }
    }

    //private void Update()
    //{
    //   
    //    if (gameObject.transform.position.y <= -8.5f)
    //    {
    //        
    //        gameObject.SetActive(false); // 이동이 끝나면 오브젝트 삭제
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Finish)
        {
            gameObject.SetActive(false); // 이동이 끝나면 오브젝트 삭제
        }
    }
}

