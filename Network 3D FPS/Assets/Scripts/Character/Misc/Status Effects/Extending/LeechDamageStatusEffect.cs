using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechDamageStatusEffect : StatusEffect
{
    #region Class Variables

    private static string statusColorHexCode = "26FF32";
    private Color statusColor = Color.white;

    private FactionReputation inflicterFactionRep = null;
    private float damageHealingLeechPercentage = 0f;

    #endregion




    #region Constructors

    public LeechDamageStatusEffect(FactionReputation inflicterFactionRepArg, 
        float dmgHealPercArg)
    {
        Setup(inflicterFactionRepArg, dmgHealPercArg);
    }

    #endregion




    #region Setup Functions

    private void Setup(FactionReputation inflicterFactionRepArg, float dmgHealPercArg)
    {
        inflicterFactionRep = inflicterFactionRepArg;
        damageHealingLeechPercentage = dmgHealPercArg;

        statusColor = GeneralMethods.GetColorFromHexColorCode(statusColorHexCode);
    }

    #endregion




    #region Override Functions

    public override void OnStatusEffectStart(CharacterStatusEffectsController
        effectedCharStatusArg)
    {
        base.OnStatusEffectStart(effectedCharStatusArg);

        // add leech value
        effectedCharStatusArg.CharMaster.CharHealth.AddDamageHealingLeechProperties(inflicterFactionRep, 
            damageHealingLeechPercentage);

        effectedCharStatusArg.CharMaster.CharModel.AddTemporaryModelColor(statusColor);
    }

    public override void OnStatusEffectEnd(CharacterStatusEffectsController
        effectedCharStatusArg)
    {
        base.OnStatusEffectEnd(effectedCharStatusArg);

        // remove leech value
        effectedCharStatusArg.CharMaster.CharHealth.RemoveDamageHealingLeechProperties(inflicterFactionRep, 
            damageHealingLeechPercentage);

        effectedCharStatusArg.CharMaster.CharModel.RemoveTemporaryModelColor(statusColor);
    }

    #endregion


}
