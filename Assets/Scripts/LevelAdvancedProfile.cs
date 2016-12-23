using UnityEngine;
using System.Collections;

// NOT USED
// A prototype for a different kind of level where every spawn is pre-defined
public enum SpawnOption { METEOR, METEOR_FAST, METEOR_STRONG, BONUS_SMALL, BONUS_LARGE }

[System.Serializable]
public struct LevelStep
{
    public SpawnOption thingToSpawn;
    public float thenWaitForSeconds;
    public SpawnOption leaveBonusIfKilled;
}
//[CreateAssetMenu(fileName = "Game Level Advanced", menuName = "Defendor/Level Advanced", order = 2)]
public class LevelAdvancedProfile : ScriptableObject {
    public LevelStep[] steps;
//    public float[] a;
}
