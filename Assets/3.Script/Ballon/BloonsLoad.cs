using UnityEngine;
using UnityEngine.Splines;

public class SplineMoveNoRotate : MonoBehaviour
{
    public SplineContainer splineContainer; // ����� ���ö���
    public float speed = 1.0f; // �̵� �ӵ�
    private float t = 0; // ���ö����� ���൵ (0~1)
    private Quaternion initialRotation; // �ʱ갪 ȸ��

    void Start()
    {
        initialRotation = transform.rotation; // ���� ȸ���� ����
    }

    void Update()
    {
        if (splineContainer == null) return;

        // ���൵ ����
        t += speed * Time.deltaTime;
        if (t > 1) t = 1; // ���� �����ϸ� ����

        // ���� ���ö��� ��ġ ��������
        Vector3 newPosition = splineContainer.EvaluatePosition(t);
        transform.position = newPosition;

        // �ʱ� ȸ���� ����
        transform.rotation = initialRotation;
    }
}
