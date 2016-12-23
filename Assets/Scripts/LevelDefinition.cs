using UnityEngine;

[CreateAssetMenu(fileName = "Game Level", menuName = "Moon Defense/Level", order = 1)]
public class LevelDefinition : ScriptableObject
{
    //  string name;
    public SpawnOddsDefinition[] odds = {
        new SpawnOddsDefinition("METEOR_REGULAR",1),
        new SpawnOddsDefinition("METEOR_FAST",1),
        new SpawnOddsDefinition("METEOR_HEAVY",1)
    };
    
    public float spawnEnemyEvery = 1f;
    public int oddsOfBonusASpawnOnEnemyDeath = 1;
    public int oddsOfNothingOnEnemyDeath = 9;
    
    public float randomlyMissTargetByUpTo = 10f;
    public float graceTimeBeforeFirstSpawn = 3;
    public float levelDuration = 120;
}
