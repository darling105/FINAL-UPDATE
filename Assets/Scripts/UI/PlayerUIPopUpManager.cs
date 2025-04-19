using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIPopUpManager : MonoBehaviour
{

    [Header("Pop Up Message")]
    [SerializeField] TextMeshProUGUI popUpMessageText;
    [SerializeField] GameObject popUpMessageGameObject;

    [Header("Item Pop Up")]
    [SerializeField] GameObject itemPopUpGameObject;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemAmount;

    [Header("Energy Site")]
    [SerializeField] GameObject energySitePopUpGameObject;
    [SerializeField] TextMeshProUGUI energySitePopUpBackgroundText;
    [SerializeField] TextMeshProUGUI energySitePopUpText;
    [SerializeField] CanvasGroup energySitePopUpCanvasGroup;

    [Header("You Died")]
    [SerializeField] GameObject youDiedPopUpGameObject;
    [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI youDiedPopUpText;
    [SerializeField] CanvasGroup youDiedPopUpCanvasGroup;

    [Header("You Died")]
    [SerializeField] GameObject bossDefeatedPopUpGameObject;
    [SerializeField] TextMeshProUGUI bossDefeatedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI bossDefeatedPopUpText;
    [SerializeField] CanvasGroup bossDefeatedPopUpCanvasGroup;

    public void CloseAllPopUpWindows()
    {
        popUpMessageGameObject.SetActive(false);
        itemPopUpGameObject.SetActive(false);

        PlayerUIManager.instance.popUpWindowIsOpen = false;
    }

    public void SendPlayerMessagePopUp(string messageText)
    {
        PlayerUIManager.instance.popUpWindowIsOpen = true;
        popUpMessageText.text = messageText;
        popUpMessageGameObject.SetActive(true);
    }

    public void SendItemPopUp(Item item, int amount)
    {
        itemAmount.enabled = false;
        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;

        if (amount > 0)
        {
            itemAmount.enabled = true;
            itemAmount.text = "x" + amount.ToString();
        }

        itemPopUpGameObject.SetActive(true);
        PlayerUIManager.instance.popUpWindowIsOpen = true;
    }

    public void SendYouDiedPopUP()
    {
        youDiedPopUpGameObject.SetActive(true);
        youDiedPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 2, 8));
        StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadeOutOverTime(youDiedPopUpCanvasGroup, 2, 4));
    }

    public void SendBossDefeatedPopUP(string bossDefeatedMessage)
    {
        bossDefeatedPopUpText.text = bossDefeatedMessage;
        bossDefeatedPopUpBackgroundText.text = bossDefeatedMessage;
        bossDefeatedPopUpGameObject.SetActive(true);
        bossDefeatedPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(bossDefeatedPopUpBackgroundText, 2, 8));
        StartCoroutine(FadeInPopUpOverTime(bossDefeatedPopUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadeOutOverTime(bossDefeatedPopUpCanvasGroup, 2, 4));
    }

    public void SendEnergySiteRestorPopUp(string energySiteMessage)
    {
        energySitePopUpText.text = energySiteMessage;
        energySitePopUpBackgroundText.text = energySiteMessage;
        energySitePopUpGameObject.SetActive(true);
        energySitePopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(energySitePopUpBackgroundText, 2, 8));
        StartCoroutine(FadeInPopUpOverTime(energySitePopUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadeOutOverTime(energySitePopUpCanvasGroup, 2, 4));
    }

    private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
    {
        if (duration > 0f)
        {
            text.characterSpacing = 0;
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                yield return null;
            }
        }
    }

    private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
    {
        if (duration > 0)
        {
            canvas.alpha = 0;
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                yield return null;
            }

        }

        canvas.alpha = 1;

        yield return null;
    }

    private IEnumerator WaitThenFadeOutOverTime(CanvasGroup canvas, float duration, float delay)
    {
        if (duration > 0)
        {

            while (delay > 0)
            {
                delay -= Time.deltaTime;
                yield return null;
            }
            canvas.alpha = 1;
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                yield return null;
            }

        }

        canvas.alpha = 0;

        yield return null;
    }

}
