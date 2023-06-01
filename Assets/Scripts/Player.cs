using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    bool gameover = false;
    public int gold;
    public int maxHp;
    private int _currentHp;
    private SceneDirector sceneDirector;
    public int currentHp
    {
        get { return _currentHp; }
        private set { _currentHp = Mathf.Max(0, value); }
    }

    private int wallCount;
    private int towerCount;

    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private WaveSystem waveSystem;
    [SerializeField]
    private Image hitRedImage;

    private void Awake()
    {
        currentHp = maxHp;

    }
    // Start is called before the first frame update
    void Start()
    {
        sceneDirector = GetComponent<SceneDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        StopCoroutine("HitAnimation");
        StartCoroutine("HitAnimation");

        if (currentHp <= 0 && !gameover)
        {
            gameover = true;
            //holy moly wtf game over damn it
            waveSystem.FinishGame();
            Invoke("GameOverScene", 3f);
        }
    }

    public void GameOverScene()
    {
        sceneDirector.OpeningScene();
    }
    private IEnumerator HitAnimation()
    {
        //�� �̹��� UI�� ȭ�� ��ü�� ���� ������ RayCastTarget�� ������
        // ��üȭ�� ũ��� ��ġ�� imageScreen�� ������ color ������ ����
        // imageScreen�� ������ 40%�� ����
        Color color = hitRedImage.color;
        color.a = 0.4f;
        hitRedImage.color = color;

        // ������ 0%�� �ɶ����� ����
        while (color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            hitRedImage.color = color;

            yield return null;
        }
    }
}
