
[System.Serializable]
public class SerializableDataStringAndHavenProgressionStatsSet
{
    #region Class Variables

    public string value1;
    public HavenProgressionStatsSet value2;

    #endregion




    #region Constructors

    public SerializableDataStringAndHavenProgressionStatsSet()
    {
        Setup();
    }

    public SerializableDataStringAndHavenProgressionStatsSet(string value1Arg,
        HavenProgressionStatsSet value2Arg)
    {
        Setup(value1Arg, value2Arg);
    }

    public SerializableDataStringAndHavenProgressionStatsSet(
        SerializableDataStringAndHavenProgressionStatsSet templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        value1 = string.Empty;
        value2 = new HavenProgressionStatsSet();
    }

    private void Setup(string value1Arg, HavenProgressionStatsSet value2Arg)
    {
        value1 = value1Arg;
        value2 = new HavenProgressionStatsSet(value2Arg);
    }

    private void Setup(SerializableDataStringAndHavenProgressionStatsSet templateArg)
    {
        value1 = templateArg.value1;
        value2 = new HavenProgressionStatsSet(templateArg.value2);
    }

    #endregion


}
