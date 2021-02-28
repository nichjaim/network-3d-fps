using Nichjaim.MasterSubMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLoadMenuController : SubMenuController
{
    #region Class Variables

    [Header("Sub-Menu Specialist Properties")]

    [SerializeField]
    private GameObject loadSlotScrollContentHolder = null;

    [SerializeField]
    private GameObject loadSlotPrefab = null;

    // list of created slot objects for clean up later
    private List<GameObject> createdLoadSlots = new List<GameObject>();

    #endregion




    #region Override Functions

    public override void OnMenuOpen()
    {
        base.OnMenuOpen();

        // setup the menu's load slots
        RefreshLoadSlots();
    }

    #endregion




    #region Load Menu Functions

    /// <summary>
    /// Sets up the menu's load slots.
    /// </summary>
    private void RefreshLoadSlots()
    {
        // destroy all the created load slots and resets reference list
        DestroyAllCreatedLoadSlots();

        // load all save games
        List<GameSaveData> allSaves = SaveLoadSystem.LoadAll();

        // initialize these vars for upcoming loop
        GameObject spawnedLoadSlotObj;
        LoadSlotController spawnedLoadSlot;

        // loop through all loaded save games
        for (int i = 0; i < allSaves.Count; i++)
        {
            // create a load slot and parent it to the scroll content holder
            spawnedLoadSlotObj = Instantiate(loadSlotPrefab, loadSlotScrollContentHolder.transform);
            // add the created load slot to list of created slots for later clean up
            createdLoadSlots.Add(spawnedLoadSlotObj);

            // get load slot component from created object
            spawnedLoadSlot = spawnedLoadSlotObj.GetComponent<LoadSlotController>();
            // if component found
            if (spawnedLoadSlot != null)
            {
                // perfrom setup on created load slot
                spawnedLoadSlot.SetupLoadSlot(allSaves[i], i + 1);
            }
            // else component NOT found
            else
            {
                // print warning to log
                Debug.LogWarning("Created load slot object did NOT have a LoadSlotController component!");
            }
        }
    }

    /// <summary>
    /// Destroys all the created load slots and resets reference list.
    /// </summary>
    private void DestroyAllCreatedLoadSlots()
    {
        // loop through all created load slots
        foreach (GameObject iterSlot in createdLoadSlots)
        {
            // destroy the iterating load slot object
            Destroy(iterSlot);
        }

        // reset load slot list to fresh empty list
        createdLoadSlots = new List<GameObject>();
    }

    #endregion




    #region Button Press Functions

    public void ButtonPressBack()
    {
        // switch to main menu's play menu
        ((MainMenuController)menuMaster).SwitchToPlayMenu();
    }

    #endregion


}
