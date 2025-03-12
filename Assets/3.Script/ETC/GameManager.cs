using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private UserInformation userinfo;
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
        TryGetComponent(out userinfo);
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    private void Start()
    {
        meso = 500;
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
        
        if (userinfo.Check_User_inform.max_experience > 0)
        {
            CanvasObject.instance.sliderEXP.value = userinfo.Check_User_inform.cur_experience / userinfo.Check_User_inform.max_experience;
        }
        else
        {
            CanvasObject.instance.sliderEXP.value = 0; // 방어 코드 (0으로 나누는 오류 방지)
        }
        CanvasObject.instance.User_exp.text = userinfo.Check_User_inform.cur_experience.ToString() + " / " + userinfo.Check_User_inform.max_experience.ToString();
        Debug.Log($"스테이지 클리어! 모든 유저에게 {experienceToAdd} 경험치 추가됨.");
    }


}
