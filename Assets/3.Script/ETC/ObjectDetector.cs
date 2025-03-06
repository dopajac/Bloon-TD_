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

            if (IsPointerOverSpecificUILayer("Panel"))
            {
                Debug.Log("Panel 위를 클릭했기 때문에 동작하지 않습니다.");
                return;
            }


            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, wallLayer);
            if (hit.collider == null || !hit.collider.CompareTag("Wall"))
            {
                return;
            }

            Vector2 snappedPosition = new Vector2(
                Mathf.Round(mousePosition.x / gridSize) * gridSize,
                Mathf.Round(mousePosition.y / gridSize) * gridSize
            );
            Tileposition = snappedPosition;


            Collider2D[] colliders = Physics2D.OverlapCircleAll(snappedPosition, 0.1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("User")) 
                {
                    return; 
                }
            }
            if (GameManager.instance.meso < 100)
            {
                Debug.Log("돈이 업성요 ~");
                return;
            }
 
            CanvasObject.instance.SetCheckBox.SetActive(true);
            gameObject.SetActive(false);
        }
    }


    private bool IsPointerOverSpecificUILayer(string layerName)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition 
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        int panelLayer = LayerMask.NameToLayer(layerName); 

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer == panelLayer) 
            {
                return true;
            }
        }

        return false; 
    }
}
