using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private UserSpawner userSpawner;

    private Camera mainCamera;
    private float gridSize = 1.0f; // Ÿ�� ũ�� (�ʿ��ϸ� ����)
    [SerializeField] private LayerMask wallLayer; // "Wall"�� �����ϴ� ���̾� ����ũ

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            // ��(Wall) ���̾ �����ϵ��� ����
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, wallLayer);

            if (hit.collider != null)
            {
            }
            else
            {
            }

            if (hit.collider != null && hit.collider.CompareTag("Wall"))
            {
                Vector2 snappedPosition = new Vector2(
                    Mathf.Round(mousePosition.x / gridSize) * gridSize,
                    Mathf.Round(mousePosition.y / gridSize) * gridSize
                );

                userSpawner.UserSpawn(snappedPosition);
            }
        }
    }
}
