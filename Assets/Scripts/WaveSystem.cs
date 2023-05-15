using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;                   // 현재 스테이지의 모든 웨이브 정보
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private TextMeshProUGUI textWaveCount;
    [SerializeField]
    private TextMeshProUGUI textCurrentScore;

    private int currentWaveIndex = 0;

    public ScoreSystem scoreSystem;
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


    private List<int> scoreData = new List<int>();

    private void Start()
    {
        textWaveCount.text = "Wave : " + 1;
        textCurrentScore.text = "Score : " + 0;
        scoreSystem = new ScoreSystem();
        scoreSystem.Setup(textCurrentScore);
        //LoadWaves();
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

    public void FinishGame()
    {
        scoreSystem.AddThisGameScore();
    }
    public void SaveToJson()
    {
        
    }

    private void LoadWaves()
    {
        
    }
}

 
//웨이브 시스템을 인스펙터에서 설정하게 그냥 놔둘까
//제이순 파일로 읽어올까, 스크립터블 오브젝트도 괜찮을거 같다.
[System.Serializable]
public struct Wave
{
    public float spawnDelay;     // 현재 웨이브 적 생성 주기
    public int maxEnemyCount; // 현재 웨이브 적 등장 숫자
    public GameObject[] enemyPrefabs;  // 현재 웨이브 적 등장 종류
}

[CreateAssetMenu]
public class Waves : ScriptableObject
{
    public GameObject[] enemyPrefabs;

    [System.Serializable]
    public struct Wave
    {
        public int spawnDelay;
        public int maxEnemyCount;
        public GameObject[] enemyPrefabs;
    }
}