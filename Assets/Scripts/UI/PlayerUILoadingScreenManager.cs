using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerUILoadingScreenManager : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] CanvasGroup canvasGroup;
    private Coroutine fadeLoadingScreenCoroutine;

    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(Scene arg0, Scene arg1)
    {
        DeactivateLoadingScreen();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    public void ActivateLoadingScreen()
    {
        if (loadingScreen.activeSelf)
            return;

        canvasGroup.alpha = 1;
        loadingScreen.SetActive(true);
    }

    public void DeactivateLoadingScreen(float delay = 1)
    {
        if (!loadingScreen.activeSelf)
            return;

        if (fadeLoadingScreenCoroutine != null)
            return;

        fadeLoadingScreenCoroutine = StartCoroutine(FadeLoadingScreen(1, delay));

    }

    private IEnumerator FadeLoadingScreen(float duration, float delay)
    {
        while (WorldAIManager.instance.isPerformingLoadingOperation)
        {
            yield return null;
        }

        loadingScreen.SetActive(true);

        if (duration > 0)
        {
            while (delay > 0)
            {
                delay -= Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
                yield return null;
            }
        }

        canvasGroup.alpha = 0;
        loadingScreen.SetActive(false);
        fadeLoadingScreenCoroutine = null;
        yield return null;
    }
}
