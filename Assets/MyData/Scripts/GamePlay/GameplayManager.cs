using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{

    public List<GameLevel> gameLevels;
    public GameBoard gameBoard;
    
    [Header("Main Menu")]
    public LeanPopup mainMenuPopup;
    public TMP_Dropdown MP_levelDropDown;
    public Toggle MP_SoundToggle;

    [Header("Continue Game")]
    public LeanPopup continueGamePopup;
    public TextMeshProUGUI CGP_gameLevelLbl;
    public TextMeshProUGUI CGP_gameScoreLbl;
    public TextMeshProUGUI CGP_gameMatchedLbl;
    
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
        Setup();
    }
    private void Setup()
    {
        MP_levelDropDown.options.Clear();
        for (int i = 0; i < gameLevels.Count; i++)
        {
            MP_levelDropDown.options.Add(new TMP_Dropdown.OptionData(){text=gameLevels[i].displayName});
        }
        MP_levelDropDown.value=SaveSystem.LastSelectedLevelDropdown;
        MP_levelDropDown.onValueChanged.AddListener(x=>SaveSystem.LastSelectedLevelDropdown=x);

        MP_SoundToggle.isOn = AudioManager.Instance.GetSoundSetting();
        MP_SoundToggle.onValueChanged.AddListener(AudioManager.Instance.SetSoundSetting);

        MP_levelDropDown.RefreshShownValue();
        gameoverPopup.SetToClosePos();
        continueGamePopup.SetToClosePos();
        mainMenuPopup.SetToOpenPos();
        CheckLastContinueGame();
             
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
        gameBoard.ClearBoard();
        gameoverPopup.Close();
        continueGamePopup.Close();
        mainMenuPopup.Open();        
    }
    public void OnPlayBtn()
    {        
        mainMenuPopup.Close();
        gameBoard.SetupGameBoard(gameLevels[MP_levelDropDown.value]);
    }
    GameLevel lastSaveLevel;
    GameSaveData lastSaveData;
    private void CheckLastContinueGame()
    {
        if(SaveSystem.HasSaveGameData())
        {
            lastSaveData = SaveSystem.LoadGameData();
            lastSaveLevel = gameLevels.Find(x=>lastSaveData.levelId.Equals(x.levelId));
            if(lastSaveLevel !=null)
            {               
                
                ShowContinueGamePopup(lastSaveLevel, lastSaveData);
            }   
        }
    }
    public void ShowContinueGamePopup(GameLevel level,GameSaveData gameSaveData)
    {
        SaveSystem.ClearSaveGameData();
        CGP_gameLevelLbl.text = $"Game Level : {level.displayName}";
        CGP_gameScoreLbl.text = $"Game Score : {lastSaveData.currentScore}";
        CGP_gameMatchedLbl.text = $"Matched : {lastSaveData.matchedCount} / {level.PairCount}";
        continueGamePopup.Open();
    }
    public void StartLastSaveGame()
    {
        mainMenuPopup.Close();
        continueGamePopup.Close();
        if(lastSaveLevel != null && lastSaveData != null)
        gameBoard.LoadGameBoardFromSave(lastSaveLevel, lastSaveData);
    }
    
}
