using System;
[Serializable()]
public class LevelStatsSnapshot
{
    public int enemiesDestroyed = 0;
    public int enemiesMissed = 0;
    public float scoreThisLevel = 0;
    public float bonusBankedThisRound = 0;
    public int citiesRemain = 0;
    public int comboLevel = 1;
    public int maxComboThisLevel;

    public void Apply(DefendorGameStateKeeper keeper)
    {
        keeper.lastLevelScore = scoreThisLevel;
        keeper.lastLevelKilledPercent = enemiesDestroyed == 0 ? 0 : (float)enemiesDestroyed / ((float)enemiesDestroyed + (float)enemiesMissed);
        keeper.lastLevelMaxCombo = maxComboThisLevel;
        keeper.lastLevelCitiesRemain = citiesRemain;
        keeper.lastLevelBonusGot = bonusBankedThisRound;
    }

}

