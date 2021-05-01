using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HavenActivityData
{
    #region Class Variables

    /// this ID will be how activity dialogue is gotten, as they should 
    /// follow naming convention that incorperates it.
    public string activityId;

    public string activityName;
    [TextArea(3, 15)]
    public string activityDescription;

    public int trait1ActivityPoints;
    public int trait2ActivityPoints;
    public int trait3ActivityPoints;
    public int trait4ActivityPoints;

    #endregion




    #region Constructors

    public HavenActivityData()
    {
        Setup();
    }

    public HavenActivityData(HavenActivityData templateArg)
    {
        Setup(templateArg);
    }

    public HavenActivityData(HavenActivityDataTemplate templateArg)
    {
        Setup(templateArg.template);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        activityId = "0";

        activityName = "ACTVTY_NAME";
        activityDescription = "ACTVTY_DESC";

        trait1ActivityPoints = 0;
        trait2ActivityPoints = 0;
        trait3ActivityPoints = 0;
        trait4ActivityPoints = 0;
    }

    private void Setup(HavenActivityData templateArg)
    {
        activityId = templateArg.activityId;

        activityName = templateArg.activityName;
        activityDescription = templateArg.activityDescription;

        trait1ActivityPoints = templateArg.trait1ActivityPoints;
        trait2ActivityPoints = templateArg.trait2ActivityPoints;
        trait3ActivityPoints = templateArg.trait3ActivityPoints;
        trait4ActivityPoints = templateArg.trait4ActivityPoints;
    }

    #endregion


}
