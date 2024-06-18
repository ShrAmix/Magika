using UnityEngine;

public class Spell : MonoBehaviour
{
    public SpellStats spellStats; // ��������� �� �������������� ��������

    private Rigidbody2D rb;
    private int remainingBounces;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        remainingBounces = spellStats.bounces;

        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-spellStats.spread / 2, spellStats.spread / 2));

        // ������������ �������� ��������
        rb.velocity = transform.right * spellStats.speed;

        // ������������ ������ ��������
        transform.localScale = new Vector3(spellStats.size.x, spellStats.size.y, 1);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // ����������, �� �������� �������� � ���������� � ����� "Bord"
        if (collider.gameObject.CompareTag("Bord"))
        {
            // ��������� �������� ��� �������
            rb.velocity *= 0.8f; // ���������, �������� �������� �� 20%

            // ³����� ��������
            if (remainingBounces > 0)
            {
                remainingBounces--;
                // �������� ������ ������� �� ������� ��������
                Vector2 reflectDir = Vector2.Reflect(rb.velocity.normalized, Vector2.Perpendicular(collider.transform.position - transform.position).normalized);
                rb.velocity = reflectDir * rb.velocity.magnitude;
            }
            else
            {
                // �������� �������� ���� ���������� ������� �������
                Destroy(gameObject);
            }
        }
    }




}
