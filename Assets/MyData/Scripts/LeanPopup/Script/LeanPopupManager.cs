using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanPopupManager : MonoBehaviour
{
    public static LeanPopupManager intance
    {
        get{
            if(_intance==null)
            {
                GameObject temp=new GameObject();
                temp.name="LeanPopupManager";   
                _intance=temp.AddComponent<LeanPopupManager>();
            }
            return _intance;
        }
    }
    static LeanPopupManager _intance=null;
    public List<LeanPopup> allPopupList;
    public List<LeanPopup> openPopupList;
    private void OnEnable() {
        if(_intance==null)
        {
            _intance=this;
            allPopupList=new List<LeanPopup>();
            openPopupList = new List<LeanPopup>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
    public void RegisterPopup(LeanPopup popup)
    {        
        if(!allPopupList.Contains(popup))
            allPopupList.Add(popup);
        allPopupList.RemoveAll(x=>x==null);
    }
    public void AddToOpenPopup(LeanPopup popup)
    {        
        allPopupList.RemoveAll(x=>x==null);
        openPopupList.RemoveAll(x=>x==null);
        openPopupList.Add(popup);
    }
    public void RemoveToOpenPopup(LeanPopup popup)
    {        
        allPopupList.RemoveAll(x=>x==null);
        openPopupList.RemoveAll(x=>x==null);
        openPopupList.Remove(popup);
    }
    void Update() 
    {
        if(Input.GetKey(KeyCode.Escape))  
        {
            LeanPopup temp=CurrentCloseOnEscPopup();
            if(temp !=null)
            {
                temp.Close();
            }
        }
    }
    public LeanPopup CurrentCloseOnEscPopup()
    {
        openPopupList.RemoveAll(x=>x==null);
        if(openPopupList.Count > 0 && openPopupList[openPopupList.Count-1].isOpen &&
        openPopupList[openPopupList.Count-1].closeOnEsc && !openPopupList[openPopupList.Count-1].isPlaying)
        {
            return openPopupList[openPopupList.Count-1];
        }
        return null;
    }
}
