
[System.Serializable]
public class SerializableDataFactionDataAndInt
{
    #region Class Variables

    public FactionData valueFactionData;
    public int valueInt;

    #endregion




    #region Constructors

    public SerializableDataFactionDataAndInt()
    {
        Setup();
    }

    public SerializableDataFactionDataAndInt(FactionData factionDataArg, int intArg)
    {
        Setup(factionDataArg, intArg);
    }

    public SerializableDataFactionDataAndInt(SerializableDataFactionDataAndInt templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        valueFactionData = new FactionData();
        valueInt = 0;
    }

    private void Setup(FactionData factionDataArg, int intArg)
    {
        valueFactionData = new FactionData(factionDataArg);
        valueInt = intArg;
    }

    private void Setup(SerializableDataFactionDataAndInt templateArg)
    {
        valueFactionData = new FactionData(templateArg.valueFactionData);
        valueInt = templateArg.valueInt;
    }

    #endregion


}
