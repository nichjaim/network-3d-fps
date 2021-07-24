using BundleSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class AssetRefMethods
{
    #region Asset Bundle Core Functions

    /// <summary>
    /// Returns an asset from an asset bundle.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bundleNameArg"></param>
    /// <param name="assetNameArg"></param>
    /// <returns></returns>
    private static T LoadBundleAsset<T>(string bundleNameArg, string assetNameArg) where T : Object
    {
        return BundleManager.Load<T>(bundleNameArg, assetNameArg);
    }

    /// <summary>
    /// Returns all assets from an asset bundle.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bundleNameArg"></param>
    /// <returns></returns>
    private static T[] LoadAllBundleAsset<T>(string bundleNameArg) where T : Object
    {
        return BundleManager.LoadAll<T>(bundleNameArg);
    }

    #endregion




    #region H Content Core Functions

    /// <summary>
    /// Returns bool that denotes if ero content is in game files.
    /// </summary>
    /// <returns></returns>
    public static bool IsHcontentInstalled()
    {
        return DoesAssetBundleHcontentExist() || DoesStreamingAssetsHcontentExist() || 
            DoesPersistentDataPathHcontentExist();
    }

    /// <summary>
    /// Returns bool that denotes if the hcontent asset bundle is in game files.
    /// </summary>
    /// <returns></returns>
    private static bool DoesAssetBundleHcontentExist()
    {
        return BundleManager.IsAssetExist("hcontent", "bg/bg-1");
    }

    /// <summary>
    /// Returns bool that denotes if the hcontent StreamingAssets files are in the game.
    /// </summary>
    /// <returns></returns>
    private static bool DoesStreamingAssetsHcontentExist()
    {
        return Directory.Exists(GetStreamingAssetsHcontentFolderPath());
    }

    /// <summary>
    /// Returns bool that denotes if the hcontent PersistentDataPath files are in the game.
    /// </summary>
    /// <returns></returns>
    private static bool DoesPersistentDataPathHcontentExist()
    {
        return Directory.Exists(GetPersistentDataPathHcontentFolderPath());
    }

    /// <summary>
    /// Returns the StreamingAssets folder path that holds the hcontent files.
    /// </summary>
    /// <returns></returns>
    private static string GetStreamingAssetsHcontentFolderPath()
    {
        return Path.Combine(Application.streamingAssetsPath, "hcontent");
    }

    /// <summary>
    /// Returns the persistentDataPath folder path that holds the hcontent files.
    /// </summary>
    /// <returns></returns>
    private static string GetPersistentDataPathHcontentFolderPath()
    {
        return Path.Combine(Application.persistentDataPath, "hcontent");
    }

    #endregion




    #region H-Content Background Functions

    /// <summary>
    /// Returns a background sprite from the hcontent files, wherever they're installed.
    /// </summary>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    public static Sprite LoadHcontentBackground(string bgIdArg)
    {
        // if hcontent is in game files through an asset bundle
        if (DoesAssetBundleHcontentExist())
        {
            return LoadAssetBundleHcontentBackground(bgIdArg);
        }
        // else if hcontent is in game files through the StreamingAssets
        else if (DoesStreamingAssetsHcontentExist())
        {
            return LoadStreamingAssetsHcontentBackground(bgIdArg);
        }
        // else if hcontent is in game files through the PersistentDataPath
        else if (DoesPersistentDataPathHcontentExist())
        {
            return LoadPersistentDataPathHcontentBackground(bgIdArg);
        }
        // else hcontent NOT in game files
        else
        {
            // return null sprite
            return null;
        }
    }

    /// <summary>
    /// Returns ero content background sprite from an asset bundle.
    /// </summary>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    private static Sprite LoadAssetBundleHcontentBackground(string bgIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "hcontent";
        string ASSET_NAME_PREFIX = "bg/bg-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + bgIdArg;

        // load and return asset
        return LoadBundleAsset<Sprite>(BUNDLE_NAME, loadAssetName);
    }

    /// <summary>
    /// Returns ero content background sprite from the StreamingAssets.
    /// </summary>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    private static Sprite LoadStreamingAssetsHcontentBackground(string bgIdArg)
    {
        return LoadFolderBasedHcontentBackground(GetStreamingAssetsHcontentFolderPath(), bgIdArg);
    }

    /// <summary>
    /// Returns ero content background sprite from the PersistentDataPath.
    /// </summary>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    private static Sprite LoadPersistentDataPathHcontentBackground(string bgIdArg)
    {
        return LoadFolderBasedHcontentBackground(GetPersistentDataPathHcontentFolderPath(), bgIdArg);
    }

    /// <summary>
    /// Returns ero content background sprite from one of the folder-based asset locations.
    /// </summary>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    private static Sprite LoadFolderBasedHcontentBackground(string folderPathArg, string bgIdArg)
    {
        // get the asset's file name
        string fileName = $"bg-{bgIdArg}{GeneralMethods.GetPngFileExtension()}";
        // get the path to the asset file
        string filePath = Path.Combine(folderPathArg, "bg", fileName);

        // if asset file actually exists
        if (File.Exists(filePath))
        {
            // return sprite loaded from path
            return GeneralMethods.GetSpriteFromFilePath(filePath);
        }
        // else asset file could NOT be found
        else
        {
            // print warning to console
            Debug.LogWarning($"Asset file path does not exist: {filePath}");
            // return null sprite
            return null;
        }
    }

    #endregion




    #region H-Content Portrait Functions

    /*/// <summary>
    /// Returns a portrait set from the hcontent files, wherever they're installed.
    /// </summary>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    public static CharacterPortraitSet LoadHcontentPortraitSet(string bgIdArg)
    {
        // if hcontent is in game files through an asset bundle
        if (DoesAssetBundleHcontentExist())
        {
            return LoadAssetBundleHcontentBackground(bgIdArg);
        }
        // else if hcontent is in game files through the StreamingAssets
        else if (DoesStreamingAssetsHcontentExist())
        {
            return LoadStreamingAssetsHcontentBackground(bgIdArg);
        }
        // else if hcontent is in game files through the PersistentDataPath
        else if (DoesPersistentDataPathHcontentExist())
        {
            return LoadPersistentDataPathHcontentBackground(bgIdArg);
        }
        // else hcontent NOT in game files
        else
        {
            // return null sprite
            return null;
        }
    }

    /// <summary>
    /// Returns ero content portrait set from an asset bundle.
    /// </summary>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    private static Sprite LoadAssetBundleHcontentPortraitSet(string bgIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "hcontent";
        string ASSET_NAME_PREFIX = "bg/bg-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + bgIdArg;

        // load and return asset
        return LoadBundleAsset<Sprite>(BUNDLE_NAME, loadAssetName);
    }

    /// <summary>
    /// Returns ero content portrait set from the StreamingAssets.
    /// </summary>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    private static Sprite LoadStreamingAssetsHcontentBackground(string bgIdArg)
    {
        return LoadFolderBasedHcontentBackground(GetStreamingAssetsHcontentFolderPath(), bgIdArg);
    }

    /// <summary>
    /// Returns ero content portrait set from the PersistentDataPath.
    /// </summary>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    private static Sprite LoadPersistentDataPathHcontentBackground(string bgIdArg)
    {
        return LoadFolderBasedHcontentBackground(GetPersistentDataPathHcontentFolderPath(), bgIdArg);
    }

    /// <summary>
    /// Returns ero content portrait set from one of the folder-based asset locations.
    /// </summary>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    private static Sprite LoadFolderBasedHcontentBackground(string folderPathArg, string bgIdArg)
    {
        // get the asset's file name
        string fileName = $"bg-{bgIdArg}{GeneralMethods.GetPngFileExtension()}";
        // get the path to the asset file
        string filePath = Path.Combine(folderPathArg, "bg", fileName);

        // if asset file actually exists
        if (File.Exists(filePath))
        {
            // return sprite loaded from path
            return GeneralMethods.GetSpriteFromFilePath(filePath);
        }
        // else asset file could NOT be found
        else
        {
            // print warning to console
            Debug.LogWarning($"Asset file path does not exist: {filePath}");
            // return null sprite
            return null;
        }
    }*/

    #endregion




    #region Asset Bundle Character Functions

    /*public static CharacterDataTemplate[] LoadAllBundleAssetPlayableCharacterDataTemplate()
    {
        // initialize bundle properties
        string BUNDLE_NAME = "PlayCharDataTemp";

        //load and return asset
        return LoadAllBundleAsset<CharacterDataTemplate>(BUNDLE_NAME);
    }*/

    public static CharacterDataTemplate LoadBundleAssetCharacterDataTemplate(string charIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "CharData";
        string ASSET_NAME_PREFIX = "chardata-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + charIdArg;

        // load and return asset
        return LoadBundleAsset<CharacterDataTemplate>(BUNDLE_NAME, loadAssetName);
    }

    /*public static AnimatorOverrideController LoadBundleAssetPlayableCharacterAnimator(string charIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "CharAnim";
        string ASSET_NAME_PREFIX = "playable/charanim-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + charIdArg;

        // load and return asset
        return LoadBundleAsset<AnimatorOverrideController>(BUNDLE_NAME, loadAssetName);
    }*/

    public static AnimatorOverrideController LoadBundleAssetCharacterAnimator(string charIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "CharAnim";
        string ASSET_NAME_PREFIX = "charanim-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + charIdArg;

        // load and return asset
        return LoadBundleAsset<AnimatorOverrideController>(BUNDLE_NAME, loadAssetName);
    }

    /*public static GameObject[] LoadAllBundleAssetCharacterEnemy()
    {
        // initialize bundle properties
        string BUNDLE_NAME = "CharEnemy";

        //load and return asset
        return LoadAllBundleAsset<GameObject>(BUNDLE_NAME);
    }*/

    public static Sprite LoadBundleAssetCharacterIcon(string charIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "CharIcon";
        string ASSET_NAME_PREFIX = "charicon-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + charIdArg;

        // load and return asset
        return LoadBundleAsset<Sprite>(BUNDLE_NAME, loadAssetName);
    }

    public static Sprite LoadBundleAssetActiveAbilityIcon(string abilityIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "ActiveAbilityIcon";
        string ASSET_NAME_PREFIX = "abilityicon-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + abilityIdArg;

        // load and return asset
        return LoadBundleAsset<Sprite>(BUNDLE_NAME, loadAssetName);
    }

    public static ActiveAbilityTemplate LoadBundleAssetActiveAbilityTemplate(string abilityIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "ActiveAbilityTemplate";
        string ASSET_NAME_PREFIX = "abilitytemp-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + abilityIdArg;

        // load and return asset
        return LoadBundleAsset<ActiveAbilityTemplate>(BUNDLE_NAME, loadAssetName);
    }

    public static CharacterOutfitTemplate LoadBundleAssetCharacterOutfitTemplate(string outfitIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "CharOutfit";
        string ASSET_NAME_PREFIX = "outfit-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + outfitIdArg;

        // load and return asset
        return LoadBundleAsset<CharacterOutfitTemplate>(BUNDLE_NAME, loadAssetName);
    }

    #endregion




    #region Asset Bundle Inventory Functions

    public static AnimatorOverrideController LoadBundleAssetFirstPersonWeaponModelAnimator(
        WeaponType wepTypeArg, string wepIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "FirstPersWepModelAnim";
        string ASSET_NAME_PREFIX = $"{wepTypeArg.ToString()}/wepanim-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + wepIdArg;

        // load and return asset
        return LoadBundleAsset<AnimatorOverrideController>(BUNDLE_NAME, loadAssetName);
    }

    public static WeaponTypeSetTemplate[] LoadAllBundleAssetWeaponTypeSetTemplate()
    {
        // initialize bundle properties
        string BUNDLE_NAME = "WepTypeSets";

        // load and return assets
        return LoadAllBundleAsset<WeaponTypeSetTemplate>(BUNDLE_NAME);
    }

    public static WeaponDataTemplate LoadBundleAssetWeaponDataTemplate(string wepIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "WepData";
        string ASSET_NAME_PREFIX = "wepdata-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + wepIdArg;

        // load and return asset
        return LoadBundleAsset<WeaponDataTemplate>(BUNDLE_NAME, loadAssetName);
    }

    public static WeaponSlotDataTemplate LoadBundleAssetWeaponSlotDataTemplate(int slotNumArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "WepSlot";
        string ASSET_NAME_PREFIX = "wepslot-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + slotNumArg;

        // load and return asset
        return LoadBundleAsset<WeaponSlotDataTemplate>(BUNDLE_NAME, loadAssetName);
    }

    public static Sprite LoadBundleAssetWeaponTypeSetIcon(string setIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "WepTypeSetIcon";
        string ASSET_NAME_PREFIX = "seticon-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + setIdArg;

        // load and return asset
        return LoadBundleAsset<Sprite>(BUNDLE_NAME, loadAssetName);
    }

    public static Sprite LoadBundleAssetWeaponTypeReticle(WeaponType wepTypeArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "AimReticle";
        string ASSET_NAME_PREFIX = "reticle-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + wepTypeArg.ToString();

        // load and return asset
        return LoadBundleAsset<Sprite>(BUNDLE_NAME, loadAssetName);
    }

    #endregion




    /*#region Asset Bundle Environment Functions

    public static GameObject LoadBundleAssetWorldChunk(bool isUniqueChunkArg, WorldBiomeType worldBiomeTypeArg, string chunkIdArg)
    {
        //initialize bundle properties
        string BUNDLE_NAME_PREFIX = "WorldChunk-";
        string ASSET_NAME_PREFIX = "WorldChunk-";

        //if asset in unique category
        if (isUniqueChunkArg)
        {
            //add category to prefixes
            BUNDLE_NAME_PREFIX += "Unique-";
            //ASSET_NAME_PREFIX += "Unique-";
        }

        //get name of asset to load and bundle to load from
        string loadBundleName = BUNDLE_NAME_PREFIX + worldBiomeTypeArg.ToString();
        string loadAssetName = ASSET_NAME_PREFIX + chunkIdArg;

        //load and return asset
        return LoadBundleAsset<GameObject>(loadBundleName, loadAssetName);
    }

    public static GameObject[] LoadAllBundleAssetWorldChunk(WorldBiomeType worldBiomeTypeArg)
    {
        //initialize bundle properties
        string BUNDLE_NAME_PREFIX = "WorldChunk-";

        //get name of asset to load and bundle to load from
        string loadBundleName = BUNDLE_NAME_PREFIX + worldBiomeTypeArg.ToString();

        //load and return asset
        return LoadAllBundleAsset<GameObject>(loadBundleName);
    }

    public static GameObject[] LoadAllBundleAssetStructureInterior(StructureType structureTypeArg)
    {
        //initialize bundle properties
        string BUNDLE_NAME_PREFIX = "StructureInterior-";

        //get name of asset to load and bundle to load from
        string loadBundleName = BUNDLE_NAME_PREFIX + structureTypeArg.ToString();

        //load and return asset
        return LoadAllBundleAsset<GameObject>(loadBundleName);
    }

    #endregion*/




    #region Asset Bundle Level Arena Functions

    public static GameObject[] LoadAllBundleAssetLevelArenaPrefab(int arenaSetArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME_PREFIX = "LevelArena-";

        // get name of asset to load and bundle to load from
        string loadBundleName = BUNDLE_NAME_PREFIX + arenaSetArg.ToString();

        // load and return asset
        return LoadAllBundleAsset<GameObject>(loadBundleName);
    }

    public static LevelArenaSetDataTemplate[] LoadAllBundleAssetLevelArenaSetDataTemplate()
    {
        // initialize bundle properties
        string BUNDLE_NAME = "ArenaSet";

        // load and return asset
        return LoadAllBundleAsset<LevelArenaSetDataTemplate>(BUNDLE_NAME);
    }

    public static LevelArenaSetDataTemplate LoadBundleAssetLevelArenaSetDataTemplate(int arenaSetArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "ArenaSet";
        string ASSET_NAME_PREFIX = "arenaset-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + arenaSetArg.ToString();

        // load and return asset
        return LoadBundleAsset<LevelArenaSetDataTemplate>(BUNDLE_NAME, loadAssetName);
    }

    public static GameObject LoadBundleAssetTutorialLevelArenaPrefab(int arenaNumInSetArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "LevelArenaTutorial";
        string ASSET_NAME_PREFIX = "arena-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + arenaNumInSetArg.ToString();

        // load and return asset
        return LoadBundleAsset<GameObject>(BUNDLE_NAME, loadAssetName);
    }

    #endregion




    #region Asset Bundle Animation Functions

    /*public static AnimatorOverrideController[] LoadAllBundleAssetAnimatorZombie()
    {
        // initialize bundle properties
        string BUNDLE_NAME = "AnimatorZombie";

        // load and return asset
        return LoadAllBundleAsset<AnimatorOverrideController>(BUNDLE_NAME);
    }*/

    #endregion




    #region Asset Bundle Haven Functions

    public static HavenActivityDataTemplate[] LoadAllBundleAssetHavenActivityTemplates()
    {
        // initialize bundle properties
        string BUNDLE_NAME = "HavenActivity";

        // load and return asset
        return LoadAllBundleAsset<HavenActivityDataTemplate>(BUNDLE_NAME);
    }

    public static HavenActivityDataTemplate LoadBundleAssetHavenActivityTemplate(string activityIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "HavenActivity";
        string ASSET_NAME_PREFIX = "activity-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + activityIdArg;

        // load and return asset
        return LoadBundleAsset<HavenActivityDataTemplate>(BUNDLE_NAME, loadAssetName);
    }

    public static HavenActivityDataTemplate LoadBundleAssetUniqueHavenActivityTemplate(string activityIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "UniqueHavenActivity";
        string ASSET_NAME_PREFIX = "activity-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + activityIdArg;

        // load and return asset
        return LoadBundleAsset<HavenActivityDataTemplate>(BUNDLE_NAME, loadAssetName);
    }

    public static CharacterProgressionInfoSet LoadBundleAssetProgressionInfoLeveling(int havenStatNumArg, string charIdArg, int levelNumArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "ProgressLeveling";
        string ASSET_NAME_PREFIX = $"havenstat-{havenStatNumArg.ToString()}/char-{charIdArg}/level-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + levelNumArg.ToString();

        // load and return asset
        return LoadBundleAsset<CharacterProgressionInfoSet>(BUNDLE_NAME, loadAssetName);
    }

    public static CharacterProgressionInfoSet LoadBundleAssetProgressionInfoLeveling(int havenStatNumArg, string charIdArg)
    {
        return LoadBundleAssetProgressionInfoLeveling(havenStatNumArg, charIdArg, 0);
    }

    public static HavenLocationEventTemplate[] LoadAllBundleAssetHavenLocationEventTemplates()
    {
        // initialize bundle properties
        string BUNDLE_NAME = "HavenLocationEvent";

        // load and return asset
        return LoadAllBundleAsset<HavenLocationEventTemplate>(BUNDLE_NAME);
    }

    #endregion




    #region Asset Bundle Dialogue Functions

    public static Sprite LoadBundleAssetDialogueBackground(string bgIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "DialogueBackground";
        string ASSET_NAME_PREFIX = "bg-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + bgIdArg;

        // load and return asset
        return LoadBundleAsset<Sprite>(BUNDLE_NAME, loadAssetName);
    }

    public static CharacterPortraitSet LoadBundleAssetCharacterPortraitSet(string charIdArg, 
        string outfitIdArg)
    {
        /*// initialize bundle properties
        string BUNDLE_NAME = "DialogueBackground";
        string ASSET_NAME_PREFIX = $"outfit/char-{charIdArg}/outfit-";

        if (outfitArg.isHContent)
        {
            BUNDLE_NAME = "hcontent";
            ASSET_NAME_PREFIX = $"outfit/char-{charIdArg}/outfit-";
        }
        else
        {
            BUNDLE_NAME = "PortraitSet";
            ASSET_NAME_PREFIX = $"char-{charIdArg}/outfit-";
        }*/

        // initialize bundle properties
        string BUNDLE_NAME = "PortraitSet";
        string ASSET_NAME_PREFIX = $"char-{charIdArg}/outfit-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + outfitIdArg;

        // load and return asset
        return LoadBundleAsset<CharacterPortraitSet>(BUNDLE_NAME, loadAssetName);
    }

    #endregion


}
