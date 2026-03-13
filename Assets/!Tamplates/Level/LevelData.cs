using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "SGames/LevelData")]
public class LevelData : ScriptableObject
{
    public string ID;
    public int ScoreTarget = 100;
    public int Timer = 100;
    public GameObject LevelPrefab;
    public Sprite Background;

    [Space]
    public float DistFinalKof = 1f;
    public float StartWeghitCap = 1f;
    public float StartWeghitAllLineKof = 1f;
   

    public static LevelData LoadFromResources (int level)
    {
        return Resources.Load<LevelData>($"Levels/LevelData{level}");
    }
}