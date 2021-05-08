
[System.Serializable]
public class SerializableDataFloatAndNetworkObjectPooler
{
    #region Class Variables

    public float value1;
    public NetworkObjectPooler value2;

    #endregion




    #region Constructors

    public SerializableDataFloatAndNetworkObjectPooler()
    {
        Setup();
    }

    public SerializableDataFloatAndNetworkObjectPooler(float value1Arg, 
        NetworkObjectPooler value2Arg)
    {
        Setup(value1Arg, value2Arg);
    }

    public SerializableDataFloatAndNetworkObjectPooler(
        SerializableDataFloatAndNetworkObjectPooler templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        value1 = 0f;
        value2 = null;
    }

    private void Setup(float value1Arg, NetworkObjectPooler value2Arg)
    {
        value1 = value1Arg;
        value2 = value2Arg;
    }

    private void Setup(SerializableDataFloatAndNetworkObjectPooler templateArg)
    {
        value1 = templateArg.value1;
        value2 = templateArg.value2;
    }

    #endregion


}
