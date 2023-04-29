using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BombProjectile : MonoBehaviour
{
    private Transform target;
    //private	int			damage;
    private float damage;
    private float moveSpeed = 5.0f;
    private float currentTime;
    private float bombRadius = 2.0f;
    private float bombDuration = 1.0f;
    private bool isExploding = false;
    private bool isBomb = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentTime = Time.time;
        spriteRenderer = GetComponent<SpriteRenderer>();
        //StartCoroutine("Bomb");
    }
    public void Setup(Transform target, float damage)
    {
        this.target = target;                       // Ÿ���� �������� target
        this.damage = damage;                       // Ÿ���� ���ݷ�
    }

    private void Update()
    {
        if (target != null && !isExploding) // target�� �����ϸ�
        {
            // �߻�ü�� target�� ��ġ�� �̵�
            Vector3 direction = (target.position - transform.position).normalized;

            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else if (target == null)              // ���� ������ target�� �������
        {
            // �߻�ü ������Ʈ ����
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collision object is not enemy
        if (!collision.CompareTag("Enemy")) return;   

        if (!isExploding && !isBomb) 
        {
            isExploding = true;
            transform.position = target.position;
            StartCoroutine("Bomb");
            return;
        }
    }

    private IEnumerator Bomb()
    {
        //isExploding = true;
        currentTime = Time.time;
        while (true)
        {
            float u = (Time.time - currentTime) / bombDuration;
            transform.localScale = Vector3.one * u * bombRadius;
            Color color = Color.red * u;
            spriteRenderer.color = color;
            if (1.0f <= u)
            {
                isBomb = true;
                // ���� �ݰ� ���� ������ ������ ����
                // overlapcircle is big than shown object size so * 0.5f to right blance
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bombRadius * 0.5f);
                foreach (Collider2D col in colliders)
                {
                    if (col.CompareTag("Enemy"))
                    {
                        col.GetComponent<EnemyHp>().TakeDamage(damage);
                        Destroy(gameObject);
                    }
                }
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
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