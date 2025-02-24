using UnityEngine;

public class BloonMovement : MonoBehaviour
{
    private Vector3 targetPosition;
    private float speed; // �̵� �ӵ�
    private BloonManager bloonManager;
    private LayerMask wallLayer; // �� ���̾� ������

   
    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
       
    }

    private void Start()
    {
        bloonManager = GetComponent<BloonManager>();
        speed = bloonManager.speed;

        // "Wall" ���̾� ���� ����
        wallLayer = LayerMask.GetMask("Wall");

        // ǳ������ �浹 ���� (Bloon ���̾��� Layer ��ȣ�� �����ؾ� ��)
        int bloonLayer = LayerMask.NameToLayer("Bloon");
        Physics2D.IgnoreLayerCollision(bloonLayer, bloonLayer, true);
    }

    private void Update()
    {
        if (targetPosition == Vector3.zero)
        {
            return;
        }

        // ���� ��ġ���� targetPosition �������� Ray�� ���� �� ����
        Vector3 direction = (targetPosition - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, 1.0f, wallLayer)) // Ư�� ���̾ ����
        {
            Debug.Log(gameObject.name + " �� ����! ��� ����");

            // ���� ���ϱ� ���� ������ ����
            AvoidWall();
            return;
        }

        // ��ǥ ��ġ�� �̵�
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // ��ǥ ������ �����ϸ� ����
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Debug.Log(gameObject.name + " ���� �Ϸ�!");
            //Destroy(gameObject);
        }
    }

    private void AvoidWall()
    {
        // ��/�� �Ǵ� ��/�Ʒ� �������� ���� ���� �̵�
        Vector3[] alternativeDirections = {
            transform.right,  // ������
            -transform.right, // ����
            transform.forward, // ����
            -transform.forward // �Ʒ���
        };

        foreach (var newDirection in alternativeDirections)
        {
            
            
            if (!Physics2D.Raycast(transform.position, newDirection)) // ���� ���� ���� ã��
            {
                transform.position += newDirection * 0.5f; // �� ���ϱ�
                return;
            }
        }

    }
    
}
