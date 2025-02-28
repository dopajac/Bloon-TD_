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

            // 1. Ŭ���� UI�� "Panel" Layer�� ���ϴ��� Ȯ��
            if (IsPointerOverSpecificUILayer("Panel"))
            {
                Debug.Log("Panel ���� Ŭ���߱� ������ �������� �ʽ��ϴ�.");
                return;
            }

            // 2. Ŭ���� ��ġ�� Wall Layer���� Ȯ�� (Physics2D.Raycast)
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

            //3. Ŭ���� ��ġ�� "User" �±װ� �ִ� ������Ʈ�� �ִ��� Ȯ��
            Collider2D[] colliders = Physics2D.OverlapCircleAll(snappedPosition, 0.1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("User")) // "User" �±װ� �ִ� ������Ʈ�� �����Ǹ�
                {
                    return; // �������� ����
                }
            }
            if (GameManager.instance.meso < 100)
            {
                Debug.Log("���� ������ ~");
                return;
            }
            // UI Ȱ��ȭ
            CanvasObject.instance.SetCheckBox.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    // Ư�� UI ����� Layer�� "Panel"���� Ȯ���ϴ� �Լ�
    private bool IsPointerOverSpecificUILayer(string layerName)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition // ���� ���콺 ��ġ
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        int panelLayer = LayerMask.NameToLayer(layerName); // "Panel" Layer ��ȣ ��������

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer == panelLayer) // Ŭ���� UI ��Ұ� Panel Layer���� Ȯ��
            {
                return true;
            }
        }

        return false; // Panel�� �ƴ�
    }
}
