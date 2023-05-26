using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

public class TextFadeOut : MonoBehaviour
{
    public float fadeDuration = 1.0f; // ���̵� �ƿ��� �ɸ��� �ð�

    [SerializeField]
    private TextMeshProUGUI showText;

    public void ShowText(string text, float duration)
    {
        gameObject.SetActive(true);
        showText.text = text;
        fadeDuration = duration;
        StartCoroutine("FadeOutCoroutine");
    }
    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        Color originalColor = showText.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); // ���� �� ����
            showText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
