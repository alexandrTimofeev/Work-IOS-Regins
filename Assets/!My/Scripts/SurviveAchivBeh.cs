using UnityEngine;

public class SurviveAchivBeh : AchievementBehaviour
{
    public string SurvivePrefsName;
    public int TargetCount = 10;

    public override bool IsConditionSuccess => IsUnlocked || PlayerPrefs.GetInt(SurvivePrefsName) >= TargetCount;
}
