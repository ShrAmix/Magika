using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb2d;

    void Start()
    {
        // �������� ��������� Rigidbody2D ��������� ��'����
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // �������� ���� ����������� �� ���������� �� ��������
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // ��������� ������ �������� � �������� ����������
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // ���������� ������ ��������, ��� ����������� �������� �������� � ��� ���������
        movement = movement.normalized;

        // ������������ ���� �������� rigidbody2D �������� �� ������� �������
        rb2d.velocity = movement * moveSpeed;
    }
}
