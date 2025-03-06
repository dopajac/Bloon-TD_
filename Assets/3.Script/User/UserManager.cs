using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UserManager : MonoBehaviour
{
    [SerializeField] public string UserJob;
    [SerializeField] public int JobLevel=0;
    [SerializeField] public int attack;
    [SerializeField] public int cost;
    [SerializeField] public int id;
    [SerializeField] public float attackspeed;
    [SerializeField] public int upgrade;

    [SerializeField] public int level = 1;
    [SerializeField] public float max_experience;
    [SerializeField] public float cur_experience;

    private void Start()
    {

        UpdateStatus();
    }

    private void UpdateStatus()
    {
        max_experience = level * 100; 
        attack = level + upgrade;
    }

    public void Upgrade()
    {
        upgrade++;
        UpdateStatus();
    }

    public void Getexperience(int stageExperience)
    {
        cur_experience += stageExperience;

        while (cur_experience >= max_experience)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        cur_experience -= max_experience; 
        level++;
        UpdateStatus(); 
    }

    
}
