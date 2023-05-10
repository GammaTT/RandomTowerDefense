using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
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

    public Wave[] waveboo;
    private void Start()
    {
        textWaveCount.text = "Wave : " + 1;
        SaveToJson();
        //LoadWaves();
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

    public void SaveToJson()
    {
        Wave[] boo = new Wave[waves.Length];
        boo = waves;
        string json = JsonUtility.ToJson(boo, true);
        string filePath = Path.Combine(Application.dataPath, "Resource", "waves.json");
        //File.WriteAllText(filePath, json);

        Debug.Log(json);

        //StreamWriter file = new StreamWriter(filePath, true);
        //file.Write(boo);
        //file.Close();
    }

    private void LoadWaves()
    {
        //string filePath = Path.Combine(Application.streamingAssetsPath, "waves.json");
        string filePath = Path.Combine(Application.dataPath, "Resource", "waves.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            waveboo = JsonUtility.FromJson<Wave[]>(json);
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