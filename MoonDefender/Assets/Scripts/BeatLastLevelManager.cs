using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* This script runs on the scene that plays when you win a level. It tallies up information from the previous level using the global
Statekeeper object, and can be modified to provide more detailed analytics. */

public class BeatLastLevelManager : MonoBehaviour {

    // Buttons in the scene to disable if necessary
    public Button[] buttons;

    // Somewhere to output a message
    public Text outputDisplay;
	// Use this for initialization
	void Start () {
        DefendorGameStateKeeper keeper = DefendorGameStateKeeper.keeper;
        // Figure out how much coins the player should get.
        float bonus = CalculateBonus();
        // Add the coins to the permanent record of cash
        keeper.cashInBank += bonus;

        // Output information based on the last level
        outputDisplay.text = keeper.lastLevelScore.ToString() + "\n"
            + (keeper.lastLevelKilledPercent * 100).ToString("00") + "\n"
            + keeper.lastLevelCitiesRemain.ToString() + "\n"
            + keeper.lastLevelMaxCombo.ToString() + "\n"
            + bonus.ToString() + "\n"
            + keeper.cashInBank.ToString() + "\n";

        // If the last stage wasn't the last one...
        if (keeper.lastSelectedStage < 7)
        {
            // Unlock the next level in the state keeper
            keeper.levelsUnlockedOrdered[keeper.lastSelectedStage + 1] = true;
        } else
        {
            // Otherwise...
            // If its the first time you have beaten the last level...
            if (!keeper.gameHasBeenBeaten)
            {
                // Disable the usual buttons and take the player to the YOU WIN scene.
                keeper.gameHasBeenBeaten = true;
                Debug.Log("You have won the game.");
                foreach (Button b in buttons)
                {
                    b.interactable = false;
                }
                GetComponent<LoadNextScene>().LoadLevelAfterDelay("BeatTheGame", 5);
            }
        }
        
    }

    float CalculateBonus()
    {
        // Calculate the bonus score based on custom algorithm.
        // The keeper variable contains information about the previous level.
        DefendorGameStateKeeper keeper = DefendorGameStateKeeper.keeper;
        float bonus = (keeper.lastSelectedStage + 1) * 1000
            + Mathf.Floor(keeper.lastLevelKilledPercent * 100 / 20) * 1000
            + keeper.lastLevelCitiesRemain * 1000
            + keeper.lastLevelBonusGot;
        return bonus;
    }
	
}
