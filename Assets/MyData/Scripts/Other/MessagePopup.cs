using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessagePopup : MonoBehaviour
{

    [Header("References")]
    public RectTransform startPos;
    public RectTransform endPos;
    public CanvasGroup messagePrefab;

    [Header("Settings")]
    [Range(0.1f, 2.0f)] public float animationTime = 1.0f;
    [Range(0.1f, 2.0f)] public float fadeTime = 0.5f;
    public float endThreshold = 30f;

    private Queue<CanvasGroup> textsPool = new Queue<CanvasGroup>();

    private CanvasGroup GetFromPool()
    {
        if (textsPool.Count > 0)
        {
            CanvasGroup obj = textsPool.Dequeue();
            obj.alpha = 1;
            obj.transform.localScale = Vector3.zero;
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            CanvasGroup newObj = Instantiate(messagePrefab, transform);
            return newObj;
        }
    }

    private void ReturnToPool(CanvasGroup obj)
    {
        obj.gameObject.SetActive(false);
        textsPool.Enqueue(obj);
    }

    public void ShowMessage(string message,float delay=0)
    {
        CanvasGroup Obj = GetFromPool();
        Obj.transform.position = startPos.position;
        Obj.transform.localScale =Vector3.zero;
        Obj.alpha=1;
        TextMeshProUGUI textMesh = Obj.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh != null)
        {
            textMesh.text = message.ToString();
        }

        Vector3 randomOffset = new Vector3(
            Random.Range(-endThreshold, endThreshold),
            Random.Range(-endThreshold, endThreshold),
            0
        );
        Vector3 finalEndPos = endPos.position + randomOffset;

        LeanTween.scale(Obj.gameObject, Vector3.one, animationTime * 0.3f).setEaseOutBack().setDelay(delay);
        LeanTween.move(Obj.gameObject, finalEndPos, animationTime).setEaseOutQuad().setDelay(delay);
        LeanTween.alphaCanvas(Obj, 0, fadeTime).setEaseOutQuad().setDelay(animationTime - fadeTime).setDelay(delay);

        LeanTween.delayedCall(animationTime+delay + 0.1f, () => ReturnToPool(Obj));
    }

}
