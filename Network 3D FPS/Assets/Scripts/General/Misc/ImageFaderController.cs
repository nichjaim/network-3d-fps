using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFaderController : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private Image _image = null;
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null;

    private bool isFadingActive = false;
    private bool isFadingIn = false;

    private float totalFadeTime = 0f;
    private float currentFadeTime = 0f;

    #endregion




    #region MonoBehaviour Functions

    private void Update()
    {
        // performs the fading processes
        ProcessFade();
    }

    #endregion




    #region Fader Functions

    /// <summary>
    /// Performs the fading processes. 
    /// Call in Update().
    /// </summary>
    private void ProcessFade()
    {
        // if currently should NOT be fading
        if (!isFadingActive)
        {
            // DONT continue code
            return;
        }

        // set visual's transparency to the current transparency color
        SetVisualTransparency(GetCurrentTransparencyValue());

        // increase fade timer time by time passed
        currentFadeTime += Time.deltaTime;

        // if timer is done
        if (currentFadeTime >= totalFadeTime)
        {
            // stop fading
            isFadingActive = false;
        }
    }

    /// <summary>
    /// Begins the fading process.
    /// </summary>
    /// <param name="fadeTimeArg"></param>
    /// <param name="fadeInArg"></param>
    public void StartFade(float fadeTimeArg, bool fadeInArg)
    {
        // set current fade time to timer start
        currentFadeTime = 0f;
        // set timer goal to given time
        totalFadeTime = fadeTimeArg;

        // denote whether fading in or out by given fade bool
        isFadingIn = fadeInArg;

        // begin fading
        isFadingActive = true;
    }

    /// <summary>
    /// Returns the appropriate alpha value based on current fade timer.
    /// </summary>
    /// <returns></returns>
    private float GetCurrentTransparencyValue()
    {
        // get the current alpha value based on the remaining time left
        float transpValue = currentFadeTime / totalFadeTime;

        // esnure that the value is a valid alpha value
        transpValue = Mathf.Clamp(transpValue, 0f, 1f);

        // if fading out
        if (!isFadingIn)
        {
            // inverse the alpha value
            transpValue = 1 - transpValue;
        }

        // return the calculated alpha value
        return transpValue;
    }

    /// <summary>
    /// Sets the transparency of the visual component's color.
    /// </summary>
    /// <param name="alphaArg"></param>
    private void SetVisualTransparency(float alphaArg)
    {
        // if image reference setup
        if (_image != null)
        {
            // change image's color to given alpha value
            _image.color = GetTransparencyColor(_image.color, alphaArg);
        }
        else if (_spriteRenderer != null)
        {
            // change SR's color to given alpha value
            _spriteRenderer.color = GetTransparencyColor(_spriteRenderer.color, alphaArg);
        }
    }

    /// <summary>
    /// Returns the given color with the given alpha value set.
    /// </summary>
    /// <param name="colorArg"></param>
    /// <param name="alphaArg"></param>
    /// <returns></returns>
    private Color GetTransparencyColor(Color colorArg, float alphaArg)
    {
        // set return color to given color's properties
        Color returnColor = colorArg;

        // change the color's transparency to given alpha value
        returnColor.a = alphaArg;

        // return setup color
        return returnColor;
    }

    #endregion


}
