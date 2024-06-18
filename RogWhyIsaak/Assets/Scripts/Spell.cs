using UnityEngine;

public class Spell : MonoBehaviour
{
    public SpellStats spellStats; // Посилання на характеристики закляття

    private Rigidbody2D rb;
    private int remainingBounces;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        remainingBounces = spellStats.bounces;

        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-spellStats.spread / 2, spellStats.spread / 2));

        // Встановлення швидкості закляття
        rb.velocity = transform.right * spellStats.speed;

        // Встановлення розміру закляття
        transform.localScale = new Vector3(spellStats.size.x, spellStats.size.y, 1);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Перевіряємо, чи зіткнення відбулося з колайдером з тегом "Bord"
        if (collider.gameObject.CompareTag("Bord"))
        {
            // Зменшення швидкості при зіткненні
            rb.velocity *= 0.8f; // Наприклад, зниження швидкості на 20%

            // Відскок закляття
            if (remainingBounces > 0)
            {
                remainingBounces--;
                // Отримуємо напрям відбиття від поверхні зіткнення
                Vector2 reflectDir = Vector2.Reflect(rb.velocity.normalized, Vector2.Perpendicular(collider.transform.position - transform.position).normalized);
                rb.velocity = reflectDir * rb.velocity.magnitude;
            }
            else
            {
                // Знищення закляття після досягнення кількості відскоків
                Destroy(gameObject);
            }
        }
    }




}
