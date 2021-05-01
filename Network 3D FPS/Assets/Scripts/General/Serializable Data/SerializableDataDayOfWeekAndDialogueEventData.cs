using System;

[System.Serializable]
public class SerializableDataDayOfWeekAndDialogueEventData
{
    #region Class Variables

    public DayOfWeek valueDow;
    public DialogueEventData valueDialogueEvent;

    #endregion




    #region Constructors

    public SerializableDataDayOfWeekAndDialogueEventData()
    {
        Setup();
    }

    public SerializableDataDayOfWeekAndDialogueEventData(DayOfWeek dowArg, DialogueEventData dialogueArg)
    {
        Setup(dowArg, dialogueArg);
    }

    public SerializableDataDayOfWeekAndDialogueEventData(SerializableDataDayOfWeekAndDialogueEventData templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        valueDow = DayOfWeek.Monday;
        valueDialogueEvent = new DialogueEventData();
    }

    private void Setup(DayOfWeek dowArg, DialogueEventData dialogueArg)
    {
        valueDow = dowArg;
        valueDialogueEvent = new DialogueEventData(dialogueArg);
    }

    private void Setup(SerializableDataDayOfWeekAndDialogueEventData templateArg)
    {
        valueDow = templateArg.valueDow;
        valueDialogueEvent = new DialogueEventData(templateArg.valueDialogueEvent);
    }

    #endregion


}
