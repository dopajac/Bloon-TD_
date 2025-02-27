using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private UserSpawner userSpawner;

    private Camera mainCamera;
    private float gridSize = 1.0f; // 타일 크기 (필요하면 조정)
    [SerializeField] private LayerMask wallLayer; // "Wall"만 감지하는 레이어 마스크

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Mouse Position: " + mousePosition);

            // 벽(Wall) 레이어만 감지하도록 설정
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, wallLayer);

            if (hit.collider != null)
            {
                Debug.Log("Hit Object: " + hit.collider.gameObject.name + ", Tag: " + hit.collider.tag);
            }
            else
            {
                Debug.Log("No object detected at this position!");
            }

            if (hit.collider != null && hit.collider.CompareTag("Wall"))
            {
                Vector2 snappedPosition = new Vector2(
                    Mathf.Round(mousePosition.x / gridSize) * gridSize,
                    Mathf.Round(mousePosition.y / gridSize) * gridSize
                );

                Debug.Log("Snapped Position: " + snappedPosition);

                userSpawner.UserSpawn(snappedPosition);
            }
        }
    }
}
