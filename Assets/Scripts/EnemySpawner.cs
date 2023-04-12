using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Enemy01Prefab;
    [SerializeField]
    private float SpawnDelay;
    private float LastSpawnTime;

    private 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnEnemy");
    }

    // Update is called once per frame
    void Update()
    {
/*        if (LastSpawnTime - Time.time > SpawnDelay)
        {
            LastSpawnTime = Time.time;
        }*/
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject Enemy = Instantiate(Enemy01Prefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(SpawnDelay);
            //break;
        }
    }
}
