using TMPro;
using UnityEngine;

public class WaveData : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;                   // ���� ���������� ��� ���̺� ����
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private TextMeshProUGUI textWaveCount;

    private int currentWaveIndex = 0;

    // ���̺� ���� ����� ���� Get ������Ƽ (���� ���̺�, �� ���̺�)
    public int MaxWave => waves.Length;

    public void StartWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            //���� ���̺��� ���� ���ʹ� �����ʿ� �Ѱ���
            enemySpawner.StartWave(waves[currentWaveIndex]);
            textWaveCount.text = "Wave : " + (currentWaveIndex + 1);
            currentWaveIndex++;
        }
    }
}

[System.Serializable]
public struct Wave
{
    public float spawnDelay;     // ���� ���̺� �� ���� �ֱ�
    public int maxEnemyCount; // ���� ���̺� �� ���� ����
    public GameObject[] enemyPrefabs;  // ���� ���̺� �� ���� ����
}