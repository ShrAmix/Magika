using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb2d;

    void Start()
    {
        // Отримуємо компонент Rigidbody2D головного об'єкта
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Отримуємо вхід користувача по горизонталі та вертикалі
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Створюємо вектор швидкості з вхідними значеннями
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // Нормалізуємо вектор швидкості, щоб гарантувати однакову швидкість в усіх напрямках
        movement = movement.normalized;

        // Встановлюємо нову швидкість rigidbody2D відповідно до вхідних значень
        rb2d.velocity = movement * moveSpeed;
    }
}
