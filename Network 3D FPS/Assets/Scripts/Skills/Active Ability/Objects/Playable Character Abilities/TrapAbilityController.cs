using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror.Experimental;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody), typeof(NetworkRigidbody))]
public class TrapAbilityController : ActiveAbilityController
{
    #region Class Variables

    private Rigidbody _rigBody = null;

    private bool isTrapSprung = false;

    private Vector3 abilitySizeUnsprung = Vector3.one;
    private Vector3 abilitySizeSprung = Vector3.one;

    [Header("Component References")]

    /// NOTE: need to manually set this reference for this due to two box collider 
    /// components being on this object
    [SerializeField]
    [Tooltip("The box collider component that is the trigger NOT the physical collider")]
    private BoxCollider _hitboxCollider = null;

    [Header("Object References")]

    [SerializeField]
    private ParticleSystem trapSprungVisuals = null;

    [Tooltip("The velocity at which the object is thrown when spawned.")]
    [SerializeField]
    private float objectThrowStrength = 10f;

    [Header("Sprite-Based References")]

    [SerializeField]
    private SpriteRenderer abilityModel = null;

    [SerializeField]
    private Sprite abilitySpriteSprung = null;
    [SerializeField]
    private Sprite abilitySpriteUnsprung = null;

    #endregion




    #region MonoBehaviour Functions

    private void OnEnable()
    {
        /// straightens out the object X and Z rotation. (NOTE: need to do this due 
        /// to the object being locked at the angle it was spawned at)
        StraightenObjectRotation();

        // setup trap state as being UN-sprung
        PrepareTrap();

        // sharply moves the the object forward
        ThrowObjectForward();
    }

    private void OnTriggerEnter(Collider other)
    {
        // get colliding object's hitbox
        HitboxController collidingHitbox = other.GetComponent<HitboxController>();

        // if hitbox found
        if (collidingHitbox != null)
        {
            // if target CAN hurt caster
            if (collidingHitbox.CharHealth.CanAttackerCauseHarmBasedOnReputation(
                _casterCharData.factionReputation))
            {
                // if trap has already been sprung
                if (isTrapSprung)
                {
                    // apply slow debuff to collding enemy
                    Debug.Log("NEED IMPL: apply slow debuff to collding enemy"); // NEED IMPL
                }
                // else trap has NOT been sprung yet
                else
                {
                    // setup trap state as being sprung
                    SpringTrap();
                }
            }
        }
    }

    #endregion




    #region Override Functions

    protected override void InitializeComponentReferences()
    {
        base.InitializeComponentReferences();

        _rigBody = GetComponent<Rigidbody>();
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
        // initialize ability size property values
        float unsprungAbilitySizeBase = 3f;
        float unsprungAbilitySizeBaseRankAddition = 1f;
        float unsprungAbilitySizeBaseMax = 10f;

        float sprungAbilitySizeBase = 12f;
        float sprungAbilitySizeBaseRankAddition = 2f;
        float sprungAbilitySizeBaseMax = 30f;

        // get calculated ability size values
        float newUnsprungAbilitySize = Mathf.Clamp(unsprungAbilitySizeBase + (unsprungAbilitySizeBaseRankAddition * 
            (abilityRankArg - 1)), unsprungAbilitySizeBase, unsprungAbilitySizeBaseMax);
        float newSprungAbilitySize = Mathf.Clamp(sprungAbilitySizeBase + (sprungAbilitySizeBaseRankAddition *
            (abilityRankArg - 1)), sprungAbilitySizeBase, sprungAbilitySizeBaseMax);

        // set ability size vectors based on calculated size values
        abilitySizeUnsprung = new Vector3(newUnsprungAbilitySize, newUnsprungAbilitySize, newUnsprungAbilitySize);
        abilitySizeSprung = new Vector3(newSprungAbilitySize, newSprungAbilitySize, newSprungAbilitySize);

        /// get size of ability model based on how big the unsprung hitbox is (NOTE: going to be a 
        /// bit smaller than actual hitbox as huge model would like a little silly)
        float newAbilityModelSize = newUnsprungAbilitySize * 0.5f;
        // set ability model's scale based on calculated size
        abilityModel.transform.localScale = new Vector3(newAbilityModelSize, newAbilityModelSize, 1f);

        Debug.Log("NEED IMPL: set ability sprung particle visuals area size"); // NEED IMPL
    }

    #endregion




    #region Trap Functions

    /// <summary>
    /// Setup trap state as being UN-sprung.
    /// </summary>
    private void PrepareTrap()
    {
        SetupTrapSpringState(false);
    }

    /// <summary>
    /// Setup trap state as being sprung.
    /// </summary>
    private void SpringTrap()
    {
        SetupTrapSpringState(true);
    }

    /// <summary>
    /// Sets up the traps collider size and visual activation based on 
    /// if the trap should be sprung or not.
    /// </summary>
    /// <param name="shouldSpringArg"></param>
    private void SetupTrapSpringState(bool shouldSpringArg)
    {
        // set trap sprung state based on given bool
        isTrapSprung = shouldSpringArg;

        // if trap should be set to sprung
        if (shouldSpringArg)
        {
            // set hitbox collider's size to ability's sprung size
            _hitboxCollider.size = abilitySizeSprung;

            // play trap sprung visuals
            trapSprungVisuals.Play();

            // set ability model's sprite to sprung sprite
            abilityModel.sprite = abilitySpriteSprung;
        }
        // else trap should be set to UN-sprung
        else
        {
            // set hitbox collider's size to ability's UN-sprung size
            _hitboxCollider.size = abilitySizeUnsprung;

            // stop trap sprung visuals
            trapSprungVisuals.Stop();

            // set ability model's sprite to UN-sprung sprite
            abilityModel.sprite = abilitySpriteUnsprung;
        }
    }

    #endregion




    #region Object Functions

    /// <summary>
    /// Sharply moves the the object forward.
    /// </summary>
    private void ThrowObjectForward()
    {
        _rigBody.velocity = transform.forward * objectThrowStrength;
    }

    /// <summary>
    /// Straightens out the object X and Z rotation.
    /// </summary>
    private void StraightenObjectRotation()
    {
        // get the current Y rotation value
        float currentRotY = transform.localRotation.eulerAngles.y;

        // get straigtened out rotation
        Quaternion straightRot = Quaternion.Euler(1f, currentRotY, 1f);

        // set rotation to new rotation
        transform.localRotation = straightRot;
    }

    #endregion


}
