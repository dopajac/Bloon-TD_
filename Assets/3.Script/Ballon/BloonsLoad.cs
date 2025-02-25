using UnityEngine;
using UnityEngine.Splines;

public class SplineMoveNoRotate : MonoBehaviour
{
    public SplineContainer splineContainer; // 사용할 스플라인
    public float speed = 1.0f; // 이동 속도
    private float t = 0; // 스플라인의 진행도 (0~1)
    private Quaternion initialRotation; // 초깃값 회전

    void Start()
    {
        initialRotation = transform.rotation; // 시작 회전값 저장
    }

    void Update()
    {
        if (splineContainer == null) return;

        // 진행도 갱신
        t += speed * Time.deltaTime;
        if (t > 1) t = 1; // 끝에 도달하면 멈춤

        // 현재 스플라인 위치 가져오기
        Vector3 newPosition = splineContainer.EvaluatePosition(t);
        transform.position = newPosition;

        // 초기 회전값 유지
        transform.rotation = initialRotation;
    }
}
