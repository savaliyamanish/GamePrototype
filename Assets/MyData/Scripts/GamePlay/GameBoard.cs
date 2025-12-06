using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    [Header("Score UI")]
    public TextMeshProUGUI gameLevelLbl;
    public TextMeshProUGUI gameScoreLbl;
    public TextMeshProUGUI gameMatchedLbl;
    public MessagePopup messagePopup;

    [Header("Board ")]
    [SerializeField] private RectTransform gridContainer;
    [SerializeField] private GridLayoutGroup gridLayout;

    [Header("Prefabs")]
    [SerializeField] private BoardItem itemPrefab;
    private GameLevel currentLevel;
    private  List<BoardItem> items = new List<BoardItem>();    
    private bool isBoardBusy = false;
    private int currentGameMatchedCount=0;
    private int currentGameScore=0;
    private int currentScoreComboCount=0;
    void Start()
    {
        ClearGrid();
    }
    public void SetupGrid(GameLevel level)
    {
        isBoardBusy=true;
        ClearGrid();
        
        currentLevel=level;

        int rows = currentLevel.rows;
        int columns = currentLevel.columns;
        int totalTiles = currentLevel.CardCount;
        currentGameScore=0;
        currentGameMatchedCount=0;
        currentScoreComboCount=0;
        
        gameLevelLbl.text=$"Game Level : {level.displayName}";
        UpdateUI();
        ConfigureGridLayout(rows, columns);

        var ids= currentLevel.GetShuffleIdsForItems();
        for (int i = 0; i < totalTiles; i++)
        {
            int row = i / columns;
            int col = i % columns;

            BoardItem item = Instantiate(itemPrefab, gridContainer);
            item.Setup(ids[i],level.itemSprites[ids[i]],()=>OnItemClicked(item));
            items.Add(item);
        }
        StartCoroutine(StartingGame());
    }
    
    IEnumerator StartingGame()
    {
        var waitTime =new WaitForSeconds(.05f);  
        yield return waitTime;
        for (int i = 0; i < items.Count; i++)
        {
            items[i].ShowCard(false);
            yield return waitTime;
        }
        yield return waitTime;
        
        for (int i = 0; i < items.Count; i++)
        {
            items[i].FlipAndOpen();
            yield return waitTime;

        }        
        yield return new WaitForSeconds(currentLevel.gameStartTileShowTime);        
        for (int i = 0; i < items.Count; i++)
        {
            items[i].FlipAndClose();
            yield return waitTime;

        }
        isBoardBusy=false;
    }

    private BoardItem firstSelected = null;
    private BoardItem secondSelected = null;

    public void OnItemClicked(BoardItem item)
    {
        if (isBoardBusy || item.IsMatched || item.IsOpen) return;

        if (firstSelected == null)
        {
            firstSelected = item;
            item.FlipAndOpen();
            return;
        }

        if (secondSelected == null && item != firstSelected)
        {
            secondSelected = item;
            item.FlipAndOpen();
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        isBoardBusy = true;
        yield return new WaitForSeconds(0.4f); 

        if (firstSelected.ItemId == secondSelected.ItemId)
        {
            firstSelected.SetAsMatched();
            secondSelected.SetAsMatched();

            var comboBonus = (currentScoreComboCount*currentLevel.comboBonus);
            currentGameScore += (currentLevel.baseMatchScore + comboBonus);
            
            messagePopup.ShowMessage($"<color=green>+{currentLevel.baseMatchScore}</color>");
            if(comboBonus > 0)
            {               
                messagePopup.ShowMessage($"Combo {currentScoreComboCount}",.2f);
                messagePopup.ShowMessage($"<color=green>+{comboBonus}</color>",.4f);             
            }       
            AudioManager.Instance.PlayMatch();
            currentScoreComboCount++;
            currentGameMatchedCount++;
            UpdateUI();
                    
            firstSelected.HideCard();
            secondSelected.HideCard();
        }
        else
        {
            currentGameScore = Mathf.Max(0,currentGameScore-currentLevel.mismatchPenalty);
            currentScoreComboCount=0;
            messagePopup.ShowMessage($"<color=red>+{currentLevel.mismatchPenalty}</color>");
            UpdateUI();
            AudioManager.Instance.PlayMismatch();
            firstSelected.FlipAndClose();
            secondSelected.FlipAndClose();
        }    
        
        firstSelected = null;
        secondSelected = null;
        isBoardBusy = false;
        if(currentGameMatchedCount >=currentLevel.PairCount)
        {
            yield return new WaitForSeconds(0.5f);        
            GameplayManager.Instance.ShowGameOver(currentLevel.displayName,currentGameScore,currentGameMatchedCount);
        }
    }
    private void ConfigureGridLayout(int rows, int columns)
    {
        Rect rect = gridContainer.rect;

        float width = rect.width - gridLayout.padding.left - gridLayout.padding.right;
        float height = rect.height - gridLayout.padding.top - gridLayout.padding.bottom;

        float cellWidth = (width - gridLayout.spacing.x * (columns - 1)) / columns;
        float cellHeight = (height - gridLayout.spacing.y * (rows - 1)) / rows;

        float cellSize = Mathf.Floor(Mathf.Min(cellWidth, cellHeight));

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;
        gridLayout.cellSize = new Vector2(cellSize, cellSize);
    }

    private void ClearGrid()
    {
        foreach (var t in items)
        {
            if (t != null)
            {
                DestroyImmediate(t.gameObject);
            }
        }
        items.Clear();
    }
    private void UpdateUI()
    {        
        gameScoreLbl.text=$"Score :  {currentGameScore}";
        gameMatchedLbl.text=$"Matched : {currentGameMatchedCount}/{currentLevel.PairCount}";
    }

}
