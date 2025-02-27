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

            // Ŭ���� ��ġ�� Ÿ�� ���ڿ� ����
            Vector2 snappedPosition = new Vector2(
                Mathf.Round(mousePosition.x / gridSize) * gridSize,
                Mathf.Round(mousePosition.y / gridSize) * gridSize
            );

            // �����: Ŭ���� ��ġ ���

            // �ش� ��ġ�� "User" �±װ� �ִ� ������Ʈ�� �ִ��� Ȯ��
            Collider2D[] colliders = Physics2D.OverlapCircleAll(snappedPosition, 0.1f);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("User")) // "User" �±װ� �ִ� ������Ʈ�� �����Ǹ�
                {
                    Check_User = collider.gameObject;
                    Canvasobj.Information_Panel.SetActive(true);

                    Check_User_inform = Check_User.GetComponent<UserManager>();

                    SpriteRenderer User_sprite;
                    User_sprite = Check_User.GetComponent<SpriteRenderer>();

                    Canvasobj.User_image.sprite = User_sprite.sprite;

                    Canvasobj.User_name.text = "���� ���� : " + Check_User_inform.UserJob;
                    Canvasobj.User_Upgrade.text = "���׷��̵� ����: " + Check_User_inform.upgrade.ToString();
                    Canvasobj.User_Attack.text = "���ݷ� : " + Check_User_inform.attack.ToString();
                    Canvasobj.User_AttackSpeed.text = "���� �ӵ� : " + Check_User_inform.attackspeed.ToString();
                }
            }
        }
    }

    
}
