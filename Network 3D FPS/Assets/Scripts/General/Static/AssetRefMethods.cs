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




    #region General Functions

    /// <summary>
    /// Returns ero content sprite from the given folder-based asset locations.
    /// </summary>
    /// <param name="folderRootPathArg"></param>
    /// <param name="folderHPathArg"></param>
    /// <param name="fileNameArg"></param>
    /// <returns></returns>
    private static Sprite LoadFolderBasedSprite(string folderRootPathArg,
        string folderHPathArg, string fileNameArg)
    {
        // get the path to the asset file
        string filePath = Path.Combine(folderRootPathArg, folderHPathArg, fileNameArg);

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

    /// <summary>
    /// Returns the file extension for the Hcontent sprite files.
    /// </summary>
    /// <returns></returns>
    private static string GetHcontentSpriteFileExtension()
    {
        return GeneralMethods.GetPngFileExtension();
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
    /// <param name="folderRootPathArg"></param>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    private static Sprite LoadFolderBasedHcontentBackground(string folderRootPathArg, string bgIdArg)
    {
        // get the asset's file name
        string fileName = $"bg-{bgIdArg}{GetHcontentSpriteFileExtension()}";

        return LoadFolderBasedSprite(folderRootPathArg, "bg", fileName);
    }

    #endregion




    #region H-Content Activity Background Functions

    /// <summary>
    /// Returns the total number of sets related to the Hcontent activity backgrounds for the 
    /// given character. 
    /// IMPORTANT: This return value must be set manually as there is no way I'm aware 
    /// of to reliably check the number of folders in each load method.
    /// </summary>
    /// <returns></returns>
    public static int GetTotalNumberOfHcontentActivityBackgroundSets(string charIdArg)
    {
        switch (charIdArg)
        {
            case "4":
                return 1;

            case "5":
                return 1;

            case "6":
                return 1;

            default:
                // print wanring to console
                Debug.LogWarning("Problem in " +
                    "GetTotalNumberOfHcontentActivityBackgroundSets(). No case for " +
                    $"charIdArg: {charIdArg}");
                // return some invalid default value
                return 0;
        }
    }

    /// <summary>
    /// Returns an activity background sprite from the hcontent files, wherever they're installed.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <param name="bgSetArg"></param>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    public static Sprite LoadHcontentActivityBackground(string charIdArg, int bgSetArg, int bgIdArg)
    {
        // if hcontent is in game files through an asset bundle
        if (DoesAssetBundleHcontentExist())
        {
            return LoadAssetBundleHcontentActivityBackground(charIdArg, bgSetArg, bgIdArg);
        }
        // else if hcontent is in game files through the StreamingAssets
        else if (DoesStreamingAssetsHcontentExist())
        {
            return LoadStreamingAssetsHcontentActivityBackground(charIdArg, bgSetArg, bgIdArg);
        }
        // else if hcontent is in game files through the PersistentDataPath
        else if (DoesPersistentDataPathHcontentExist())
        {
            return LoadPersistentDataPathHcontentActivityBackground(charIdArg, bgSetArg, bgIdArg);
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
    /// <param name="charIdArg"></param>
    /// <param name="bgSetArg"></param>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    private static Sprite LoadAssetBundleHcontentActivityBackground(string charIdArg, 
        int bgSetArg, int bgIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "hcontent";
        string ASSET_NAME_PREFIX = GetFolderBasedHcontentActivityBackgroundHcontentFolderPath(
            charIdArg, bgSetArg) + "/bg-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + bgIdArg.ToString();

        // load and return asset
        return LoadBundleAsset<Sprite>(BUNDLE_NAME, loadAssetName);
    }

    /// <summary>
    /// Returns ero content background sprite from the StreamingAssets.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <param name="bgSetArg"></param>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    private static Sprite LoadStreamingAssetsHcontentActivityBackground(string charIdArg, 
        int bgSetArg, int bgIdArg)
    {
        return LoadFolderBasedHcontentActivityBackground(GetStreamingAssetsHcontentFolderPath(), 
            charIdArg, bgSetArg, bgIdArg);
    }

    /// <summary>
    /// Returns ero content background sprite from the PersistentDataPath.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <param name="bgSetArg"></param>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    private static Sprite LoadPersistentDataPathHcontentActivityBackground(string charIdArg,
        int bgSetArg, int bgIdArg)
    {
        return LoadFolderBasedHcontentActivityBackground(GetPersistentDataPathHcontentFolderPath(), 
            charIdArg, bgSetArg, bgIdArg);
    }

    /// <summary>
    /// Returns ero content background sprite from one of the folder-based asset locations.
    /// </summary>
    /// <param name="folderRootPathArg"></param>
    /// <param name="charIdArg"></param>
    /// <param name="bgSetArg"></param>
    /// <param name="bgIdArg"></param>
    /// <returns></returns>
    private static Sprite LoadFolderBasedHcontentActivityBackground(string folderRootPathArg, 
        string charIdArg, int bgSetArg, int bgIdArg)
    {
        // gets the hcontent activity backgorund folder path starting at the hcontent folder
        string hFolderPath = GetFolderBasedHcontentActivityBackgroundHcontentFolderPath(charIdArg, 
            bgSetArg);

        // get the asset's file name
        string fileName = $"bg-{bgIdArg}{GetHcontentSpriteFileExtension()}";

        return LoadFolderBasedSprite(folderRootPathArg, hFolderPath, fileName);
    }

    /// <summary>
    /// Returns the hcontent activity backgorund folder path starting at the hcontent folder.
    /// </summary>
    /// <param name="charIdArg"></param>
    /// <param name="bgSetArg"></param>
    /// <returns></returns>
    private static string GetFolderBasedHcontentActivityBackgroundHcontentFolderPath(string charIdArg, 
        int bgSetArg)
    {
        return $"activity-bg/char-{charIdArg}/set-{bgSetArg.ToString()}";
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

    public static Sprite LoadBundleAssetHavenActivityDialogueBackground(
        string havenActivityIdArg, TimeSlotType timeSlotArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "HavenActivityDialogueBackground";
        string ASSET_NAME_PREFIX = $"act-{havenActivityIdArg}/bg-";

        // get start of name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + GetTimeDayIdAppend(timeSlotArg);

        // load and return asset
        return LoadBundleAsset<Sprite>(BUNDLE_NAME, loadAssetName);
    }

    public static Sprite LoadBundleAssetHavenLocationActivityHubDialogueBackground(
        HavenLocation locationArg, TimeSlotType timeSlotArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "HavenLocationActivityHubDialogueBackground";
        string ASSET_NAME_PREFIX = $"loc-{locationArg.ToString()}/bg-";

        // get start of name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + GetTimeDayIdAppend(timeSlotArg);

        // load and return asset
        return LoadBundleAsset<Sprite>(BUNDLE_NAME, loadAssetName);
    }

    public static Sprite LoadBundleAssetHavenLocationEventDialogueBackground(
        string eventIdArg, TimeSlotType timeSlotArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "HavenLocationEventDialogueBackground";
        string ASSET_NAME_PREFIX = $"eve-{eventIdArg}/bg-";

        // get start of name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + GetTimeDayIdAppend(timeSlotArg);

        // load and return asset
        return LoadBundleAsset<Sprite>(BUNDLE_NAME, loadAssetName);
    }

    /// <summary>
    /// Returns the string ID that should be appeneded onto a load file asscoiated 
    /// with the given time slot.
    /// </summary>
    /// <param name="timeSlotArg"></param>
    /// <returns></returns>
    private static string GetTimeDayIdAppend(TimeSlotType timeSlotArg)
    {
        // check cases based on given time slot and add a time slot ID appropriately
        switch (timeSlotArg)
        {
            case TimeSlotType.Morning:
                return "1";

            case TimeSlotType.Afternoon:
                return "2";

            case TimeSlotType.Evening:
                return "3";

            default:
                // print warning to console
                Debug.LogWarning("In GetTimeDayIdAppend(), invalid " +
                    $"timeSlotArg: {timeSlotArg.ToString()}");
                return "0";
        }
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
