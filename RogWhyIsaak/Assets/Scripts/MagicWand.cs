using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWand : MonoBehaviour
{
    public GameObject spellPrefab; // Префаб закляття
    public Transform spawnSpell; // Точка спавну закляття
    public SpellStats spellStats; // Характеристики закляття
    private bool isCastingSpell = false;
    private Coroutine spellCoroutine;
    public Transform spellSave;

    void FixedUpdate()
    {
        WandSeeMouse();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isCastingSpell)
        {
            isCastingSpell = true;
            spellCoroutine = StartCoroutine(CastSpellsContinuously());
        }
        else if (Input.GetMouseButtonUp(0) && isCastingSpell)
        {
            isCastingSpell = false;
            StopCoroutine(spellCoroutine);
        }
    }

    void WandSeeMouse()
    {
        // Отримання позиції курсора мишки в світових координатах
        Vector3 mousePos = Input.mousePosition;

        // Перевірка, чи позиція мишки знаходиться в межах екрану
        if (mousePos.x < 0 || mousePos.x > Screen.width || mousePos.y < 0 || mousePos.y > Screen.height)
        {
            return; // Якщо ні, виходимо з функції
        }

        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // Обчислення напряму від об'єкта до курсора мишки
        Vector2 direction = new Vector2(
            mousePos.x - transform.position.x,
            mousePos.y - transform.position.y
        );

        // Обчислення кута обертання в радіанах
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Зміна обертання об'єкта
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    IEnumerator CastSpellsContinuously()
    {
        while (isCastingSpell)
        {
            CastSpell();
            yield return new WaitForSeconds(spellStats.cooldown / 1000f); // Використання затримки з характеристик закляття
        }
    }

    void CastSpell()
    {
        // Створення закляття
        GameObject spell = Instantiate(spellPrefab, spawnSpell.position, spawnSpell.rotation);
        spell.transform.SetParent(spellSave);

        // Передача характеристик закляття новому об'єкту
        Spell spellScript = spell.GetComponent<Spell>();
        if (spellScript != null)
        {
            spellScript.spellStats = spellStats;
        }

        // Отримання напряму від точки спавну до мишки
        Vector3 direction = (spawnSpell.position - transform.position).normalized;
        
        // Застосування сили до закляття в напрямку від позиції об'єкта до точки спавну
        Rigidbody2D rb = spell.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * spellStats.speed; // Встановлення швидкості закляття з характеристик
        }
    }
}
