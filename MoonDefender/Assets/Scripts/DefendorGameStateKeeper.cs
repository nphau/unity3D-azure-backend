using System.Collections.Generic;
using UnityEngine;
// Class which persists from scene to scene and keeps track of 
// 1 - powerups that the player has collected
// 2 - levels which are unlocked
// 3 - some information about the last level the player played

public class DefendorGameStateKeeper : MonoBehaviour
{
    private static bool created = false;
    public static DefendorGameStateKeeper keeper;

    public float cashInBank;
    public float lastLevelScore;
    public float lastLevelKilledPercent;
    public float lastLevelBonusGot;
    public int lastLevelCitiesRemain;
    public int lastLevelMaxCombo;
    public bool gameHasBeenBeaten;

    // Set the state keeper to persist between scene loadings
    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this);
            created = true;
            keeper = this;
        } else
        {
            // If there already is a statekeeper, delete this one
            Destroy(this.gameObject);
        }
        
    }
    public int lastSelectedStage = 0;
    
    // Array of booleans representing levels the player has unlocked. so if [0] is true, level 1 is unlocked, and so forth.
    public bool[] levelsUnlockedOrdered = new bool[8];

    public List<GenericUpgradeProfile> upgradesAcquired = new List<GenericUpgradeProfile>();
}
