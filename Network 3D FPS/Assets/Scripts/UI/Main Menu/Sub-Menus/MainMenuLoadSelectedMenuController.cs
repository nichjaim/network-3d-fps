using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Tools;
using Nichjaim.MasterSubMenu;

public class MainMenuLoadSelectedMenuController : SubMenuController, 
    MMEventListener<LoadSlotPressedEvent>
{
    #region Class Variables

    private GameSaveData slotSave = null;
    private int saveFileNumber = 0;

    [Header("Sub-Menu Specialist Properties")]

    [SerializeField]
    private TextMeshProUGUI saveFileText = null;

    #endregion




    #region MonoBehaviour Functions

    private void OnEnable()
    {
        // starts listening for all relevant events
        StartAllEventListening();
    }

    private void OnDisable()
    {
        // stops listening for all relevant events
        StopAllEventListening();
    }

    #endregion




    #region Override Functions

    public override void OnMenuOpen()
    {
        base.OnMenuOpen();

        // setup the menu info
        RefreshMenu();
    }

    #endregion




    #region Load Selected Menu Functions

    /// <summary>
    /// Sets up the menu info.
    /// </summary>
    private void RefreshMenu()
    {
        saveFileText.text = "save-" + saveFileNumber;
    }

    #endregion




    #region Button Press Functions

    public void ButtonPressLoad()
    {
        Debug.Log("Load Button Needs Impl!"); // NEEDS IMPL
    }

    public void ButtonPressDelete()
    {
        // switch to main menu's load menu
        ((MainMenuController)menuMaster).SwitchToLoadDeleteMenu();
    }

    public void ButtonPressBack()
    {
        // switch to main menu's load menu
        ((MainMenuController)menuMaster).SwitchToLoadMenu();
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<LoadSlotPressedEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<LoadSlotPressedEvent>();
    }

    public void OnMMEvent(LoadSlotPressedEvent eventType)
    {
        // set save file properties
        slotSave = eventType.saveData;
        saveFileNumber = eventType.saveFileNumber;

        // switch to this sub menu
        ((MainMenuController)menuMaster).SwitchToLoadSelectedMenu();
    }

    #endregion




    #region Getter Functions

    public int GetSaveFileNumber()
    {
        return saveFileNumber;
    }

    #endregion


}
