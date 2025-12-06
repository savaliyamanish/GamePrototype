using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    public GameLevel currentLevel;
    public GameBoard gameBoard;
    [ContextMenu("Run")]
    private void Start()
    {
        gameBoard.SetupGrid(currentLevel);
    }
}
