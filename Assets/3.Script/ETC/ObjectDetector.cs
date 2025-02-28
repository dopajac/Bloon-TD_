using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ObjectDetector : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask wallLayer;
    public float gridSize = 3f;
    public Vector2 Tileposition;
    [SerializeField] public UserSpawner userSpawner;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // 1. 클릭한 UI가 "Panel" Layer에 속하는지 확인
            if (IsPointerOverSpecificUILayer("Panel"))
            {
                Debug.Log("Panel 위를 클릭했기 때문에 동작하지 않습니다.");
                return;
            }

            // 2. 클릭한 위치가 Wall Layer인지 확인 (Physics2D.Raycast)
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, wallLayer);
            if (hit.collider == null || !hit.collider.CompareTag("Wall"))
            {
                return;
            }

            // 클릭한 위치를 타일 격자에 맞춤
            Vector2 snappedPosition = new Vector2(
                Mathf.Round(mousePosition.x / gridSize) * gridSize,
                Mathf.Round(mousePosition.y / gridSize) * gridSize
            );
            Tileposition = snappedPosition;

            //3. 클릭한 위치에 "User" 태그가 있는 오브젝트가 있는지 확인
            Collider2D[] colliders = Physics2D.OverlapCircleAll(snappedPosition, 0.1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("User")) // "User" 태그가 있는 오브젝트가 감지되면
                {
                    return; // 생성하지 않음
                }
            }
            if (GameManager.instance.meso < 100)
            {
                Debug.Log("돈이 업성요 ~");
                return;
            }
            // UI 활성화
            CanvasObject.instance.SetCheckBox.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    // 특정 UI 요소의 Layer가 "Panel"인지 확인하는 함수
    private bool IsPointerOverSpecificUILayer(string layerName)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition // 현재 마우스 위치
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        int panelLayer = LayerMask.NameToLayer(layerName); // "Panel" Layer 번호 가져오기

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer == panelLayer) // 클릭된 UI 요소가 Panel Layer인지 확인
            {
                return true;
            }
        }

        return false; // Panel이 아님
    }
}
