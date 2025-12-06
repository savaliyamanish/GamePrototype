using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeanPopup : MonoBehaviour
{
    public bool isOpen;
    public bool closeOnEsc = false;
    public bool ignoreTimeScale = true;
    [Range(.01f, 5f)]
    public float animTime = .3f;
    public LeanTweenType curveType;
    public UnityEvent onClose, onOpen;
    public AudioClip OpenPopupClip, ClosePopupClip;

    public virtual bool isPlaying
    {
        get
        {
            return LeanTween.isTweening(gameObject);
        }
    }
    void OnEnable()
    {
        LeanPopupManager.intance.RegisterPopup(this);
    }
    void Start()
    {
        if (isOpen)
        {
            SetToOpenPos();
        }
        else
        {
            SetToClosePos();
        }
    }
    public void Open()
    {
        OpenTween();

    }
    public void Close()
    {
        CloseTween();
    }
    public virtual LTDescr OpenTween()
    {
        LeanTween.cancel(gameObject);
        LeanPopupManager.intance.AddToOpenPopup(this);
        return LeanTween.value(gameObject, 0, 1, animTime).setEase(curveType).setOnComplete(() =>
        {
            isOpen = true;
            onOpen.Invoke();

        }).setIgnoreTimeScale(ignoreTimeScale);

    }
    public virtual LTDescr CloseTween()
    {
        LeanTween.cancel(gameObject);
        LeanPopupManager.intance.RemoveToOpenPopup(this);
        return LeanTween.value(gameObject, 1, 2, animTime).setEase(curveType).setOnComplete(() =>
        {
            isOpen = false;
            onClose.Invoke();

        }).setIgnoreTimeScale(ignoreTimeScale);
    }
    public virtual void SetToOpenPos()
    {
        isOpen = true;
    }
    public virtual void SetToClosePos()
    {
        isOpen = false;
    }
}
#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(LeanPopup), true)]
public class LeanPopupEditor : UnityEditor.Editor
{

    public override void OnInspectorGUI()
    {
        LeanPopup targetPopup = (LeanPopup)target;
        UnityEditor.EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Open"))
        {
            if (UnityEditor.EditorApplication.isPlaying)
                targetPopup.Open();
            else
            {
                List<Object> all = new List<Object>();
                foreach (var item in targetPopup.GetComponents<Component>())
                {
                    all.Add(item);
                }
                UnityEditor.Undo.RecordObjects(all.ToArray(), targetPopup.name + " Popup Open");
                targetPopup.SetToOpenPos();
            }
        }
        if (GUILayout.Button("Close"))
        {
            if (UnityEditor.EditorApplication.isPlaying)
                targetPopup.Close();
            else
            {
                List<Object> all = new List<Object>();
                foreach (var item in targetPopup.GetComponents<Component>())
                {
                    all.Add(item);
                }
                UnityEditor.Undo.RecordObjects(all.ToArray(), targetPopup.name + " Popup Close");
                targetPopup.SetToClosePos();
            }
        }
        UnityEditor.EditorGUILayout.EndHorizontal();
        DrawDefaultInspector();

    }
}
#endif