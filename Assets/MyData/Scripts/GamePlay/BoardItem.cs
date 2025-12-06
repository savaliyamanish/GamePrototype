using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoardItem : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup itemContent; 
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private Button button;

    [SerializeField] private int _itemId = -1;
    public int ItemId => _itemId;

    [SerializeField] private bool _isOpen = false;
    [SerializeField] private bool _isMatched = false;
    public bool IsMatched
    {
        get
        {
            return _isMatched;
        }
        private set
        {
            _isMatched=value;
        }
    }
    public bool IsOpen
    {
        get
        {            
            return _isOpen;
        }
        private set
        {
            _isOpen=value;
            itemImage.enabled=_isOpen;
        }
    }

    public void Setup(int itemId, Sprite itemSprite,UnityAction onClick)
    {
        _itemId = itemId;
        itemImage.sprite = itemSprite;

        IsMatched = false;
        IsOpen = false;
        itemContent.alpha = 0f;
        itemContent.transform.localScale= Vector3.one;
        button.onClick.AddListener(onClick);
    }
    public void SetAsMatched()
    {
        IsMatched=true;
    }
    public void ShowCard(bool open,float animTime=0.25f)
    {
        IsOpen=open;
        itemContent.LeanAlpha(1f,animTime).setEaseInOutQuad();;
    }
    public void HideCard(float animTime=0.25f)
    {
        itemContent.LeanAlpha(0f,animTime).setEaseInOutQuad();;
    }
    public void FlipAndClose(float animTime=0.25f)
    {
       FlipCardAnimation(
            () =>
            {
                IsOpen=false;
            }
        ,animTime);
    }
    public void FlipAndOpen(float animTime=0.25f)
    {
        FlipCardAnimation(
            () =>
            {
                IsOpen=true;
            }
        ,animTime);
    }
    private void FlipCardAnimation(UnityAction atFlipAction,float animTime)
    {
         itemContent.transform.LeanScaleX(0,animTime*.5f).setOnComplete(
            () =>
            {
                atFlipAction?.Invoke();
                itemContent.transform.LeanScaleX(1,animTime*.5f).setEaseInOutQuad();
            }
        ).setEaseInOutQuad();
    }
}
