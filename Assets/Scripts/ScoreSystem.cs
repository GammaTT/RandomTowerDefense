using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using TMPro;

public class ScoreSystem
{
    private List<int> scoreData = new();
    private int showScoreCount = 8;
    public int currentScore;

    string path = Path.Combine(Application.dataPath, "Resources/Score.json");
    JsonSerializer serializer = new();


    private TextMeshProUGUI textCurrentScore;
    public void Setup(TextMeshProUGUI textCurrentScore)
    {
        this.textCurrentScore = textCurrentScore;
        LoadScore();
    }

    //���� ���ھ� ������ŭ ����
    public void AddScore(int point)
    {
        currentScore += point;
        //textCurrentScore.text = "Score : " + currentScore;
        textCurrentScore.text = currentScore.ToString();
    }

    //��ϵ� ���ھ� �� ���� ���ھ� ����
    public void AddThisGameScore()
    {
        if (!scoreData.Contains(currentScore))
        {
            scoreData.Add(currentScore);
        }
        SaveScore();

    }
    public void SaveScore()
    {
        if (File.Exists(path))
        {
            scoreData.Sort(CompareScoreDescending);

            string jsonScoreData = JsonConvert.SerializeObject(scoreData);

            File.WriteAllText(path, jsonScoreData);

            foreach (int score in scoreData)
            {
                Debug.Log(score);
            }
        }
    }
    public void LoadScore()
    {
        if (File.Exists (path))
        {
            string jsonString = File.ReadAllText(path);
            scoreData = JsonConvert.DeserializeObject<List<int>>(jsonString);
            scoreData.Sort(CompareScoreDescending);
        }

    }

    int CompareScoreDescending(int x, int y)
    {
        return y - x;

        //y - x �� ���������� �ȴ�.
        //�񱳽��� ���ϰ��� 0 ���� Ŭ���� �� ����� ��ġ�� �ٲ۴�.
        //�츮�� �������� (������ �����ʺ��� ũ��) �� �ʿ��ϹǷ� 
        //������ ������ ���� �� Ŭ�� (���� ���� 0 ���� Ŭ��) �� �� ��ġ�� �ٲٰ� �Ѵ�.

    }
}