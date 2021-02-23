using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuMainMenuController : StandardSubMenuController
{
    #region Button Press Functions

    public void ButtonPressUnpause()
    {
        // trigger event to unpause game
        GamePausingActionEvent.Trigger();
    }

    public void ButtonPressMainMenu()
    {
        // trigger event to unpause game
        GamePausingActionEvent.Trigger();
        // trivver event to leave current game session
        ExitGameSessionEvent.Trigger();
    }

    #endregion


}
