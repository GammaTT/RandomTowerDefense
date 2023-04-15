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
    private float LastSpawnTime;
    private int currentEnemyCount;

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
            enemy.SetUp(this);
            enemyList.Add(enemy);
            currentEnemyCount++;
            yield return new WaitForSeconds(SpawnDelay);
            
            //break;
        }
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
