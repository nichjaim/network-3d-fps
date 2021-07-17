using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HavenMenuSchedulePopupController : MonoBehaviour
{
    #region Class Variables

    private GameManager _gameManager = null;

    [Header("Object References")]

    [Tooltip("The object that holds all the schedule objects")]
    [SerializeField]
    private GameObject menuMasterObject = null;

    [Tooltip("The content parent object for the scroll view that holds the activity entries")]
    [SerializeField]
    private GameObject activitiesScrollViewContentParent = null;

    [Tooltip("The prefab for the activity entry UI object")]
    [SerializeField]
    private GameObject activityEntryUiPrefab = null;

    [Header("Stat Reference Lists")]

    [SerializeField]
    private List<TextMeshProUGUI> waifu1HavenStatCounterTexts = new List<TextMeshProUGUI>();
    [SerializeField]
    private List<TextMeshProUGUI> waifu2HavenStatCounterTexts = new List<TextMeshProUGUI>();
    [SerializeField]
    private List<TextMeshProUGUI> waifu3HavenStatCounterTexts = new List<TextMeshProUGUI>();

    // the objects created for the activity entries, kept track of for easy deletion
    private List<GameObject> createdActivityEntries = new List<GameObject>();

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup reference vars to their respective singletons
        InitializeSingetonReferences();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up reference vars to their respective singletons. 
    /// Call in Start() so as to give time for singleton instances to actually instantiate.
    /// </summary>
    private void InitializeSingetonReferences()
    {
        _gameManager = GameManager.Instance;
    }

    #endregion




    #region General Menu Functions

    /// <summary>
    /// Turns ON the menu.
    /// </summary>
    public void MenuActivate()
    {
        SetMenuActivation(true);

        // setup all the relevant menu components
        RefreshMenu();
    }

    /// <summary>
    /// Turns OFF the menu.
    /// </summary>
    public void MenuDeactivate()
    {
        SetMenuActivation(false);
    }

    /// <summary>
    /// Turns the menu on/off.
    /// </summary>
    /// <param name="activeArg"></param>
    private void SetMenuActivation(bool activeArg)
    {
        menuMasterObject.SetActive(activeArg);
    }

    #endregion




    #region Schedule Menu Functions

    /// <summary>
    /// Sets up all the relevant menu components.
    /// </summary>
    private void RefreshMenu()
    {
        RefreshHavenStatText();
        RefreshActivityEntries();
    }

    /// <summary>
    /// Sets up the haven stat text component's text.
    /// </summary>
    private void RefreshHavenStatText()
    {
        // initialize these vars for upcoming loop
        List<List<TextMeshProUGUI>> partyMembersToHavenStatCounters = 
            GetWaifusToHavenStatCounterTexts();
        List<SerializableDataStringAndHavenProgressionStatsSet> serialDataList = 
            _gameManager.HavenData.havenProgression.charIdToProgressionStats;
        List<HavenProgressionStatData> statDataList = new List<HavenProgressionStatData>();

        // loop through all party memebrs haven progress
        for (int i = 0; i < serialDataList.Count; i++)
        {
            // if there is not a schedule line text for the iterating party member
            if (partyMembersToHavenStatCounters.Count <= i)
            {
                // print warning to log
                Debug.LogWarning("More party members then schedule menu haven stat text lines!");

                // DONT continue loop
                break;
            }

            // get the iterating party memebr's haven stats
            statDataList = serialDataList[i].value2.statTypesToStatData;

            // loop through all haven stats of iterating party member
            for (int y = 0; y < statDataList.Count; y++)
            {
                // if there is not a haven stat text for iterating haven stat
                if (statDataList.Count <= y)
                {
                    // print warning to log
                    Debug.LogWarning("More haven stats then haven stat text!");

                    // DONT continue loop
                    break;
                }

                // set iterating party member's iterating haven stat's text based on the activity points
                partyMembersToHavenStatCounters[i][y].text = statDataList[y].activityPoints.ToString();
            }
        }
    }

    /// <summary>
    /// Returns list of all the waifu haven stat counter text lists.
    /// </summary>
    /// <returns></returns>
    private List<List<TextMeshProUGUI>> GetWaifusToHavenStatCounterTexts()
    {
        List<List<TextMeshProUGUI>> returnList = new List<List<TextMeshProUGUI>>();

        returnList.Add(waifu1HavenStatCounterTexts);
        returnList.Add(waifu2HavenStatCounterTexts);
        returnList.Add(waifu3HavenStatCounterTexts);

        return returnList;
    }

    /// <summary>
    /// Sets up all haven activity entries.
    /// </summary>
    private void RefreshActivityEntries()
    {
        // destory all created activity entry objects and resets list
        DestoryAllCreatedActivityEntries();

        // initialize vars for upcoming loop
        GameObject iterCreatedObj;
        ActivityUiEntryController activityEntry;

        // loop through all currently avaiable activities
        foreach (HavenActivityData iterActivity in 
            _gameManager.HavenData.activityPlanning.availableActivities)
        {
            // instantiate activity entry object
            iterCreatedObj = Instantiate(activityEntryUiPrefab, activitiesScrollViewContentParent.transform);

            // add created object to created list for quick easy destroying later
            createdActivityEntries.Add(iterCreatedObj);

            // get activity UI entry component from created object
            activityEntry = iterCreatedObj.GetComponent<ActivityUiEntryController>();

            // if there was an acivity entry component on created object
            if (activityEntry)
            {
                // Setup all the entry components based on iterating activity
                activityEntry.SetupActivityEntry(iterActivity);
            }
            // else needed component was NOT found
            else
            {
                // print warning to console
                Debug.LogWarning("Created activity UI entry object did NOT have the appropriate " +
                    "controller attached to it!");
            }
        }
    }

    /// <summary>
    /// Destory all created activity entry objects and resets list.
    /// </summary>
    private void DestoryAllCreatedActivityEntries()
    {
        // loop through all created activity entry objects
        foreach (GameObject iterObj in createdActivityEntries)
        {
            // destroy iterating entry object
            Destroy(iterObj);
        }

        // reset list to fresh empty list
        createdActivityEntries = new List<GameObject>();
    }

    #endregion


}
