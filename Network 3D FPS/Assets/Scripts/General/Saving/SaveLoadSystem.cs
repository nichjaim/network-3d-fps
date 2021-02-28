using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;

public static class SaveLoadSystem
{
    #region Save Functions

    /// <summary>
    /// Saves given save file data to a file.
    /// </summary>
    /// <param name="saveDataArg"></param>
    /// <param name="saveFileNumberArg"></param>
    public static void Save(GameSaveData saveDataArg, int saveFileNumberArg)
    {
        // convert argument into json data
        string json = JsonUtility.ToJson(saveDataArg);

        // get the appopriate file path for the data being saved
        string filePath = GetSaveFilePathFromSaveFileNumber(saveFileNumberArg);

        // write json data to file
        File.WriteAllText(filePath, json);
    }

    #endregion




    #region Load Functions

    /// <summary>
    /// Returns save data from save file with save file number that matches the argument.
    /// </summary>
    /// <param name="filePathArg"></param>
    /// <returns></returns>
    private static GameSaveData LoadFromPath(string filePathArg)
    {
        // get a string from json data
        string jsonString = File.ReadAllText(filePathArg);

        // get the SaveData infromation from the json string
        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(jsonString);

        // return the save data
        return saveData;
    }

    /// <summary>
    /// Returns all save file data.
    /// </summary>
    /// <returns></returns>
    public static List<GameSaveData> LoadAll()
    {
        // initialize the return list that will hold all the save data instances
        List<GameSaveData> allSaveData = new List<GameSaveData>();

        // loop through all save files
        foreach (FileInfo iteratingFile in GetAllSaveFiles())
        {
            // add save file's save data from iterating save file
            allSaveData.Add(LoadFromPath(iteratingFile.FullName));
        }

        // return list now filled with all save file's save data
        return allSaveData;
    }

    #endregion




    #region Delete Functions

    /// <summary>
    /// Erases the save file that is associated with the given save file number.
    /// </summary>
    /// <param name="saveFileNumberArg"></param>
    public static void Delete(int saveFileNumberArg)
    {
        // get the appopriate file paths for the data being deleted
        string pathOfSaveFileToDelete = GetSaveFilePathFromSaveFileNumber(saveFileNumberArg);
        string pathOfMetaFileToDelete = pathOfSaveFileToDelete + GetMetaFileExtension();

        // if save file is actually in project
        if (File.Exists(pathOfSaveFileToDelete))
        {
            // delete the save file
            File.Delete(pathOfSaveFileToDelete);
        }
        // else save file is NOT in project
        else
        {
            // print warning to console
            Debug.LogWarning("Tried to delete save file that does not exist. Attempted " +
                "deletion directed towards file path " + pathOfSaveFileToDelete);
        }

        // if meta file is actually in project
        if (File.Exists(pathOfMetaFileToDelete))
        {
            // delete the save file's associated meta file
            File.Delete(pathOfMetaFileToDelete);
        }

        // rename any save files whose file names no longer accurately represent their file number
        RefreshSaveFileNames();
    }

    #endregion




    #region General Functions

    /// <summary>
    /// Retuns the appropriate save file path from a save file number.
    /// </summary>
    /// <param name="saveFileNumber"></param>
    /// <returns></returns>
    private static string GetSaveFilePathFromSaveFileNumber(int saveFileNumber)
    {
        // return the save file folder path combined with the appropriate save file name
        return Path.Combine(GetSaveFolderPath(), GetSaveFileNameFromSaveFileNumber(saveFileNumber));
    }

    /// <summary>
    /// Returns the appropriate file name for a save file associated with the given save file number.
    /// </summary>
    /// <param name="saveFileNumberArg"></param>
    /// <returns></returns>
    private static string GetSaveFileNameFromSaveFileNumber(int saveFileNumberArg)
    {
        // initialize the prefix of a save file's name
        string SAVE_FILE_NAME_PREFIX = "save";

        // get the string version of the save file num argument
        string saveFileNumberString = saveFileNumberArg.ToString();

        // if the save file num argument is in the SINGLE DIGITS
        if (saveFileNumberArg < 10)
        {
            /// add a zero to the beginning of the string version of the save file number to bring 
            /// it up to to naming standards with the save files with double digit file numbers
            saveFileNumberString = "0" + saveFileNumberString;
        }

        // return the proper file name of the save number
        return SAVE_FILE_NAME_PREFIX + saveFileNumberString + GetJsonExtension();
    }

