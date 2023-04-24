using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLighting : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;

    public LayerMask enemyLayer;
    public float damage;

    public GameObject chianLightningEffect;

    public GameObject beenStruck;

    public int amountToChain;

    private GameObject startObject;
    public GameObject endObject;

    private Animator ani;

    public ParticleSystem particle;

    private int singleSpawns;
    // Start is called before the first frame update
    void Start()
    {
        if (amountToChain == 0)
        {
            Destroy(gameObject);
        }

        circleCollider2D = GetComponent<CircleCollider2D>();

        ani = GetComponent<Animator>();

        particle = GetComponent<ParticleSystem>();

        startObject = gameObject;

        singleSpawns = 1;
    }

    // Update is called once per frame
    void Update()
    {
        

 /*       if (Input.GetKeyDown(KeyCode.Space))
        {

        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (enemyLayer == (enemyLayer | 1 << collision.gameObject)) && !collision.GetComponentInChildren<E>

        if (collision.CompareTag("Enemy"))
        {
            if (singleSpawns != 0)
            {
                Debug.Log("Chain");

                endObject = collision.gameObject;

                amountToChain -= 1;
                Instantiate(chianLightningEffect, collision.gameObject.transform.position, Quaternion.identity);

                //Instantiate(beenStruck, collision.gameObject.transform);

                collision.gameObject.GetComponent<EnemyHp>().TakeDamage(damage);

                ani.StopPlayback();

                circleCollider2D.enabled = false;

                singleSpawns--;

                particle.Play();

                var emitParams = new ParticleSystem.EmitParams();
                emitParams.position = startObject.transform.position;

                particle.Emit(emitParams, 1);

                emitParams.position = endObject.transform.position;

                particle.Emit(emitParams, 1);

                emitParams.position = (startObject.transform.position + endObject.transform.position) / 2;

                Destroy(gameObject, 1f);
            }
        }
    }
}
