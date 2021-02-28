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




    #region H Content Functions

    /// <summary>
    /// Returns bool that denotes if ero content is in game files.
    /// </summary>
    /// <returns></returns>
    public static bool IsHcontentInstalled()
    {
        return DoesAssetBundleHcontentExist() || DoesStreamingAssetsHcontentExist();
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
    /// Returns the folder path that holds the hcontent files.
    /// </summary>
    /// <returns></returns>
    private static string GetStreamingAssetsHcontentFolderPath()
    {
        return Path.Combine(Application.streamingAssetsPath, "hcontent");
    }

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
        // if hcontent is in game files through the StreamingAssets
        else if (DoesStreamingAssetsHcontentExist())
        {
            return LoadStreamingAssetsHcontentBackground(bgIdArg);
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
        // get the asset's file name
        string fileName = $"bg-{bgIdArg}{GeneralMethods.GetPngFileExtension()}";
        // get the path to the asset file
        string filePath = Path.Combine(GetStreamingAssetsHcontentFolderPath(), "bg", fileName);

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
            Debug.LogWarning($"StreamingAssets file path does not exist: {filePath}");
            // return null sprite
            return null;
        }
    }

    #endregion




    #region Asset Bundle Character Functions

    public static CharacterDataTemplate[] LoadAllBundleAssetPlayableCharacterDataTemplate()
    {
        // initialize bundle properties
        string BUNDLE_NAME = "PlayCharDataTemp";

        //load and return asset
        return LoadAllBundleAsset<CharacterDataTemplate>(BUNDLE_NAME);
    }

    public static AnimatorOverrideController LoadBundleAssetPlayableCharacterAnimator(string charIdArg)
    {
        // initialize bundle properties
        string BUNDLE_NAME = "CharAnim";
        string ASSET_NAME_PREFIX = "playable/charanim-";

        // get name of asset to load
        string loadAssetName = ASSET_NAME_PREFIX + charIdArg;

        // load and return asset
        return LoadBundleAsset<AnimatorOverrideController>(BUNDLE_NAME, loadAssetName);
    }

    public static GameObject[] LoadAllBundleAssetCharacterEnemy()
    {
        // initialize bundle properties
        string BUNDLE_NAME = "CharEnemy";

        //load and return asset
        return LoadAllBundleAsset<GameObject>(BUNDLE_NAME);
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
        // initialize bundle properties
        string BUNDLE_NAME_PREFIX = "StructureInterior-";

        // get name of asset to load and bundle to load from
        string loadBundleName = BUNDLE_NAME_PREFIX + structureTypeArg.ToString();

        //load and return asset
        return LoadAllBundleAsset<GameObject>(loadBundleName);
    }

    #endregion




    #region Asset Bundle Animation Functions

    public static AnimatorOverrideController[] LoadAllBundleAssetAnimatorZombie()
    {
        // initialize bundle properties
        string BUNDLE_NAME = "AnimatorZombie";

        // load and return asset
        return LoadAllBundleAsset<AnimatorOverrideController>(BUNDLE_NAME);
    }

    #endregion*/


}
