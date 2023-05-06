using System.Collections;
using Unity.VisualScripting;
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
    private float bombTime = 0.5f;
    private float bombStartDistance = 0.5f;
    private bool isExploding = false;
    //private bool isBomb = false;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private ParticleSystem bombParticle;
    private void Start()
    {
        currentTime = Time.time;
        spriteRenderer = GetComponent<SpriteRenderer>();
        //StartCoroutine("Bomb");
    }
    public void Setup(Transform target, float damage)
    {
        this.target = target;                       // 타워가 설정해준 target
        this.damage = damage;                       // 타워의 공격력
    }

    private void Update()
    {
        if (target != null && !isExploding) // target이 존재하면
        {
            // 발사체를 target의 위치로 이동
            Vector3 direction = (target.position - transform.position).normalized;

            transform.position += direction * moveSpeed * Time.deltaTime;

            if (Vector2.Distance(transform.position, target.position) < bombStartDistance)
            {
                isExploding = true;

                StartCoroutine("Bomb");
            }
        }
        else if (target == null)
        {
            // 발사체 오브젝트 삭제
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collision object is not enemy
        if (!collision.CompareTag("Enemy")) return;   

/*        if (!isExploding && !isBomb) 
        {
            isExploding = true;
            transform.position = (transform.position + target.position ) / 2;
            StartCoroutine("Bomb");
            return;
        }*/
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
                //isBomb = true;
                // 폭발 반경 내의 적에게 데미지 적용
                // OverlapCircle의 크기가 보이는 오브젝트의 크기보다 크기 때문에 0.5를 곱해서 줄인다.
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bombRadius * 0.5f);
                foreach (Collider2D col in colliders)
                {
                    if (col != null && col.CompareTag("Enemy"))
                    {
                        spriteRenderer.color = new Color(0f, 0f, 0f, 0f);
                        col.GetComponent<EnemyHp>().TakeDamage(damage);
                        bombParticle.Play();
                        yield return new WaitForSeconds(bombTime);
                        Destroy(gameObject);
                    }
                }
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}