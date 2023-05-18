using System.Collections;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    public float maxHp;          // �ִ� ü��
    public float currentHp;      // ���� ü��
    private bool isDie = false;  // ���� ��� �����̸� isDie�� true�� ����

    private Enemy enemy;
    private SpriteRenderer spriteRenderer;
    private EnemyHpViewer enemyHpViewer;


    public void SetUp(EnemyHpViewer enemyHpViewer)
    {
        this.enemyHpViewer = enemyHpViewer;
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        maxHp = enemy.enemyData.maxHp;
        currentHp = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log("enemyhpstart : " + maxHp + " " + currentHp);
    }

    public void TakeDamage(float damage)
    {
        // Tip. ���� ü���� damage ��ŭ �����ؼ� ���� ��Ȳ�� �� ���� Ÿ���� ������ ���ÿ� ������
        // enemy.OnDie() �Լ��� ���� �� ����� �� �ִ�.

        // ���� ���� ���°� ��� �����̸� �Ʒ� �ڵ带 �������� �ʴ´�.
        if (isDie == true) return;

        // ���� ü���� damage��ŭ ����
        currentHp -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        enemyHpViewer.hpSliderUpdate();

        // ü���� 0���� = �� ĳ���� ���
        if (currentHp <= 0)
        {
            isDie = true;
            // �� ĳ���� ���
            enemy.OnDie(EnemyDestroyType.Kill);
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        // ���� ���� ������ color ������ ����
        Color color = spriteRenderer.color;

        // ���� ������ 40%�� ����
        color.a = 0.4f;
        spriteRenderer.color = color;

        // 0.05�� ���� ���
        yield return new WaitForSeconds(0.05f);

        // ���� ������ 100%�� ����
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}


/*
 * File : EnemyHP.cs
 * Desc
 *	: �� ĳ������ ü��
 *	
 * Functions
 *	: TakeDamage() - ü�� ����
 *	: HitAlphaAnimation() - ������ 100% -> 40% -> 100%�� ����
 */