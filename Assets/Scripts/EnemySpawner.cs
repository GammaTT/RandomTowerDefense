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

    public List<Enemy> enemyList;
    // Start is called before the first frame update
    void Start()
    {
        enemyList = new List<Enemy>();
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
            GameObject enemyObject = Instantiate(Enemy01Prefab, transform.position, Quaternion.identity);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemyList.Add(enemy);
            yield return new WaitForSeconds(SpawnDelay);
            
            //break;
        }
    }
}
