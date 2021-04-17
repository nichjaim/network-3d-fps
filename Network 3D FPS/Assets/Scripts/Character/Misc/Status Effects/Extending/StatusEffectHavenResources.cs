using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectHavenResources : StatusEffect
{
    #region Class Variables

    private float overallChangePercentage = 0f;

    #endregion




    #region Constructors

    public StatusEffectHavenResources(float changePercArg)
    {
        Setup(changePercArg);
    }

    #endregion




    #region Setup Functions

    private void Setup(float changePercArg)
    {
        overallChangePercentage = changePercArg;
    }

    #endregion




    #region Override Functions

    public override void OnStatusEffectStart(CharacterStatusEffectsController
        effectedCharStatusArg)
    {
        base.OnStatusEffectStart(effectedCharStatusArg);

        // NOTE: half any movement change so that a debuff isn't too annoying
        effectedCharStatusArg.CharMaster.CharMovement.
            AddTemporaryMovementChangePercentage(overallChangePercentage / 2f);

        Debug.Log("NEED IMPL: start status effect change max health"); // NEED IMPL
        Debug.Log("NEED IMPL: start status effect change damage done by weapon"); // NEED IMPL
        Debug.Log("NEED IMPL: start status effect change ability cooldown"); // NEED IMPL
    }

    public override void OnStatusEffectEnd(CharacterStatusEffectsController
        effectedCharStatusArg)
    {
        base.OnStatusEffectEnd(effectedCharStatusArg);

        // remove the negative move change
        effectedCharStatusArg.CharMaster.CharMovement.
            AddTemporaryMovementChangePercentage(-(overallChangePercentage / 2f));

        Debug.Log("NEED IMPL: undo status effect change max health"); // NEED IMPL
        Debug.Log("NEED IMPL: undo status effect change damage done by weapon"); // NEED IMPL
        Debug.Log("NEED IMPL: undo status effect change ability cooldown"); // NEED IMPL
    }

    #endregion


}
