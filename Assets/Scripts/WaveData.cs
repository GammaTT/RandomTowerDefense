using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Image startGameButton;
    [SerializeField]
    private Image stopGameButton;

    [SerializeField]
    private Sprite startGameBlackButton;
    [SerializeField]
    private Sprite startGameWhiteButton;
    [SerializeField]
    private Sprite stopGameWhiteButton;
    [SerializeField]
    private Sprite stopGameBlackButton;

    private void Start()
    {
        textWaveCount.text = "Wave : " + 1;
    }
    public void StartWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            StartGame();
            //���� ���̺��� ���� ���ʹ� �����ʿ� �Ѱ���
            enemySpawner.StartWave(waves[currentWaveIndex]);
            textWaveCount.text = "Wave : " + (currentWaveIndex + 1);
        }
    }

    public void FinishWave()
    {
        startGameButton.sprite = startGameWhiteButton;
        currentWaveIndex++;
    }
    public void StartGame()
    {
        startGameButton.sprite = startGameBlackButton;
        stopGameButton.sprite = stopGameWhiteButton;
        Time.timeScale = 1.0f;
    }
    public void StopGame()
    {
        startGameButton.sprite = startGameWhiteButton;
        stopGameButton.sprite = stopGameBlackButton;
        Time.timeScale = 0;
    }
}

[System.Serializable]
public struct Wave
{
    public float spawnDelay;     // ���� ���̺� �� ���� �ֱ�
    public int maxEnemyCount; // ���� ���̺� �� ���� ����
    public GameObject[] enemyPrefabs;  // ���� ���̺� �� ���� ����
}