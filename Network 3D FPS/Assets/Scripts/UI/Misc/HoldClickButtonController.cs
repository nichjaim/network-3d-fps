using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldClickButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Class Variables

    private bool pointerDown = false;
    private float pointerDownTimer = 0f;

    [SerializeField]
    private float requiredHoldTime = 1f;

    public UnityEvent onHoldClick;

    [SerializeField]
    private Image fillImage = null;

    #endregion




    #region MonoBehaviour Functions

    private void Update()
    {
        // performs all the pointer down actions, if appropriate
        ProcessPointerDown();
    }

    private void OnEnable()
    {
        // setup the fill image based on current pointer down
        RefreshFillImage();
    }

    #endregion




    #region Hold Button Functions

    /// <summary>
    /// Performs all the pointer down actions, if appropriate to do so. 
    /// Call in Update().
    /// </summary>
    private void ProcessPointerDown()
    {
        // if button NOT being held
        if (!pointerDown)
        {
            // DONT continue code
            return;
        }

        // add pointer down time based on passed time
        pointerDownTimer += Time.deltaTime;

        // if pointer timer passed the needed hold time
        if (pointerDownTimer >= requiredHoldTime)
        {
            // trigger hold-click event if NOT null
            onHoldClick?.Invoke();

            // reset pointer down state to default
            ResetPointer();
        }

        // setup the fill image based on current pointer down
        RefreshFillImage();
    }

    /// <summary>
    /// Reset pointer down state to default. 
    /// Call when completed a hold-click.
    /// </summary>
    private void ResetPointer()
    {
        pointerDown = false;
        pointerDownTimer = 0f;

        // setup the fill image based on current pointer down
        RefreshFillImage();
    }

    /// <summary>
    /// Sets up the fill image based on current pointer down.
    /// </summary>
    private void RefreshFillImage()
    {
        fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
    }

    #endregion




    #region Pointer Functions

    public void OnPointerDown(PointerEventData eventData)
    {
        // denote hold-click has started
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // reset pointer down state to default
        ResetPointer();
    }

    #endregion


}
