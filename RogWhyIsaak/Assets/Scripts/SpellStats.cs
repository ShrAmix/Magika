using UnityEngine;

[System.Serializable]
public class SpellStats
{
    public float speed = 10f; // �������� ��������
    public int damage = 10; // ���� ��������
    public float cooldown = 500f; // �������� �� ���������� � ����������
    public Vector2 size = new Vector2(1, 1); // ����� ��������
    public int bounces = 3; // ʳ������ �������
    public int spread; //������� ��� ������
}
