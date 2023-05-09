using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveData : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;                   // 현재 스테이지의 모든 웨이브 정보
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private TextMeshProUGUI textWaveCount;

    private int currentWaveIndex = 0;

    // 웨이브 정보 출력을 위한 Get 프로퍼티 (현재 웨이브, 총 웨이브)
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
            //현재 웨이브의 정보 에너미 스포너에 넘겨줌
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
    public float spawnDelay;     // 현재 웨이브 적 생성 주기
    public int maxEnemyCount; // 현재 웨이브 적 등장 숫자
    public GameObject[] enemyPrefabs;  // 현재 웨이브 적 등장 종류
}