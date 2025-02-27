using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInformation : MonoBehaviour
{
    [SerializeField] private CanvasObject Canvasobj;

    public UserManager Check_User_inform;
    private Camera mainCamera;

    private float gridSize = 1.0f;

    [SerializeField]
    public GameObject Check_User;

    private void Awake()
    {
        mainCamera = Camera.main;
        Canvasobj = GetComponent<CanvasObject>();
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
                    Canvasobj.Information_Panel.SetActive(true);

                    Check_User_inform = Check_User.GetComponent<UserManager>();

                    SpriteRenderer User_sprite;
                    User_sprite = Check_User.GetComponent<SpriteRenderer>();

                    Canvasobj.User_image.sprite = User_sprite.sprite;

                    Canvasobj.User_name.text = "유저 직업 : " + Check_User_inform.UserJob;
                    Canvasobj.User_Upgrade.text = "업그레이드 레벨: " + Check_User_inform.upgrade.ToString();
                    Canvasobj.User_Attack.text = "공격력 : " + Check_User_inform.attack.ToString();
                    Canvasobj.User_AttackSpeed.text = "공격 속도 : " + Check_User_inform.attackspeed.ToString();
                }
            }
        }
    }

    
}
