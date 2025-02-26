using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    [SerializeField] public string UserName;
    [SerializeField] public int attack;
    [SerializeField] public int cost;
    [SerializeField] public int id;
    [SerializeField] public float attackspeed;
    [SerializeField] public int upgrade; // 100 , 10, 1 자리수로 판단
}
