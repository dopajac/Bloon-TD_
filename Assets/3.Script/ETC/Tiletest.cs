using UnityEngine;

public class ClickObjectDetector2D : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���� ���콺 Ŭ�� ����
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero); // 2D Raycast ���

            if (hit.collider != null) // Ray�� 2D ������Ʈ�� ��Ҵ��� Ȯ��
            {
                if (hit.collider.CompareTag("Wall")) // �±װ� "Wall"���� Ȯ��
                {
                    Debug.Log("������Ʈ�� Wall �Դϴ�");
                }
                else
                {
                    Debug.Log($"Ŭ���� ������Ʈ: {hit.collider.name}, �±�: {hit.collider.tag}");
                }
            }
            else
            {
                Debug.Log("Ŭ���� ��ġ�� ������Ʈ�� �����ϴ�.");
            }
        }
    }
}
