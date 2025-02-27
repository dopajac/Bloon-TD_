using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField] private CanvasObject canvasobj;

    
    public UserSpawner userSpawner;

    private Camera mainCamera;
    private float gridSize = 1.0f; // 타일 크기 (필요하면 조정)
    [SerializeField] private LayerMask wallLayer; // "Wall"만 감지하는 레이어 마스크

    public Vector2 Tileposition;
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
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
            // 디버깅: 클릭한 위치 출력

            // 해당 위치에 "User" 태그가 있는 오브젝트가 있는지 확인
            Collider2D[] colliders = Physics2D.OverlapCircleAll(snappedPosition, 0.1f);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("User")) // "User" 태그가 있는 오브젝트가 감지되면
                {
                    return; // 생성하지 않음
                }
            }
            canvasobj.SetCheckBox.SetActive(true);
            gameObject.SetActive(false);
        }
    }

}
