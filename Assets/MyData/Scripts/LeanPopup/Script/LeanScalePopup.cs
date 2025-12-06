using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LeanScalePopup : LeanPopup
{
    public Vector3 openSize;
    public Vector3 closeSize;
    public override LTDescr OpenTween()
    {
        LeanTween.cancel(gameObject);      
        LeanPopupManager.intance.AddToOpenPopup(this);
        return LeanTween.scale(gameObject.GetComponent<RectTransform>(),openSize,animTime).setEase(curveType).setOnComplete(()=>{
            isOpen=true;
            onOpen.Invoke();
        }).setIgnoreTimeScale(ignoreTimeScale);
      
    }
    public override LTDescr CloseTween()
    {
        LeanTween.cancel(gameObject);
        LeanPopupManager.intance.RemoveToOpenPopup(this);
        return LeanTween.scale(gameObject.GetComponent<RectTransform>(),closeSize,animTime).setEase(curveType).setOnComplete(()=>{
            isOpen=false;
            onClose.Invoke();
        }).setIgnoreTimeScale(ignoreTimeScale);
    }
    
    public override void SetToOpenPos()
    {
        isOpen=true;
        gameObject.transform.localScale=openSize;       
    }
    public override void SetToClosePos()
    {     
        isOpen=false;
        gameObject.transform.localScale=closeSize;           
    }
}
