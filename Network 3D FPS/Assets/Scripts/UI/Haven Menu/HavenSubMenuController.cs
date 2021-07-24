using Nichjaim.MasterSubMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HavenSubMenuController : SubMenuController
{
    #region Class Variables

    protected DialogueGameCoordinator _dialgGameCoord = null;

    [Header("Haven Sub Menu References")]

    [SerializeField]
    private HavenLocation havenSpot = HavenLocation.None;

    [SerializeField]
    private Sprite bgSprite = null;

    [SerializeField]
    private List<SerializableDataHavenLocationEventTemplateAndGameObject> 
        havenLocationEventTempToEventObject = 
        new List<SerializableDataHavenLocationEventTemplateAndGameObject>();

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup vars that hold reference to components
        InitializeComponentReferences();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up vars that hold reference to component. 
    /// Call in Start(), to give time for singletons to instantiate.
    /// </summary>
    private void InitializeComponentReferences()
    {
        _dialgGameCoord = ((HavenMenuController)menuMaster)._UiManager.DialgGameCoordr;
    }

    #endregion




    #region Override Functions

    public override void OnMenuOpen()
    {
        base.OnMenuOpen();

        ((HavenMenuController)menuMaster).BackgroundTransition(bgSprite, 1f);

        /// activate/deactivate the menu's haven location event objects based 
        /// on if those events are active today.
        RefreshHavenLocationEventObjects();
    }

    #endregion




    #region Haven Menu Functions

    /// <summary>
    /// Activates/Deactivates the menu's haven location event objects based 
    /// on if those events are active today.
    /// </summary>
    private void RefreshHavenLocationEventObjects()
    {
        // initialize var for upcoming loop
        bool isEventActive;

        // loop through all location event entries
        foreach (SerializableDataHavenLocationEventTemplateAndGameObject 
            iterData in havenLocationEventTempToEventObject)
        {
            // get whether the iterating event is currently active
            isEventActive = ((HavenMenuController)menuMaster)._GameManager.
                IsHavenLocationEventActiveToday(iterData.valueTemplate.template);

            //activate/deactivate the iterating event object based on if the event is active or not
            iterData.valueObject.SetActive(isEventActive);
        }
    }

    /// <summary>
    /// Starts dialogue conversation associated with given have location event.
    /// </summary>
    /// <param name="templateArg"></param>
    public void PlayHavenLocationEventDialogue(HavenLocationEventTemplate templateArg)
    {
        // starts dialogue conversation associated with given have location event
        _dialgGameCoord.PlayHavenLocationEventDialogue(new HavenLocationEvent(templateArg));

        /// activate/deactivate the menu's haven location event objects based 
        /// on if those events are active today.
        RefreshHavenLocationEventObjects();
    }

    #endregion


}
