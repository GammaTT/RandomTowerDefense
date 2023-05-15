using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private WaveData waveData;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private TextMeshProUGUI textWaveCount;
    [SerializeField]
    private TextMeshProUGUI textCurrentScore;

    private int currentWaveIndex = 0;

    public ScoreSystem scoreSystem;
    // ���̺� ���� ����� ���� Get ������Ƽ (���� ���̺�, �� ���̺�)
    public int MaxWave => waveData.waves.Length;

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
        if (currentWaveIndex < waveData.waves.Length)
        {
            StartGame();
            //���� ���̺��� ���� ���ʹ� �����ʿ� �Ѱ���
            enemySpawner.StartWave(waveData.waves[currentWaveIndex]);
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
    
}
