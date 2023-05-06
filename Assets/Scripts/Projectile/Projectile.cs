using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private Vector3 target;
    private float moveSpeed = 4.0f;
    private float damage;
    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    public void Setup(Vector3 target, float damage)
    {
        this.target = target;
        this.damage = damage;
        AddForceToTarget(this.target);
    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyThisProjectile", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddForceToTarget(Vector3 target)
    {
        if (rigidbody2d == null) { Debug.Log("rigid null"); }
        //Vector3 position = transform.position;
        //Vector3 direction = (target - position).normalized;
        //rigidbody2d.AddForce(target.normalized * moveSpeed);
        rigidbody2d.velocity = target * moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null || !collision.CompareTag("Enemy"))
        {
            return;
        }
        collision.GetComponent<EnemyHp>().TakeDamage(damage);   // 적 체력을 damage만큼 감소
        Destroy(gameObject);                                    // 발사체 오브젝트 삭제
    }

    void DestroyThisProjectile()
    {
        Destroy(gameObject);
    }
}
