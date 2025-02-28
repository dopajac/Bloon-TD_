using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    [SerializeField] public string UserJob;
    [SerializeField] public int attack;
    [SerializeField] public int cost;
    [SerializeField] public int id;
    [SerializeField] public float attackspeed;
    [SerializeField] public int upgrade;

    [SerializeField] public int level;
    [SerializeField] public float max_experience;
    [SerializeField] public float cur_experience;

    private void Update()
    {
        max_experience = level * 100;
        attack = level + upgrade;
        max_experience = level * 20;
    }

    public void Upgrade()
    {
        upgrade++;
        attack = level + upgrade;
    }

    
}
