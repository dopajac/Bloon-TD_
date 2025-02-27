using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField] private CanvasObject canvasobj;

    
    public UserSpawner userSpawner;

    private Camera mainCamera;
    private float gridSize = 1.0f; // Ÿ�� ũ�� (�ʿ��ϸ� ����)
    [SerializeField] private LayerMask wallLayer; // "Wall"�� �����ϴ� ���̾� ����ũ

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

            // Ŭ���� ��ġ�� Ÿ�� ���ڿ� ����
            Vector2 snappedPosition = new Vector2(
                Mathf.Round(mousePosition.x / gridSize) * gridSize,
                Mathf.Round(mousePosition.y / gridSize) * gridSize
            );
            Tileposition = snappedPosition;
            // �����: Ŭ���� ��ġ ���

            // �ش� ��ġ�� "User" �±װ� �ִ� ������Ʈ�� �ִ��� Ȯ��
            Collider2D[] colliders = Physics2D.OverlapCircleAll(snappedPosition, 0.1f);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("User")) // "User" �±װ� �ִ� ������Ʈ�� �����Ǹ�
                {
                    return; // �������� ����
                }
            }
            canvasobj.SetCheckBox.SetActive(true);
            gameObject.SetActive(false);
        }
    }

}
