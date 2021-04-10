using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakeningStatusEffect : StatusEffect
{
    #region Class Variables

    private static string statusColorHexCode = "A400E5";
    private Color statusColor = Color.white;

    private float damageTakenIncreasePercentage = 0f;

    #endregion




    #region Constructors

    public WeakeningStatusEffect(float dmgTakeIncrPercArg)
    {
        Setup(dmgTakeIncrPercArg);
    }

    #endregion




    #region Setup Functions

    private void Setup(float dmgTakeIncrPercArg)
    {
        damageTakenIncreasePercentage = dmgTakeIncrPercArg;

        statusColor = GeneralMethods.GetColorFromHexColorCode(statusColorHexCode);
    }

    #endregion




    #region Override Functions

    public override void OnStatusEffectStart(CharacterStatusEffectsController 
        effectedCharStatusArg)
    {
        base.OnStatusEffectStart(effectedCharStatusArg);

        effectedCharStatusArg.CharMaster.CharHealth.AddDamageTakenChangePercentage(damageTakenIncreasePercentage);
        effectedCharStatusArg.CharMaster.CharHealth.AddTemporaryDamagePopupColor(statusColor);

        effectedCharStatusArg.CharMaster.CharModel.AddTemporaryModelColor(statusColor);
    }

    public override void OnStatusEffectEnd(CharacterStatusEffectsController 
        effectedCharStatusArg)
    {
        base.OnStatusEffectEnd(effectedCharStatusArg);

        effectedCharStatusArg.CharMaster.CharHealth.AddDamageTakenChangePercentage(-damageTakenIncreasePercentage);
        effectedCharStatusArg.CharMaster.CharHealth.RemoveTemporaryDamagePopupColor(statusColor);

        effectedCharStatusArg.CharMaster.CharModel.RemoveTemporaryModelColor(statusColor);
    }

    #endregion


}
