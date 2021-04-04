using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ShieldAbilityController : ActiveAbilityController
{
    #region Class Variables

    private BoxCollider _hitboxCollider = null;

    [Header("Object References")]

    [SerializeField]
    private GameObject visualObject = null;

    #endregion




    #region MonoBehaviour Functions

    private void OnTriggerEnter(Collider other)
    {
        // get colliding projectile
        ProjectileController collidingProjectile = other.GetComponent<ProjectileController>();

        // if projectile found
        if (collidingProjectile != null)
        {
            // if this ability's caster can be hurt by the colliding projectile
            if (_casterCharData.factionReputation.DoesReputationAllowHarm(
                collidingProjectile.CharOwnerData.factionReputation))
            {
                // despawn the colliding projectile
                collidingProjectile.DespawnObject();
            }
        }
    }

    #endregion




    #region Override Functions

    protected override void InitializeComponentReferences()
    {
        base.InitializeComponentReferences();

        _hitboxCollider = GetComponent<BoxCollider>();
    }

    public override void SetupActiveAbility(ActiveAbility castedActiveAbilityArg,
        CharacterMasterController casterCharacterArg, CharacterData casterCharacterDataArg)
    {
        base.SetupActiveAbility(castedActiveAbilityArg, casterCharacterArg,
            casterCharacterDataArg);

        SetupAbilitySize(castedActiveAbilityArg.abilityRank);
    }

    #endregion




    #region Setup Functions

    /// <summary>
    /// Adjusts the ability's visuals and hitbox size based on the given ability rank argument.
    /// </summary>
    /// <param name="abilityRankArg"></param>
    private void SetupAbilitySize(int abilityRankArg)
    {
        // initialize visual object size property values
        float visualSizeXBase = 7f;
        float visualSizeXRankAddition = 0.5f;
        float visualSizeXMax = 12f;

        float visualSizeYBase = 5f;
        float visualSizeYRankAddition = 0.5f;
        float visualSizeYMax = 10f;

        // get calcualted visual object size values
        float newVisualSizeX = Mathf.Clamp(visualSizeXBase + (visualSizeXRankAddition * (abilityRankArg - 1)),
            visualSizeXBase, visualSizeXMax);
        float newVisualSizeY = Mathf.Clamp(visualSizeYBase + (visualSizeYRankAddition * (abilityRankArg - 1)),
            visualSizeYBase, visualSizeYMax);

        // get calculated hitbox size values
        float newHitboxSizeX = newVisualSizeX + 1f;
        float newHitboxSizeY = newVisualSizeY + 1f;

        // set visual object and hitbox sizes based on calcualted values
        visualObject.transform.localScale = new Vector3(newVisualSizeX, newVisualSizeY, 
            visualObject.transform.localScale.z);
        _hitboxCollider.size = new Vector3(newHitboxSizeX, newHitboxSizeY, _hitboxCollider.size.z);
    }

    #endregion


}
