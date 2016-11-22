using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageSelectManager : MonoBehaviour {

    public GameObject[] orderedButtons;

    public void Start()
    {

        Color gray = new Color(0.5f, 0.5f, 0.5f);
        Color white = new Color(1f, 1f, 1f);
        DefendorGameStateKeeper keeper = DefendorGameStateKeeper.keeper;
        keeper.levelsUnlockedOrdered[0] = true;

        for (int i = 0; i < orderedButtons.Length; i++)
        {
            if (orderedButtons[i])
            {
                Button b = orderedButtons[i].GetComponent<Button>();
                b.interactable = keeper.levelsUnlockedOrdered[i];
            }
            
        }  
    }
}
