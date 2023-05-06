using UnityEngine;
using UnityEngine.EventSystems;

public class TargetProjectile : MonoBehaviour
{
    private Transform target;
    //private	int			damage;
    private float damage;
    private float moveSpeed = 5.0f;

    public void Setup(Transform target, float damage)
    {
        this.target = target;                       // Ÿ���� �������� target
        this.damage = damage;                       // Ÿ���� ���ݷ�
    }

    private void Update()
    {
        if (target != null) // target�� �����ϸ�
        {
            // �߻�ü�� target�� ��ġ�� �̵�
            Vector3 direction = (target.position - transform.position).normalized;

            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else                    // ���� ������ target�� �������
        {
            // �߻�ü ������Ʈ ����
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;         // ���� �ƴ� ���� �ε�����
        if (collision.transform != target) return;          // ���� target�� ���� �ƴ� ��

        //Destroy(collision.gameObject);
        collision.GetComponent<EnemyHp>().TakeDamage(damage);   // �� ü���� damage��ŭ ����
        Destroy(gameObject);                                    // �߻�ü ������Ʈ ����
    }
}


/*
 * File : Projectile.cs
 * Desc
 *	: Ÿ���� �߻��ϴ� �⺻ �߻�ü�� ����
 *	
 * Functions
 *	: Update() - Ÿ���� �����ϸ� Ÿ�� �������� �̵��ϰ�, Ÿ���� �������� ������ �߻�ü ����
 *	: OnTriggerEnter2D() - Ÿ������ ������ ���� �ε����� �� �Ѵ� ����
 *	
 */