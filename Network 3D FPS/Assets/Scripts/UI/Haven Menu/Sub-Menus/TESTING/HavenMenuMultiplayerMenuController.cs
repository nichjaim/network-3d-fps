using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nichjaim.MasterSubMenu;
using UnityEngine.UI;
using Mirror;
using TMPro;
using MoreMountains.Tools;

public class HavenMenuMultiplayerMenuController : HavenSubMenuController, 
    MMEventListener<ServerAddedPlayerEvent>, MMEventListener<ServerDisconnectedPlayerEvent>,
    MMEventListener<ExitGameSessionEvent>
{
    #region Class Variables

    [Header("Party Slot Properties")]

    [Tooltip("Object that parents all the party slot button objects.")]
    [SerializeField]
    private GameObject partySlotButtonsParent = null;
    private List<Image> partySlotButtonImages = new List<Image>();

    // the party order slot that is currently selected by player
    private int selectedPartySlot = 0;

    [Header("Host Button Properties")]

    [Tooltip("TMP Text of the host button.")]
    [SerializeField]
    private TextMeshProUGUI textHostButton = null;
    [Tooltip("Host button gameobject.")]
    [SerializeField]
    private GameObject hostButton = null;

    // can other players connect to host
    private bool areConnectionsOpen = false;

    [Header("Connection Display Properties")]

    [Tooltip("Object of scroll view that parents the content.")]
    [SerializeField]
    private GameObject scrollViewJoinedPlayerContentParent = null;
    [Tooltip("Prefab of the network connection card object.")]
    [SerializeField]
    private GameObject networkConnectionCardPrefab = null;
    private List<GameObject> createdNetConnectionCards = new List<GameObject>();

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // setup the list of party slot button images
        InitializePartySlotButtonImages();
        // setup the actions for the party slot buttons
        InitializePartySlotButtonActions();
    }

    private void OnEnable()
    {
        // starts listening for all relevant events
        StartAllEventListening();
    }

    private void OnDisable()
    {
        // stops listening for all relevant events
        StopAllEventListening();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up the list of party slot button images. 
    /// Call in Awake().
    /// </summary>
    private void InitializePartySlotButtonImages()
    {
        // reset button image list to fresh empty list
        partySlotButtonImages = new List<Image>();

        // initialize var for upcoming loop
        Image buttonImage;
        // loop through all party slot button parent's child objects
        foreach (Transform child in partySlotButtonsParent.transform)
        {
            // get image comp from iterating button object
            buttonImage = child.GetComponent<Image>();
            // if found a image comp
            if (buttonImage != null)
            {
                // add image to image list
                partySlotButtonImages.Add(buttonImage);
            }
            // else NO image comp on iterating object
            else
            {
                // print wanring to console
                Debug.LogWarning("Party slot button has no Image component!");
            }
        }
    }

    /// <summary>
    /// Sets up the actions for the party slot buttons. 
    /// Call in Awake().
    /// </summary>
    private void InitializePartySlotButtonActions()
    {
        // initialize vars for upcoming loop
        Button buttonComp;
        int buttonSlotNumber = 1;
        // loop through all party slot button parent's child objects
        foreach (Transform child in partySlotButtonsParent.transform)
        {
            // get button comp from iterating button object
            buttonComp = child.GetComponent<Button>();
            // if found a button comp
            if (buttonComp != null)
            {
                /// due to onClick listener weirdness, need to create a new var for 
                /// the onClick function argument for correct value to be passed
                int tempInt = buttonSlotNumber;
                // add action to button comp
                buttonComp.onClick.AddListener(() => ButtonPressPartySlot(tempInt) );
            }
            // else NO button comp on iterating object
            else
            {
                // print wanring to console
                Debug.LogWarning("Party slot button has no Button component!");
            }

            // increment slot number
            buttonSlotNumber++;
        }
    }

    #endregion




    #region Override Functions

    public override void OnMenuOpen()
    {
        base.OnMenuOpen();

        // de-select any party slots
        selectedPartySlot = 0;
        // setup the image color of the party slot buttons
        RefreshPartySlotImageColoring();
        // setup the pictures of the party slot buttons
        RefreshPartySlotImageContent();

        // denote connections are closed
        areConnectionsOpen = false;
        // setup the text for the host button
        RefreshHostButtonText();

        // setup the content for the joined players scroll view
        RefreshScrollViewJoinedPlayers();
    }

    public override void OnMenuClose()
    {
        base.OnMenuClose();

        // stop any new players from joining session
        StopNewConnections();
    }

    #endregion




    #region Menu Functions

    /// <summary>
    /// Sets up the image color of the party slot buttons. 
    /// Use when party slot was selected by user.
    /// </summary>
    private void RefreshPartySlotImageColoring()
    {
        // loop through all party slot images
        for (int i = 0; i < partySlotButtonImages.Count; i++)
        {
            // if iterating party slot is selected
            if (selectedPartySlot == i + 1)
            {
                // change slot image's color to denote selection
                partySlotButtonImages[i].color = Color.yellow;
            }
            // else iterating party slot is de-selected
            else
            {
                // change slot image's color to denote de-selection
                partySlotButtonImages[i].color = Color.white;
            }
        }
    }

    /// <summary>
    /// Sets up the pictures of the party slot buttons. 
    /// Use when party characters or party order has been changed.
    /// </summary>
    private void RefreshPartySlotImageContent()
    {
        // initialize vars for upcoming loop
        CharacterData charData;
        string charId;

        // loop through all party slot images
        for (int i = 0; i < partySlotButtonImages.Count; i++)
        {
            // get character in iterating party slot
            charData = GameManager.Instance.GetPartyCharacterInPartySlot(i + 1);

            // if got a character data
            if (charData != null)
            {
                // get ID of character
                charId = charData.characterInfo.characterId;
            }
            // else NO valid character data was returned
            else
            {
                // print warning to console
                Debug.LogWarning("Returned char data was null!");
                // set ID to some default string
                charId = string.Empty;
            }

            // set iterating slot image's picture to char's icon
            partySlotButtonImages[i].sprite = AssetRefMethods.LoadBundleAssetCharacterIcon(charId);
        }
    }

    /// <summary>
    /// Sets up the text for the host button.
    /// </summary>
    private void RefreshHostButtonText()
    {
        // if user is hosting an online game session
        if (IsHostingGame())
        {
            // if connections are OPEN
            if (areConnectionsOpen)
            {
                textHostButton.text = "Halt Joins";
            }
            // else connections are CLOSED
            else
            {
                textHostButton.text = "Open Joins";
            }
        }
        // else user is currently offline
        else
        {
            textHostButton.text = "Host Game";
        }
    }

    /// <summary>
    /// Sets up the content for the joined players scroll view.
    /// </summary>
    private void RefreshScrollViewJoinedPlayers()
    {
        // call internal function as coroutine
        StartCoroutine(RefreshScrollViewJoinedPlayersInternal());
    }

    private IEnumerator RefreshScrollViewJoinedPlayersInternal()
    {
        yield return new WaitForEndOfFrame();

        // destory all created net connected card objects
        DestroyAllCreatedNetConnectionCards();

        // initialize vars for upcoming loop
        NetworkIdentity networkId;
        GameObject netConnectionCardObj;
        NetworkConnectionCardController netConnectionCard;
        // loop through all connected players
        foreach (PlayerCharacterMasterController iterPlayer in
            ((NetworkManagerCustom)NetworkManager.singleton).connectedPlayers)
        {
            // get iterating player's network ID comp
            networkId = iterPlayer.GetComponent<NetworkIdentity>();
            // if comp found
            if (networkId != null)
            {
                // create net connnection card object and parent it to scroll view content parent
                netConnectionCardObj = Instantiate(networkConnectionCardPrefab,
                    scrollViewJoinedPlayerContentParent.transform);
                // add created object to list for easy clean up later
                createdNetConnectionCards.Add(netConnectionCardObj);

                // get net connnection card comp
                netConnectionCard = netConnectionCardObj.GetComponent<NetworkConnectionCardController>();
                // if comp found
                if (netConnectionCard != null)
                {
                    // setup card
                    netConnectionCard.SetupCard(networkId.connectionToClient);
                }
                // else comp NOT found
                else
                {
                    // print warning to console
                    Debug.LogWarning($"created net connection card object does NOT have a " +
                        $"net connection card component");
                }
            }
            // else no component did NOT exist on iterating object
            else
            {
                // print warning to console
                Debug.LogWarning($"Player object <{iterPlayer.name}> does NOT have a " +
                    "NetworkIdentity component!");
            }
        }
    }

    /// <summary>
    /// Destory all created net connected card objects.
    /// </summary>
    private void DestroyAllCreatedNetConnectionCards()
    {
        // loop through all created net connection cards
        foreach (GameObject iterObj in createdNetConnectionCards)
        {
            // destroy iterating card objects
            Destroy(iterObj);
        }

        // reset list to fresh empty list
        createdNetConnectionCards = new List<GameObject>();
    }

    #endregion




    #region Network Functions

    /// <summary>
    /// Returns bool that denotes if user is hosting an online game session.
    /// </summary>
    /// <returns></returns>
    private bool IsHostingGame()
    {
        return ((NetworkManagerCustom)NetworkManager.singleton).IsHostingOnlineSession();
    }

    /// <summary>
    /// Starts online game hosting.
    /// </summary>
    private void StartOnlineHost()
    {
        ((NetworkManagerCustom)NetworkManager.singleton).StartHostingOnlineSession();
    }

    /// <summary>
    /// Stop any new players from joining session.
    /// </summary>
    private void StopNewConnections()
    {
        ((NetworkManagerCustom)NetworkManager.singleton).CloseToNewConnections();
    }

    /// <summary>
    /// Allows any new players to join session.
    /// </summary>
    private void AllowNewConnections()
    {
        ((NetworkManagerCustom)NetworkManager.singleton).OpenToNewConnectionsDefault();
    }

    #endregion




    #region Button Press Functions

    /// <summary>
    /// NOTE: Attached through code, do NOT try to attach through editor.
    /// </summary>
    /// <param name="slotNumberArg"></param>
    private void ButtonPressPartySlot(int slotNumberArg)
    {
        // if no party slot already selected
        if (selectedPartySlot <= 0)
        {
            // select given party slot
            selectedPartySlot = slotNumberArg;
        }
        // else if the selected slot was the one that was ALREADY selected
        else if (selectedPartySlot == slotNumberArg)
        {
            // de-select the party slot
            selectedPartySlot = 0;
        }
        // else selected a party slot while another was also selected
        else
        {
            // swap party order between the previously selected slot and newly selected slot
            GameManager.Instance.SwapPartyOrderSlots(selectedPartySlot, slotNumberArg);
            // de-select the party slot
            selectedPartySlot = 0;
            // setup the pictures of the party slot buttons
            RefreshPartySlotImageContent();
        }

        // setup the image color of the party slot buttons
        RefreshPartySlotImageColoring();
    }

    public void ButtonPressBack()
    {
        // switch to haven menu's main menu
        ((HavenMenuController)menuMaster).SwitchToStartingMenu();
    }

    public void ButtonPressHost()
    {
        // if user is hosting an online game session
        if (IsHostingGame())
        {
            // if connections are OPEN
            if (areConnectionsOpen)
            {
                // close connections
                StopNewConnections();
            }
            // else connections are CLOSED
            else
            {
                // open connections
                AllowNewConnections();
            }
        }
        // else user is currently offline
        else
        {
            // starts online game hosting
            StartOnlineHost();
        }

        // switch whether connects are open or not
        areConnectionsOpen = !areConnectionsOpen;
        // setup the text for the host button
        RefreshHostButtonText();

        // turn off host button object
        hostButton.SetActive(false); // POSSIBLY TEMP CODE FOR NOW
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<ServerAddedPlayerEvent>();
        this.MMEventStartListening<ServerDisconnectedPlayerEvent>();
        this.MMEventStartListening<ExitGameSessionEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<ServerAddedPlayerEvent>();
        this.MMEventStopListening<ServerDisconnectedPlayerEvent>();
        this.MMEventStopListening<ExitGameSessionEvent>();
    }

    public void OnMMEvent(ServerAddedPlayerEvent eventType)
    {
        // setup the content for the joined players scroll view
        RefreshScrollViewJoinedPlayers();
    }

    public void OnMMEvent(ServerDisconnectedPlayerEvent eventType)
    {
        // setup the content for the joined players scroll view
        RefreshScrollViewJoinedPlayers();
    }

    public void OnMMEvent(ExitGameSessionEvent eventType)
    {
        // turn on host button object
        hostButton.SetActive(true); // POSSIBLY TEMP CODE FOR NOW
    }

    #endregion


}
