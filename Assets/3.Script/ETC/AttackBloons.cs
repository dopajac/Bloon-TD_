using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBloons : MonoBehaviour
{

    public void DamageAllBloons()
    {
        BloonManager[] allBloons = FindObjectsOfType<BloonManager>(); // ��� ǳ�� ã��

        foreach (var bloon in allBloons)
        {
            bloon.TakeDamage(1); // ü�� 1 ����
        }
    }
}
