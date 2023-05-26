using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BombProjectile : MonoBehaviour
{
    private Vector3 target;
    private float damage;
    private float moveSpeed = 8.0f;
    private float bombRadius = 2.0f;
    private float bombLifeTime = 0.5f;
    private float bombStartDistance = 0.1f;
    private bool isExploding = false;
    //private bool isBomb = false;

    [SerializeField]
    private ParticleSystem bombParticle;

    public void Setup(Transform target, float damage)
    {
        this.target = target.position;
        this.damage = damage;
    }

    private void Update()
    {
        if (!isExploding)
        {
            Vector3 direction = (target - transform.position).normalized;

            transform.position += direction * moveSpeed * Time.deltaTime;

            if (Vector2.Distance(transform.position, target) < bombStartDistance)
            {
                isExploding = true;

                StartCoroutine("Bomb");
            }
        }
    }

    private IEnumerator Bomb()
    {
        bombParticle.Play();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bombRadius * 0.5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") && collider != null)
            {
                EnemyHp enemyHp = collider.GetComponent<EnemyHp>();
                enemyHp.TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(bombLifeTime);
        Destroy(gameObject);
    }
}