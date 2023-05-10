using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float bombTime = 0.5f;
    private float currentTime;
    private float damage;
    bool isExplode = false;


    [SerializeField]
    private ParticleSystem explodeParticle;

    public void SetUp(float damage)
    {
        this.damage = damage;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float u = (Time.time - currentTime) / bombTime;

        transform.localScale = new Vector3(u, u, u) * 1.0f;

        if (u >= 1 && !isExplode)
        {
            //ExplodeBomb();
            isExplode = true;
            GetComponent<SpriteRenderer>().enabled = false;
            explodeParticle.Play();
            Invoke("DestoryThis", 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        if (isExplode) 
        {
            collision.GetComponent<EnemyHp>().TakeDamage(damage);
        }
    }

    private void DestoryThis()
    {
        Destroy(this.gameObject);
    }
/*    public void ExplodeBomb()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (Collider2D col in colliders)
        {
            if (col != null && col.CompareTag("Enemy"))
            {
                col.GetComponent<EnemyHp>().TakeDamage(damage);
                bombParticle.Play();
                yield return new WaitForSeconds(bombTime);
                Destroy(gameObject);
            }
        }
    }*/
}
