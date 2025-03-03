using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int meso; // 메소
    [SerializeField]
    public int life;
    [SerializeField]
    public int StageLevel;
    [SerializeField]
    public int StageExperience;
    [SerializeField]
    public bool isStagefinish=false;
    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    private void Start()
    {
        meso = 100;
        life = 100;
    }

    private void Update()
    {
        CanvasObject.instance.User_life.text = "목숨 : " + life;
        CanvasObject.instance.User_meso.text = "메소 : " + meso;
    }

    public void AddExperienceToUsers()
    {
        int experienceToAdd = StageExperience;

        // 모든 UserManager 컴포넌트를 찾음
        UserManager[] users = FindObjectsOfType<UserManager>();

        foreach (UserManager user in users)
        {
            user.Getexperience(experienceToAdd);
        }

        Debug.Log($"스테이지 클리어! 모든 유저에게 {experienceToAdd} 경험치 추가됨.");
    }


}
