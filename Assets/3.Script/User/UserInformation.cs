using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInformation : MonoBehaviour
{

    public UserManager Check_User_inform;
    private Camera mainCamera;

    private float gridSize = 1.0f;

    [SerializeField]
    public GameObject Check_User;
    SpriteRenderer User_sprite;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // 클릭한 위치를 타일 격자에 맞춤
            Vector2 snappedPosition = new Vector2(
                Mathf.Round(mousePosition.x / gridSize) * gridSize,
                Mathf.Round(mousePosition.y / gridSize) * gridSize
            );

            // 디버깅: 클릭한 위치 출력

            // 해당 위치에 "User" 태그가 있는 오브젝트가 있는지 확인
            Collider2D[] colliders = Physics2D.OverlapCircleAll(snappedPosition, 0.1f);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("User")) // "User" 태그가 있는 오브젝트가 감지되면
                {
                    Check_User = collider.gameObject;
                    CanvasObject.instance.Information_Panel.SetActive(true);

                    Check_User_inform = Check_User.GetComponent<UserManager>();

                    
                    User_sprite = Check_User.GetComponent<SpriteRenderer>();

                    SetInfoPanel();
                    OpenJobUpgradeButton();
                }
            }
        }
    }

    public void SetInfoPanel()
    {
        CanvasObject.instance.User_image.sprite = User_sprite.sprite;
        CanvasObject.instance.User_Lv.text = "Lv. : " + Check_User_inform.level;
        CanvasObject.instance.User_name.text = "유저 직업 : " + Check_User_inform.UserJob;
        CanvasObject.instance.User_Upgrade.text = "강화 레벨: " + Check_User_inform.upgrade.ToString();
        CanvasObject.instance.User_Attack.text = "공격력 : " + Check_User_inform.attack.ToString();
        CanvasObject.instance.User_exp.text = Check_User_inform.cur_experience.ToString() + " / " + Check_User_inform.max_experience.ToString();
        
        if (Check_User_inform.max_experience > 0)
        {
            CanvasObject.instance.sliderEXP.value = Check_User_inform.cur_experience / Check_User_inform.max_experience;
        }
        else
        {
            CanvasObject.instance.sliderEXP.value = 0; // 방어 코드 (0으로 나누는 오류 방지)
        }

    }
    public bool OpenJobUpgrade()
    {
        int requiredLevel = Check_User_inform.JobLevel switch
        {
            0 => 10,
            1 => 20,
            2 => 30,
            _ => int.MaxValue // 기본적으로 불가능하도록 설정
        };

        return Check_User_inform.level >= requiredLevel;
    }


    public void OpenJobUpgradeButton()
    {
        if (OpenJobUpgrade())
        {
            CanvasObject.instance.JobUpgradeButton = CanvasObject.instance.JobUpgradeButton.transform.GetComponent<Button>();
            CanvasObject.instance.JobUpgradeButton.interactable = true;
        }
        
    }
}
