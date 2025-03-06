using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipController : MonoBehaviour
{
    public Transform user; // ����(�÷��̾�) ĳ������ Transform
    public GameObject target; // ���� Ÿ�� (���� ��)
    private SpriteRenderer spriteRenderer; // ��������Ʈ ������

    public Range range;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ ��������

        if (user == null)
        {
            Debug.LogError("FlipController: ����(�÷��̾�) ������Ʈ�� �������� �ʾҽ��ϴ�!");
        }
    }

    void Update()
    {
        if (target == null) return; // Ÿ���� ������ �������� ����

        // Ÿ���� ������ �����ʿ� ������ Flip
        if (target.transform.position.x > user.position.x)
        {
            spriteRenderer.flipX = true; // �������� �� Flip Ȱ��ȭ
        }
        else
        {
            spriteRenderer.flipX = false; // ������ �� ���� ���� ����
        }
    }
}