    /// <summary>
    /// Retuns the folder path that holds the save files.
    /// </summary>
    /// <returns></returns>
    private static string GetSaveFolderPath()
    {
        // if should be using the StreamingAssets folder
        if (ShouldUseStreamingAssets())
        {
            // return the StreamingAssets' save folder path
            return Path.Combine(Application.streamingAssetsPath, "saves");
        }
        // else should be using the persistentDataPath folder
        else
        {
            return Application.persistentDataPath;
        }
    }

    /// <summary>
    /// Returns bool that denotes if should be using the StreamingAssets folder.
    /// </summary>
    /// <returns></returns>
    private static bool ShouldUseStreamingAssets()
    {
        // if running on android then do NOT use streaming assets
        return !(Application.platform == RuntimePlatform.Android);
    }

    /// <summary>
    /// Returns the string that should match a json file's extension.
    /// </summary>
    /// <returns></returns>
    private static string GetJsonExtension()
    {
        return ".json";
    }

    /// <summary>
    /// Returns the string that matches the file extension for unity's meta files.
    /// </summary>
    /// <returns></returns>
    private static string GetMetaFileExtension()
    {
        return ".meta";
    }

    /// <summary>
    /// Returns the number of save files that exist.
    /// </summary>
    /// <returns></returns>
    public static int GetNumberOfSaveFiles()
    {
        return GetAllSaveFiles().Count;
    }

    /// <summary>
    /// Returns a bool that denotes whether the save file folder holds the maximum number of save files allowed.
    /// </summary>
    /// <returns></returns>
    public static bool IsSaveFileFolderAtMaximumCapacity()
    {
        return GetNumberOfSaveFiles() >= GetSaveFileFolderMaximumCapacity();
    }

    /// <summary>
    /// Returns the maximum number of save files allowed.
    /// </summary>
    /// <returns></returns>
    private static int GetSaveFileFolderMaximumCapacity()
    {
        return 99;
    }

    /// <summary>
    /// Refreshes the save file names based on their save file number.
    /// </summary>
    private static void RefreshSaveFileNames()
    {
        // get all save files
        List<FileInfo> allSaveFiles = GetAllSaveFiles();

        // initialize this var for the upcoming loop
        int iteratingFileNumber;

        // loop through all save files
        for (int i = 0; i < allSaveFiles.Count; i++)
        {
            // set the iterating file number
            iteratingFileNumber = i + 1;

            // if the iterating file does NOT have the appropriate name for it's associated file number
            if (allSaveFiles[i].Name != GetSaveFileNameFromSaveFileNumber(iteratingFileNumber))
            {
                // import any saved changes 
                AssetDatabase.Refresh();

                /// rename the iterating file to the name associated with the iterating file number (NOTE: 
                /// need to use paths that are relative to the project for renaming project assets)
                AssetDatabase.RenameAsset(GetFilePathRelativeToProjectFolder(allSaveFiles[i].FullName),
                    GetSaveFileNameFromSaveFileNumber(iteratingFileNumber));

                // write all unsaved asset changes to disk
                AssetDatabase.SaveAssets();
            }
        }
    }

    /// <summary>
    /// Returns a list of all json files in the save file folder.
    /// </summary>
    /// <returns></returns>
    private static List<FileInfo> GetAllSaveFiles()
    {
        // initialize the return list that will hold the save files
        List<FileInfo> saveFiles = new List<FileInfo>();

        // get the save file folder
        DirectoryInfo saveFileFolder = new DirectoryInfo(GetSaveFolderPath());

        // loop through each file in save file folder
        foreach (FileInfo iteratingFile in saveFileFolder.GetFiles())
        {
            // if file is a json file
            if (iteratingFile.Extension == GetJsonExtension())
            {
                // add save file to save file list
                saveFiles.Add(new FileInfo(iteratingFile.FullName));
            }
        }

        // return the filled save file list
        return saveFiles;
    }

    /// <summary>
    /// Returns a file path string that is formatted to be relative to the project folder 
    /// (Ex. Assets/SteamingAssets/Saves/Save02.json)
    /// </summary>
    /// <param name="filePathArg"></param>
    /// <returns></returns>
    private static string GetFilePathRelativeToProjectFolder(string filePathArg)
    {
        // get the file path argument with any alternate directory seperator chars replaced with main directory seperator chars
        string filePathWithMainDirectorySeperators = filePathArg.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        // get the application data path with any alternate directory seperator chars replaced with main directory seperator chars
        string appDataPathWithMainDirectorySeperators = Application.dataPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        // return the file path argument in a form that has it relative to the project folder
        return "Assets" + filePathWithMainDirectorySeperators.Replace(appDataPathWithMainDirectorySeperators, string.Empty);
    }

    #endregion


}
