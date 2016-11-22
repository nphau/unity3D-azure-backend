using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoreBuyButton : MonoBehaviour {

    public float price;
    public Text priceText;
    public GenericUpgradeProfile upgradeProfile;
	// Use this for initialization
	void Start () {
        Refresh();
	}
    public void Refresh()
    {
        bool isSoldOut = false;
        DefendorGameStateKeeper keeper = DefendorGameStateKeeper.keeper;

        foreach (GenericUpgradeProfile profile in keeper.upgradesAcquired)
        {
            if (profile.slug == upgradeProfile.slug)
            {
                GetComponent<Button>().interactable = false;
                priceText.text = "SOLD OUT";
                isSoldOut = true;
            }
        }

        if (keeper.cashInBank < price)
        {
            GetComponent<Button>().interactable = false;
        } else if (!isSoldOut)
        {
            GetComponent<Button>().interactable = true;
        }
    }
}
