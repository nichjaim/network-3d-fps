using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundFadeController : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private ImageFaderController bgFrontFader = null;

    [SerializeField]
    private Image bgFront = null;
    [SerializeField]
    private Image bgBack = null;

    private Sprite currentBg = null;

    #endregion




    #region Transition Functions

    /// <summary>
    /// Performs the background transition.
    /// </summary>
    /// <param name="newBgArg"></param>
    /// <param name="transitionTimeArg"></param>
    public void BackgroundTransition(Sprite newBgArg, float transitionTimeArg)
    {
        // turn ON front BG object
        bgFront.gameObject.SetActive(true);

        // set front BG to the newly given BG
        bgFront.sprite = newBgArg;

        // if there is already a bg set
        if (currentBg != null)
        {
            // turn ON back BG object
            bgBack.gameObject.SetActive(true);

            // set the back bg to the previous BG
            bgBack.sprite = currentBg;

            // fade in the front background
            bgFrontFader.StartFade(transitionTimeArg, true);
        }

        // denote that the current BG is now the newly given BG
        currentBg = newBgArg;
    }

    /// <summary>
    /// Deactivates the backgrounds.
    /// </summary>
    public void RemoveBackgrounds()
    {
        // set BG image's to NULL sprites
        bgFront.sprite = null;
        bgBack.sprite = null;

        // turn OFF BG objects
        bgFront.gameObject.SetActive(false);
        bgBack.gameObject.SetActive(false);

        //set the current BG sprite to a NULL sprite
        currentBg = null;
    }

    #endregion


}
