using UnityEngine;

public class BloonMovement : MonoBehaviour
{
    private Vector3 targetPosition;
    private float speed; // 이동 속도
    private BloonManager bloonManager;
    private LayerMask wallLayer; // 벽 레이어 감지용

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        Debug.Log(gameObject.name + " 목표 위치: " + targetPosition);
    }

    private void Start()
    {
        bloonManager = GetComponent<BloonManager>();
        speed = bloonManager.speed;

        // "Wall" 레이어만 감지하도록 설정
        wallLayer = LayerMask.GetMask("Wall");
    }

    private void Update()
    {
        if (targetPosition == Vector3.zero)
        {
            return;
        }

        // 현재 위치에서 targetPosition 방향으로 Ray를 쏴서 벽 감지
        Vector3 direction = (targetPosition - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, 1.0f, wallLayer)) // 특정 레이어만 감지
        {
            Debug.Log(gameObject.name + " 벽 감지! 경로 변경");

            // 벽을 피하기 위한 간단한 로직
            AvoidWall();
            return;
        }

        // 목표 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 목표 지점에 도달하면 삭제
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Debug.Log(gameObject.name + " 도착 완료!");
            //Destroy(gameObject);
        }
    }

    private void AvoidWall()
    {
        // 좌/우 또는 위/아래 방향으로 벽을 피해 이동
        Vector3[] alternativeDirections = {
            transform.right,  // 오른쪽
            -transform.right, // 왼쪽
            transform.forward, // 위쪽
            -transform.forward // 아래쪽
        };

        foreach (var newDirection in alternativeDirections)
        {
            

            if (!Physics2D.Raycast(transform.position, newDirection)) // 벽이 없는 방향 찾기
            {
                transform.position += newDirection * 0.5f; // 벽 피하기
                return;
            }
        }
    }
}
