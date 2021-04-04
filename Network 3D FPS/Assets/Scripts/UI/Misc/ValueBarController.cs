using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValueBarController : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [Tooltip("The top layer object that parents all the bar objects")]
    [SerializeField]
    private GameObject objectContainer = null;

    [SerializeField]
    private GameObject barBorder = null;
    private Image _imageBarBorder = null;
    private RectTransform _transBarBorder = null;

    [SerializeField]
    private GameObject barInner = null;
    private Image _imageBarInner = null;
    private RectTransform _transBarInner = null;

    [SerializeField]
    private TextMeshProUGUI valueText = null;

    // the current max value of the bar
    private float maxValue = 1;

    [Header("Bar Length Properties")]

    [SerializeField]
    private float baseBarLength = 100f;
    [SerializeField]
    private float barLengthIncreasePerMaxValue = 1f;

    [SerializeField]
    private float borderBarLengthAdvantage = 20f;

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // setup the references to the bars component references
        InitializeBarComponentReferences();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up the references to the bars component references. 
    /// Call in Awake().
    /// </summary>
    private void InitializeBarComponentReferences()
    {
        _imageBarBorder = barBorder.GetComponent<Image>();
        _transBarBorder = barBorder.GetComponent<RectTransform>();

        _imageBarInner = barInner.GetComponent<Image>();
        _transBarInner = barInner.GetComponent<RectTransform>();
    }

    #endregion




    #region Bar Functions

    /// <summary>
    /// Sets up the bar's dimensional properties based on the new values.
    /// </summary>
    /// <param name="currentValueArg"></param>
    /// <param name="maxValueArg"></param>
    public void RefreshBarDimensions(float currentValueArg, float maxValueArg)
    {
        // set the max value to the one given
        maxValue = maxValueArg;

        // get how much to increase the base bar length by
        float barLengthIncrease = maxValue * barLengthIncreasePerMaxValue;

        // get the INNER bar's total length
        float innerBarLength = baseBarLength + barLengthIncrease;
        // get the BORDER bar's total length
        float borderBarLength = innerBarLength + (borderBarLengthAdvantage * 2f);

        // set the bar sizes
        _transBarInner.sizeDelta = new Vector2(innerBarLength, _transBarInner.sizeDelta.y);
        _transBarBorder.sizeDelta = new Vector2(borderBarLength, _transBarBorder.sizeDelta.y);

        // setup the bar's properties based on a current value change
        RefreshBarValue(currentValueArg);
    }

    /// <summary>
    /// Sets up the bar's properties based on a current value change.
    /// </summary>
    /// <param name="currentValueArg"></param>
    public void RefreshBarValue(float currentValueArg)
    {
        // set inner bar's fill to the appropriate percentage
        _imageBarInner.fillAmount = currentValueArg / maxValue;

        // set value text to appropriate string
        valueText.text = GetValueText(currentValueArg);
    }

    /// <summary>
    /// Returns the appropriate value text based on given current value.
    /// </summary>
    /// <param name="currentValueArg"></param>
    /// <returns></returns>
    private string GetValueText(float currentValueArg)
    {
        return currentValueArg.ToString() + " / " + maxValue.ToString();
    }

    /// <summary>
    /// Turn on/off the bar object depending on the given bool.
    /// </summary>
    /// <param name="activeArg"></param>
    public void SetBarActivation(bool activeArg)
    {
        objectContainer.SetActive(activeArg);
    }

    #endregion


}
