using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBloons : MonoBehaviour
{

    public void DamageAllBloons()
    {
        BloonManager[] allBloons = FindObjectsOfType<BloonManager>(); // 모든 풍선 찾기

        foreach (var bloon in allBloons)
        {
            bloon.TakeDamage(1); // 체력 1 감소
        }
    }
}
