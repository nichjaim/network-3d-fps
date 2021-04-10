using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapStatusEffect : StatusEffect
{
    #region Class Variables

    private static string statusColorHexCode = "28C7FF";
    private Color statusColor = Color.white;

    private float movementDecreasePercentage = 0f;

    #endregion




    #region Constructors

    public TrapStatusEffect(float moveDecrPercArg)
    {
        Setup(moveDecrPercArg);
    }

    #endregion




    #region Setup Functions

    private void Setup(float moveDecrPercArg)
    {
        movementDecreasePercentage = moveDecrPercArg;

        statusColor = GeneralMethods.GetColorFromHexColorCode(statusColorHexCode);
    }

    #endregion




    #region Override Functions

    public override void OnStatusEffectStart(CharacterStatusEffectsController
        effectedCharStatusArg)
    {
        base.OnStatusEffectStart(effectedCharStatusArg);

        // add negative move change
        effectedCharStatusArg.CharMaster.CharMovement.AddTemporaryMovementChangePercentage(-movementDecreasePercentage);

        effectedCharStatusArg.CharMaster.CharModel.AddTemporaryModelColor(statusColor);
    }

    public override void OnStatusEffectEnd(CharacterStatusEffectsController
        effectedCharStatusArg)
    {
        base.OnStatusEffectEnd(effectedCharStatusArg);

        // remove the negative move change
        effectedCharStatusArg.CharMaster.CharMovement.AddTemporaryMovementChangePercentage(movementDecreasePercentage);

        effectedCharStatusArg.CharMaster.CharModel.RemoveTemporaryModelColor(statusColor);
    }

    #endregion


}
