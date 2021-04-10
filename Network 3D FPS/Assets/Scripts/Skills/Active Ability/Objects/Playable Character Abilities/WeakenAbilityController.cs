using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeakenAbilityController : ActiveAbilityController
{
    #region Class Variables

    private NetworkObjectPooler _weakenProjectilePooler = null;

    private float damageTakenIncreasePercentage = 0f;
    private float effectDuration = 1f;

    private int targetPenetration = 1;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all variables that reference singleton instance related components
        InitializeSingletonReferences();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Setup all variables that reference singleton instance related components. 
    /// Call in Start(), needs to be start to give the instances time to be created.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        _weakenProjectilePooler = GameManager.Instance.SpawnManager.WeakeningProjectilePooler;
    }

    #endregion




    #region Override Functions

    public override void SetupActiveAbility(ActiveAbility castedActiveAbilityArg, 
        CharacterMasterController casterCharacterArg, CharacterData casterCharacterDataArg)
    {
        base.SetupActiveAbility(castedActiveAbilityArg, casterCharacterArg, casterCharacterDataArg);

        // setup ability proeprties
        SetupAbilityDamageTakenIncreasePercentage(_castedAbility.abilityRank);
        SetupAbilityEffectDuration(_castedAbility.abilityRank);
        SetupAbilityTargetPenetration(_castedAbility.abilityRank);

        // fires off the appropriate amount of weakening projectiles
        FireWeakeningProjectiles();
    }

    #endregion




    #region Setup Functions

    /// <summary>
    /// Adjusts the ability's damage taken increase based on the given ability rank argument.
    /// </summary>
    /// <param name="abilityRankArg"></param>
    private void SetupAbilityDamageTakenIncreasePercentage(int abilityRankArg)
    {
        // initialize ability damage taken property values
        float dmgTakeBase = 0.5f;
        float dmgTakeRankAddition = 0.25f;
        float dmgTakeMax = 10f;

        // get calculated damage percentage value
        float newDmgTake = Mathf.Clamp(dmgTakeBase + (dmgTakeRankAddition *
            (abilityRankArg - 1)), dmgTakeBase, dmgTakeMax);

        // set damage taken increase to calcualted damage percentage value
        damageTakenIncreasePercentage = newDmgTake;
    }

    /// <summary>
    /// Adjusts the ability's status effect duration based on the given ability rank argument.
    /// </summary>
    /// <param name="abilityRankArg"></param>
    private void SetupAbilityEffectDuration(int abilityRankArg)
    {
        // initialize ability effect duration property values
        float effectDurBase = 15f;
        float effectDurRankAddition = 5f;
        float effectDurMax = 300f;

        // get calculated damage percentage value
        float newEffectDur = Mathf.Clamp(effectDurBase + (effectDurRankAddition *
            (abilityRankArg - 1)), effectDurBase, effectDurMax);

        // set status effect duration to calcualted effect duration value
        effectDuration = newEffectDur;
    }

    /// <summary>
    /// Adjusts the ability's max target penetration based on the given ability rank argument.
    /// </summary>
    /// <param name="abilityRankArg"></param>
    private void SetupAbilityTargetPenetration(int abilityRankArg)
    {
        // initialize relevant ability property values
        int valueBase = 5;
        int valueRankAddition = 2;
        int valueMax = 50;

        // get calculated value
        int newValue = Mathf.Clamp(valueBase + (valueRankAddition *
            (abilityRankArg - 1)), valueBase, valueMax);

        // set target penetration to calculated value
        targetPenetration = newValue;
    }

    #endregion




    #region Ability Functions

    /// <summary>
    /// Returns a random rotation within the ability's projectile firing cone boundary.
    /// </summary>
    /// <returns></returns>
    private Quaternion GetRandomProjectileRotation()
    {
        // initialize rotational change range
        float rotChangeRange = 15f;

        // get random rotational changes for the X and Y axis
        float newRotChangeX = Random.Range(-rotChangeRange, rotChangeRange);
        float newRotChangeY = Random.Range(-rotChangeRange, rotChangeRange);

        // get the base direction to start making changes to
        //Vector3 baseDirection = transform.forward;
        Vector3 baseDirection = transform.localRotation.eulerAngles;

        // get new directional values for the X and Y axis
        float newDirX = baseDirection.x + newRotChangeX;
        float newDirY = baseDirection.y + newRotChangeY;

        // get new direction from new directional values
        Quaternion newDirection = Quaternion.Euler(newDirX, newDirY, baseDirection.z);
        // return the new direction
        return newDirection;
    }

    /// <summary>
    /// Returns number of projectiles to fire based on given abilty rank.
    /// </summary>
    /// <param name="abilityRankArg"></param>
    /// <returns></returns>
    private int GetNumberOfProjectileBasedOnAbilityRank(int abilityRankArg)
    {
        // initialize count property values
        int projCountBase = 5;
        int projCountRankAddition = 2;
        int projCountMax = 20;

        // get calcualted count value
        int newProjCount = Mathf.Clamp(projCountBase + (projCountRankAddition * (abilityRankArg - 1)),
            projCountBase, projCountMax);

        // return calcualted count
        return newProjCount;
    }

    /// <summary>
    /// Fires off the appropriate amount of weakening projectiles.
    /// </summary>
    private void FireWeakeningProjectiles()
    {
        // if pooler reference NOT set
        if (_weakenProjectilePooler == null)
        {
            // set necessary reference
            InitializeSingletonReferences();
        }

        // get number of projectiles to fire
        int projNum = GetNumberOfProjectileBasedOnAbilityRank(_castedAbility.abilityRank);

        // loop through every projectile to be fired
        for (int i = 0; i < projNum; i++)
        {
            // spawn and fire a projectile
            SpawnProjectile();
        }
    }

    #endregion




    #region Spawn Projectile Functions

    /// <summary>
    /// Spawns and fires a projectile.
    /// </summary>
    private void SpawnProjectile()
    {
        if (NetworkClient.isConnected)
        {
            CmdSpawnProjectile();
        }
        else
        {
            SpawnProjectileInternal();
        }
    }

    [Command]
    private void CmdSpawnProjectile()
    {
        SpawnProjectileInternal();
    }

    private void SpawnProjectileInternal()
    {
        // get object from given pooler
        GameObject projectileObj = _weakenProjectilePooler.GetFromPool(_castPoint.position,
            GetRandomProjectileRotation());

        // spawn pooled object on network
        NetworkServer.Spawn(projectileObj);

        // get projectile from pooled object
        WeakeningProjectileController projectile = projectileObj.GetComponent<WeakeningProjectileController>();
        // if no such component found
        if (projectile == null)
        {
            // print warning to console
            Debug.LogWarning("Projectile object does NOT have ProjectileController component! " +
                $"Object name: {projectileObj.name}");

            // DONT continue code
            return;
        }

        // setup projectile data
        projectile.SetupProjectile(_casterCharMaster);
        projectile.TargetPenetrationMax = targetPenetration;
        projectile.SetupWeakeningProjectile(damageTakenIncreasePercentage, effectDuration);
    }

    #endregion


}
