using TMPro;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    public GameLevel currentLevel;
    public GameBoard gameBoard;
    
    [Header("Game Over UI")]
    public LeanPopup gameoverPopup;
    public TextMeshProUGUI GOP_gameLevelLbl;
    public TextMeshProUGUI GOP_gameScoreLbl;
    public TextMeshProUGUI GOP_gameMatchedLbl;

    public static GameplayManager Instance;
    
    private void Awake()
    {
        if(Instance==null)
            Instance=this;
    }
    private void Start()
    {
        gameoverPopup.SetToClosePos();
        gameBoard.SetupGrid(currentLevel);
    }
    public void ShowGameOver(string levelName,int score,int matched)
    {
        GOP_gameLevelLbl.text = $"Game Level : {levelName}";
        GOP_gameScoreLbl.text = $"Game Score : {score}";
        GOP_gameMatchedLbl.text = $"Matched : {matched}";
        gameoverPopup.Open();
        AudioManager.Instance.PlayGameOver();
    }
    public void ReloadToMainManu()
    {
        gameoverPopup.Close();
        gameBoard.SetupGrid(currentLevel);
        
    }
}
