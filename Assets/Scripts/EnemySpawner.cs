using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WaveData;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject Enemy01Prefab;
    [SerializeField]
    private float SpawnDelay;
    [SerializeField]
    private WaveSystem waveSystem;


    private float LastSpawnTime;
    private int currentEnemyCount;
    private Wave currentWave;
    private int currentWaveCount;
    public List<Enemy> enemyList;
    // Start is called before the first frame update
    void Start()
    {
        enemyList = new List<Enemy>();
        //StartCoroutine("SpawnEnemy");
    }

    // Update is called once per frame
    void Update()
    {
/*        if (LastSpawnTime - Time.time > SpawnDelay)
        {
            LastSpawnTime = Time.time;
        }*/
    }
    public void CheckPathForAllEnemy()
    {
        foreach(Enemy enemy in enemyList)
        {
            if (enemy != null)
            {
                enemy.SetPath();
            }
        }
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;
        currentWaveCount = wave.maxEnemyCount;
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy()
    {
        int spawnEnemyCount = 0;
        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {
            float randomSpawnRoll = Random.value;
            GameObject selectEnemy = null;

            foreach(var enemyInWave in currentWave.enemies)
            {
                if (randomSpawnRoll <= enemyInWave.enemyPercentage)
                {
                    selectEnemy = enemyInWave.enemyPrefab;
                    break;
                }

                randomSpawnRoll -= enemyInWave.enemyPercentage;
            }

            if (selectEnemy != null)
            {
                GameObject enemyObject = Instantiate(selectEnemy, transform.position, Quaternion.identity);
                Enemy enemy = enemyObject.GetComponent<Enemy>();
                enemy.SetUp(this);
                enemyList.Add(enemy);
                currentEnemyCount++;

                spawnEnemyCount++;
                yield return new WaitForSeconds(currentWave.spawnDelay);
            }
        }

        waveSystem.FinishWave();
    }

    //그냥 에너미 스크립트에서 얻어오는게 낫나?
    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold, int score)
    {
        // 적이 목표지점까지 도착했을 때
        if (type == EnemyDestroyType.Arrive)
        {
            // 플레이어의 체력 -1
            player.TakeDamage(1);
        }
        // 적이 플레이어의 발사체에게 사망했을 떄
        else if (type == EnemyDestroyType.Kill)
        {
            // 적의 종류에 따라 사망 시 골드 획득
            player.gold += gold;
            waveSystem.scoreSystem.AddScore(score);
        }

        // 적이 사망할 때마다 현재 웨이브의 생존 적 숫자 감소 (UI 표시용)
        currentEnemyCount--;

        // 리스트에서 사망하는 적 정보 삭제
        enemyList.Remove(enemy);
        // 적 오브젝트 삭제
        Destroy(enemy.gameObject);

        
    }
}
