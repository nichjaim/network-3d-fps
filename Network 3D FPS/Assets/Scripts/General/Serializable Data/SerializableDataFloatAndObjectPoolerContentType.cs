
[System.Serializable]
public class SerializableDataFloatAndObjectPoolerContentType
{
    #region Class Variables

    public float value1;
    public ObjectPoolerContentType value2;

    #endregion




    #region Constructors

    public SerializableDataFloatAndObjectPoolerContentType()
    {
        Setup();
    }

    public SerializableDataFloatAndObjectPoolerContentType(float value1Arg,
        ObjectPoolerContentType value2Arg)
    {
        Setup(value1Arg, value2Arg);
    }

    public SerializableDataFloatAndObjectPoolerContentType(
        SerializableDataFloatAndObjectPoolerContentType templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        value1 = 0f;
        value2 = ObjectPoolerContentType.None;
    }

    private void Setup(float value1Arg, ObjectPoolerContentType value2Arg)
    {
        value1 = value1Arg;
        value2 = value2Arg;
    }

    private void Setup(SerializableDataFloatAndObjectPoolerContentType templateArg)
    {
        value1 = templateArg.value1;
        value2 = templateArg.value2;
    }

    #endregion


}
