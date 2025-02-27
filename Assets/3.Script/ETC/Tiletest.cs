using UnityEngine;

public class ClickObjectDetector2D : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 왼쪽 마우스 클릭 감지
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero); // 2D Raycast 사용

            if (hit.collider != null) // Ray가 2D 오브젝트에 닿았는지 확인
            {
                if (hit.collider.CompareTag("Wall")) // 태그가 "Wall"인지 확인
                {
                    Debug.Log("오브젝트는 Wall 입니다");
                }
                else
                {
                    Debug.Log($"클릭한 오브젝트: {hit.collider.name}, 태그: {hit.collider.tag}");
                }
            }
            else
            {
                Debug.Log("클릭한 위치에 오브젝트가 없습니다.");
            }
        }
    }
}
