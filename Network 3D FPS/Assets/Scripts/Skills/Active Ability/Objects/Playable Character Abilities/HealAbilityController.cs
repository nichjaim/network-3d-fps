using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class HealAbilityController : ActiveAbilityController
{
    #region Class Variables

    private BoxCollider _hitboxCollider = null;

    private List<CharacterHealthController> healthAlreadyHealed = new 
        List<CharacterHealthController>();

    #endregion




    #region MonoBehaviour Functions

    private void OnEnable()
    {
        // reset list of those healed
        healthAlreadyHealed = new List<CharacterHealthController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // get colliding object's hitbox
        HitboxController collidingHitbox = other.GetComponent<HitboxController>();

        // if hitbox found
        if (collidingHitbox != null)
        {
            // get health associated with health
            CharacterHealthController collidingHealth = collidingHitbox.CharMaster.
                CharHealth;

            // if colliding char already been healed
            if (healthAlreadyHealed.Contains(collidingHealth))
            {
                // DONT continue code
                return;
            }

            // if hit target is ally of caster
            if (collidingHitbox.CharMaster.CharHealth.AreFactionAllies(
                _casterCharData.factionReputation))
            {
                // heal hitbox target to full health
                collidingHitbox.CharMaster.CharHealth.HealHealth(_casterCharMaster, 999999);

                // denote that hitbox target has been healed
                healthAlreadyHealed.Add(collidingHealth);
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
        // initialize hitbox size property values
        float hitboxSizeXBase = 3f;
        float hitboxSizeXRankAddition = 0.5f;
        float hitboxSizeXMax = 6f;

        float hitboxSizeYBase = 4f;
        float hitboxSizeYRankAddition = 0.5f;
        float hitboxSizeYMax = 7f;

        float hitboxSizeZBase = 5f;
        float hitboxSizeZRankAddition = 0.5f;
        float hitboxSizeZMax = 12f;

        // get calcualted hitbox size values
        float newHitboxSizeX = Mathf.Clamp(hitboxSizeXBase + (hitboxSizeXRankAddition * (abilityRankArg - 1)),
            hitboxSizeXBase, hitboxSizeXMax);
        float newHitboxSizeY = Mathf.Clamp(hitboxSizeYBase + (hitboxSizeYRankAddition * (abilityRankArg - 1)),
            hitboxSizeYBase, hitboxSizeYMax);
        float newHitboxSizeZ = Mathf.Clamp(hitboxSizeZBase + (hitboxSizeZRankAddition * (abilityRankArg - 1)),
            hitboxSizeZBase, hitboxSizeZMax);

        // get calcualted hitbox center values
        float newHitboxCenterY = newHitboxSizeY / 4f;
        float newHitboxCenterZ = newHitboxSizeZ / 4f;

        // set ability hitbox collider to calculated properties
        _hitboxCollider.size = new Vector3(newHitboxSizeX, newHitboxSizeY, newHitboxSizeZ);
        _hitboxCollider.center = new Vector3(0f, newHitboxCenterY, newHitboxCenterZ);
    }

    #endregion


}
