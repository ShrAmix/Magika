using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWand : MonoBehaviour
{
    public GameObject spellPrefab; // ������ ��������
    public Transform spawnSpell; // ����� ������ ��������
    public SpellStats spellStats; // �������������� ��������
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
        // ��������� ������� ������� ����� � ������� �����������
        Vector3 mousePos = Input.mousePosition;

        // ��������, �� ������� ����� ����������� � ����� ������
        if (mousePos.x < 0 || mousePos.x > Screen.width || mousePos.y < 0 || mousePos.y > Screen.height)
        {
            return; // ���� �, �������� � �������
        }

        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // ���������� ������� �� ��'���� �� ������� �����
        Vector2 direction = new Vector2(
            mousePos.x - transform.position.x,
            mousePos.y - transform.position.y
        );

        // ���������� ���� ��������� � �������
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ���� ��������� ��'����
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    IEnumerator CastSpellsContinuously()
    {
        while (isCastingSpell)
        {
            CastSpell();
            yield return new WaitForSeconds(spellStats.cooldown / 1000f); // ������������ �������� � ������������� ��������
        }
    }

    void CastSpell()
    {
        // ��������� ��������
        GameObject spell = Instantiate(spellPrefab, spawnSpell.position, spawnSpell.rotation);
        spell.transform.SetParent(spellSave);

        // �������� ������������� �������� ������ ��'����
        Spell spellScript = spell.GetComponent<Spell>();
        if (spellScript != null)
        {
            spellScript.spellStats = spellStats;
        }

        // ��������� ������� �� ����� ������ �� �����
        Vector3 direction = (spawnSpell.position - transform.position).normalized;
        
        // ������������ ���� �� �������� � �������� �� ������� ��'���� �� ����� ������
        Rigidbody2D rb = spell.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * spellStats.speed; // ������������ �������� �������� � �������������
        }
    }
}
