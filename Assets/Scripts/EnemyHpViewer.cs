using UnityEngine;
using UnityEngine.UI;

public class EnemyHpViewer : MonoBehaviour
{
    private EnemyHp enemyHp;
    private Slider hpSlider;

    public void Setup(EnemyHp enemyHp)
    {
        this.enemyHp = enemyHp;
        hpSlider = GetComponent<Slider>();
        hpSliderUpdate();
    }

    public void hpSliderUpdate()
    {
        hpSlider.value = enemyHp.currentHp / enemyHp.maxHp;
    }
    private void Update()
    {
        hpSliderUpdate();
    }
}


/*
 * File : EnemyHPView.cs
 * Desc
 *	: 적의 체력 정보를 Slider UI에 업데이트
 *	
 */