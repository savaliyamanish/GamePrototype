using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class LeanCanvasGroupPopup : LeanPopup
{
    public float openAlpha = 1;
    public float closeAlpha = 0;
    CanvasGroup canvasGroup
    {
        get
        {
            return gameObject.GetComponent<CanvasGroup>();
        }
    }
    public override LTDescr OpenTween()
    {
        LeanTween.cancel(gameObject);
        canvasGroup.blocksRaycasts = true;
        LeanPopupManager.intance.AddToOpenPopup(this);
        return LeanTween.alphaCanvas(canvasGroup, openAlpha, animTime).setEase(curveType).setOnComplete(() =>
        {
            isOpen = true;
            canvasGroup.interactable = true;
            onOpen.Invoke();
        }).setIgnoreTimeScale(ignoreTimeScale);

    }
    public override LTDescr CloseTween()
    {
        LeanTween.cancel(gameObject);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        LeanPopupManager.intance.RemoveToOpenPopup(this);
        return LeanTween.alphaCanvas(canvasGroup, closeAlpha, animTime).setEase(curveType).setOnComplete(() =>
        {
            isOpen = false;
            onClose.Invoke();
        }).setIgnoreTimeScale(ignoreTimeScale);
    }

    public override void SetToOpenPos()
    {
        isOpen = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        canvasGroup.alpha = 1;
    }
    public override void SetToClosePos()
    {
        isOpen = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0;
    }
}
