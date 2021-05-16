
[System.Serializable]
public class SerializableDataObjectPoolerContentTypeAndNetworkObjectPooler
{
    #region Class Variables

    public ObjectPoolerContentType value1;
    public NetworkObjectPooler value2;

    #endregion




    #region Constructors

    public SerializableDataObjectPoolerContentTypeAndNetworkObjectPooler()
    {
        Setup();
    }

    public SerializableDataObjectPoolerContentTypeAndNetworkObjectPooler(
        ObjectPoolerContentType value1Arg, NetworkObjectPooler value2Arg)
    {
        Setup(value1Arg, value2Arg);
    }

    public SerializableDataObjectPoolerContentTypeAndNetworkObjectPooler(
        SerializableDataObjectPoolerContentTypeAndNetworkObjectPooler templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        value1 = ObjectPoolerContentType.None;
        value2 = null;
    }

    private void Setup(ObjectPoolerContentType value1Arg, NetworkObjectPooler value2Arg)
    {
        value1 = value1Arg;
        value2 = value2Arg;
    }

    private void Setup(SerializableDataObjectPoolerContentTypeAndNetworkObjectPooler templateArg)
    {
        value1 = templateArg.value1;
        value2 = templateArg.value2;
    }

    #endregion


}
