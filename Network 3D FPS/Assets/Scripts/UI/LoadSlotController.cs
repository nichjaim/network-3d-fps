using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadSlotController : MonoBehaviour
{
    #region Class Variables

    private GameSaveData slotSave = null;
    private int saveFileNumber = 0;

    [SerializeField]
    private TextMeshProUGUI textSaveNum = null;

    #endregion




    #region Load Slot Functions

    /// <summary>
    /// Sets up the load slot from the given save data.
    /// </summary>
    /// <param name="gameSaveArg"></param>
    /// <param name="saveFileNumberArg"></param>
    public void SetupLoadSlot(GameSaveData gameSaveArg, int saveFileNumberArg)
    {
        // set save and file number
        slotSave = gameSaveArg;
        saveFileNumber = saveFileNumberArg;

        // set slot text number from save file number
        textSaveNum.text = "Save-" + saveFileNumber;
    }

    #endregion




    #region Button Press Functions

    public void ButtonPressLoadSlot()
    {
        // trigger event to denote that a load slot button was pressed
        LoadSlotPressedEvent.Trigger(slotSave, saveFileNumber);
    }

    #endregion


}
