using UnityEngine;

[System.Serializable]
public class SpellStats
{
    public float speed = 10f; // Швидкість закляття
    public int damage = 10; // Урон закляття
    public float cooldown = 500f; // Затримка між закляттями в мілісекундах
    public Vector2 size = new Vector2(1, 1); // Розмір закляття
    public int bounces = 3; // Кількість відскоків
    public int spread; //Расброс при пострілі
}
