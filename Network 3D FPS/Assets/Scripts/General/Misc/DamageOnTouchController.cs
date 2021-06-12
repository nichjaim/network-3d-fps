using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouchController : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private CharacterMasterController _charMaster = null;

    [SerializeField]
    private AIBrain _aiBrain = null;

    [Tooltip("The layers that can be effected by the damage touch.")]
    [SerializeField]
    private LayerMask effectableLayers = new LayerMask();

    [Header("Touch Properties")]

    [SerializeField]
    private float damageCooldownTime = 1f;
    private bool onDamageCooldown = false;

    [Tooltip("Can a target only be harmed on touch if the AI is targeting the " +
        "entity being touched?")]
    [SerializeField]
    private bool requireAITargetingToHarm = false;

    [SerializeField]
    private int baseDamageBoundaryLow = 1;
    [SerializeField]
    private int baseDamageBoundaryHigh = 1;

    #endregion




    #region MonoBehaviour Functions

    private void OnEnable()
    {
        // ensure touch damaging is OFF cooldown
        onDamageCooldown = false;
    }

    private void OnTriggerStay(Collider other)
    {
        // if collision should NOT be effected by damage touch
        if (!IsEffectableCollision(other))
        {
            // DONT continue code
            return;
        }

        // if touch damaging is ON a cooldown
        if (onDamageCooldown)
        {
            // DONT continue code
            return;
        }

        // get collididng object's hitbox component
        HitboxController collidingHitbox = other.GetComponent<HitboxController>();

        // if can harm the hitbox target
        if (GeneralMethods.CanHarmHitboxTarget(_charMaster.CharData, collidingHitbox))
        {
            OnTargetTouch(collidingHitbox);
        }
    }

    #endregion




    #region Touch Functions

    /// <summary>
    /// Should be called when a harmable target has been touched.
    /// </summary>
    /// <param name="collidingHitboxArg"></param>
    private void OnTargetTouch(HitboxController collidingHitboxArg)
    {
        // if given invalid hitbox
        if (collidingHitboxArg == null)
        {
            // DONT continue code
            return;
        }

        /// if AI targeting is required to harm the target BUT the AI is NOT currently 
        /// targeting the one who was touched
        if (requireAITargetingToHarm && (collidingHitboxArg.CharMaster.transform != 
            _aiBrain.Target))
        {
            // DONT continue code
            return;
        }

        // harm target based on character's stats
        collidingHitboxArg.CharMaster.CharHealth.TakeDamage(_charMaster, GetRandomDamage());

        // places touch damaging on a cooldown for the appropriate amount of time
        DamageCooldown();
    }

    /// <summary>
    /// Returns random damage amount based on base damage value and character's stats.
    /// </summary>
    /// <returns></returns>
    private int GetRandomDamage()
    {
        // get random base damage value between the damage boundaries
        int randomDamage = Random.Range(baseDamageBoundaryLow, baseDamageBoundaryHigh);

        // get the front-facing character being used by this component's char owner
        CharacterData charInUse = GeneralMethods.GetFrontFacingCharacterDataFromCharMaster(_charMaster);

        // add char's strength stat to the damage
        randomDamage += charInUse.characterStats.statStrength;

        // ensure damage is a valid damage value
        randomDamage = Mathf.Max(0, randomDamage);

        // return the calcualted damage value
        return randomDamage;
    }

    /// <summary>
    /// Places touch damaging on a cooldown for the appropriate amount of time.
    /// </summary>
    private void DamageCooldown()
    {
        // call internal function as coroutine
        StartCoroutine(DamageCooldownInternal());
    }

    private IEnumerator DamageCooldownInternal()
    {
        // put damaging ON cooldown
        onDamageCooldown = true;

        // wait cooldown time
        yield return new WaitForSeconds(damageCooldownTime);

        // take damaging OFF cooldown
        onDamageCooldown = false;
    }

    /// <summary>
    /// Returns whether the given collision is considered effectable by the damage touch.
    /// </summary>
    /// <param name="colliderArg"></param>
    /// <returns></returns>
    private bool IsEffectableCollision(Collider colliderArg)
    {
        return GeneralMethods.DoesLayerMaskContainLayer(effectableLayers, 
            colliderArg.gameObject.layer);
    }

    #endregion


}
