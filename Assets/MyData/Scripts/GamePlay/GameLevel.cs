using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameLevel", menuName = "MyGame/Game Level", order = 0)]
public class GameLevel : ScriptableObject
{
    [Header("Identification")]
    public string levelId = "level_1";
    public string displayName = "Level 1";

    [Header("Grid Layout")]
    [Min(1)] public int rows = 2;
    [Min(1)] public int columns = 2;

    [Header("Scoring")]
    public int baseMatchScore = 100;   
    public int mismatchPenalty = 10;   
    public int comboBonus = 20;       

    [Header("Card Sprites")]

    public Sprite[] itemSprites;

    [Header("Gameplay")]
    public float gameStartTileShowTime = 3f;

    // Helper properties
    public int CardCount => rows * columns;
    public int PairCount => CardCount / 2;
    public List<int> GetShuffleIdsForItems()
    {
        List<int> ids = new List<int>();
        int totalPairs = PairCount;
        for (int i = 0; i < totalPairs; i++)
        {
            if(itemSprites.Length > i)
            {
                ids.Add(i);
                ids.Add(i);
            }
            else
            {
                int randomId=Random.Range(0,itemSprites.Length-1);
                ids.Add(randomId);
                ids.Add(randomId);
            }
        }
        ids.Shuffle();
        return ids;
    }
}
