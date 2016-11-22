using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// Called by StoreBuyButtons. Used to manage the behavior of store and link the items to the State Keeper (which persists the upgrades to many levels)
public class StoreManager : MonoBehaviour
{
    public Text bankDisplay;
    public StoreBuyButton[] allButtons;
    public AudioSource buySound;
   
    void Start()
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        bankDisplay.text = "BANK: " + DefendorGameStateKeeper.keeper.cashInBank.ToString();
    }
    
    /* Called by StoreBuyButtons in the scene*/
    public void RequestPurchaseItem()
    {
        // Get a reference to the button calling this function
        StoreBuyButton button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<StoreBuyButton>();
        // Call the real function
        RequestPurchaseItem(button.upgradeProfile, button.price);
    }

    public void RequestPurchaseItem(GenericUpgradeProfile profile, float price)
    {
        DefendorGameStateKeeper keeper = DefendorGameStateKeeper.keeper;
        // Add the item to the upgrades acquired list in the keeper
        keeper.upgradesAcquired.Add(profile);

        // Subtract the price from cash on hand
        keeper.cashInBank -= price;

        // Refresh the bank display and the text on the buttons
        UpdateDisplay();
        foreach (StoreBuyButton button in allButtons) 
        {
            button.Refresh();
        }

        // Play the buy sound
        buySound.Play();
    }
}
