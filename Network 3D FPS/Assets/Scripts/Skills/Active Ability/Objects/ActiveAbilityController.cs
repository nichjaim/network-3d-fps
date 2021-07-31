using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkPooledObjectController))]
public abstract class ActiveAbilityController : NetworkBehaviour
{
    #region Character Functions

    protected NetworkPooledObjectController _pooledObject = null;

    protected ActiveAbility _castedAbility = null;
    protected CharacterMasterController _casterCharMaster = null;
    protected CharacterData _casterCharData = null;
    protected Transform _castPoint = null;

    protected int _appropriateAbilityRank = 1;

    protected float abilityDuration = 1f;

    [Header("Base Ability Movement Properties")]

    [Tooltip("Should the ability object move with the caster's fire point?")]
    [SerializeField]
    private bool movesWithCastPoint = false;

    [Header("Base Ability Duration Properties")]

    [SerializeField]
    protected float durationBase = 5f;
    [SerializeField]
    protected float durationRankAddition = 1f;
    [SerializeField]
    protected float durationMax = 20f;

    #endregion




    #region MonoBehaviour Functions

    protected virtual void Awake()
    {
        // setup the vars that hold references to relevant components
        InitializeComponentReferences();
    }

    protected virtual void FixedUpdate()
    {
        // moves and rotates this object based on the cast point, if should do so
        MoveWithCastPointIfAppropriate();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up the vars that hold references to relevant components. 
    /// Call in Awake().
    /// </summary>
    protected virtual void InitializeComponentReferences()
    {
        _pooledObject = GetComponent<NetworkPooledObjectController>();
    }

    #endregion




    #region Setup Functions

    public virtual void SetupActiveAbility(ActiveAbility castedActiveAbilityArg, 
        CharacterMasterController casterCharacterArg, CharacterData casterCharacterDataArg)
    {
        _castedAbility = castedActiveAbilityArg;
        _casterCharMaster = casterCharacterArg;
        _casterCharData = casterCharacterDataArg;
        _castPoint = casterCharacterArg.CharSight.FirePoint;

        // setup the ability rank to the rank calcualted based on the casted ability and the caster
        SetupAppropriateAbilityRank();

        // setup how long the object will be active for
        SetupObjectLifetime();
    }

    /// <summary>
    /// Sets up the appropriate ability rank based on the casted ability and the caster.
    /// </summary>
    protected void SetupAppropriateAbilityRank()
    {
        // initialize ability rank as the casting ability's rank
        int newAbilityRank = _castedAbility.abilityRank;

        // add any of the caster's ability proficiency if they are above the base level
        newAbilityRank += (_casterCharData.characterStats.statAllAbilityProficiencyLevel - 1);

        // if ability rank is an invalid value
        if (newAbilityRank < 1)
        {
            // print warning to console
            Debug.LogWarning($"Invalid ability rank of: {newAbilityRank}");
        }

        // ensure ability rank is a valid value
        newAbilityRank = Mathf.Max(1, newAbilityRank);

        // set apporpriate ability rank to the newly calculated rank
        _appropriateAbilityRank = newAbilityRank;
    }

    /// <summary>
    /// Sets up how long the object will be active for.
    /// </summary>
    protected virtual void SetupObjectLifetime()
    {
        // refresh ability duration based on rank
        SetupAbilityDuration();

        // despawn object after lifetime has run it's course
        DespawnObjectAfterLifetime();
    }

    /// <summary>
    /// Adjusts the ability's time duration based on the given ability rank argument.
    /// </summary>
    protected virtual void SetupAbilityDuration()
    {
        // get calculated duration
        float newDuration = Mathf.Clamp(durationBase + (durationRankAddition * (_appropriateAbilityRank - 1)),
            durationBase, durationMax);
        // set ability duration to new ability duration
        abilityDuration = newDuration;
    }

    #endregion




    #region Object Functions

    /// <summary>
    /// Moves and rotates this object based on the cast point, if should do so. 
    /// Call in FixedUpdate().
    /// </summary>
    protected virtual void MoveWithCastPointIfAppropriate()
    {
        // if should move with caster's fire point
        if (movesWithCastPoint)
        {
            transform.SetPositionAndRotation(_castPoint.position, _castPoint.rotation);
        }
    }

    /// <summary>
    /// De-spawns the object after it's lifetime has run it's course.
    /// </summary>
    protected void DespawnObjectAfterLifetime()
    {
        // start the internal function as a coroutine
        StartCoroutine(DespawnObjectDeactivationInternal());
    }

    protected virtual IEnumerator DespawnObjectDeactivationInternal()
    {
        // waits for the extent of the ability duration time length
        yield return new WaitForSeconds(abilityDuration);

        // despawn the object
        DespawnObject();
    }

    /// <summary>
    /// Despawn this networked object.
    /// </summary>
    protected virtual void DespawnObject()
    {
        _pooledObject.UnspawnObject();
    }

    #endregion


}
