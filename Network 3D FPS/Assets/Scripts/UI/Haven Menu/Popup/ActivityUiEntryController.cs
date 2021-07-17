using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActivityUiEntryController : MonoBehaviour
{
    #region Class Variables

    [SerializeField]
    private TextMeshProUGUI activityNameText = null;

    [Header("Activity Point References")]

    [SerializeField]
    private TextMeshProUGUI trait1PointCountText = null;
    [SerializeField]
    private TextMeshProUGUI trait2PointCountText = null;
    [SerializeField]
    private TextMeshProUGUI trait3PointCountText = null;
    [SerializeField]
    private TextMeshProUGUI trait4PointCountText = null;

    #endregion




    #region Setup Functions

    /// <summary>
    /// Sets up all the entry components based on given activity.
    /// </summary>
    /// <param name="templateArg"></param>
    public void SetupActivityEntry(HavenActivityData templateArg)
    {
        // set texct of activity name
        activityNameText.text = templateArg.activityName;

        // set text of activity's trait points
        trait1PointCountText.text = templateArg.trait1ActivityPoints.ToString();
        trait2PointCountText.text = templateArg.trait2ActivityPoints.ToString();
        trait3PointCountText.text = templateArg.trait3ActivityPoints.ToString();
        trait4PointCountText.text = templateArg.trait4ActivityPoints.ToString();
    }

    #endregion


}
