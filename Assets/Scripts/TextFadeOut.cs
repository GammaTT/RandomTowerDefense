using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

public class TextFadeOut : MonoBehaviour
{
    public float fadeDuration = 1.0f; // ���̵� �ƿ��� �ɸ��� �ð�

    [SerializeField]
    private TextMeshProUGUI waveText;

    public void ShowWaveText(int waveCount)
    {
        gameObject.SetActive(true);
        waveText.text = waveCount.ToString() + " Wave";
        StartCoroutine("FadeOutCoroutine");
    }
    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        Color originalColor = waveText.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); // ���� �� ����
            waveText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
