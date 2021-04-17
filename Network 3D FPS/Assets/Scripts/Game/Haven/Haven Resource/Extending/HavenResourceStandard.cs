using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HavenResourceStandard : HavenResource
{
    #region Constructors

    public HavenResourceStandard()
    {
        Setup();
    }

    public HavenResourceStandard(HavenResourceStandard templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup(HavenResourceBurstDecrease templateArg)
    {
        base.Setup(((HavenResource)templateArg));
    }

    #endregion




    #region Override Functions

    public override void ApplyDayPassing()
    {
        base.ApplyDayPassing();

        // decreases current resources based on associated percentage change
        DecreaseResourceByPercentage();
    }

    public override void AddResourceAction()
    {
        base.AddResourceAction();

        // increases current resources based on associated percentage change
        IncreaseResourceByPercentage();
    }

    #endregion


}
