using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject Enemy01Prefab;
    [SerializeField]
    private float SpawnDelay;
    [SerializeField]
    private WaveData waveSystem;


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
            GameObject enemyObject = Instantiate(currentWave.enemyPrefabs[Random.Range(0, currentWave.enemyPrefabs.Length)],
                transform.position, Quaternion.identity);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemy.SetUp(this);
            enemyList.Add(enemy);
            currentEnemyCount++;

            spawnEnemyCount++;
            yield return new WaitForSeconds(currentWave.spawnDelay);
            
            //break;
        }

        waveSystem.FinishWave();
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        // ���� ��ǥ�������� �������� ��
        if (type == EnemyDestroyType.Arrive)
        {
            // �÷��̾��� ü�� -1
            player.TakeDamage(1);
        }
        // ���� �÷��̾��� �߻�ü���� ������� ��
        else if (type == EnemyDestroyType.Kill)
        {
            // ���� ������ ���� ��� �� ��� ȹ��
            player.gold += gold;
        }

        // ���� ����� ������ ���� ���̺��� ���� �� ���� ���� (UI ǥ�ÿ�)
        currentEnemyCount--;
        // ����Ʈ���� ����ϴ� �� ���� ����
        enemyList.Remove(enemy);
        // �� ������Ʈ ����
        Destroy(enemy.gameObject);

        
    }
}
