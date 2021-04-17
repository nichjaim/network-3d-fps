
[System.Serializable]
public class SerializableDataIntAndInt
{
    #region Class Variables

    public int valueInt1;
    public int valueInt2;

    #endregion




    #region Constructors

    public SerializableDataIntAndInt()
    {
        Setup();
    }

    public SerializableDataIntAndInt(int int1Arg, int int2Arg)
    {
        Setup(int1Arg, int2Arg);
    }

    public SerializableDataIntAndInt(SerializableDataIntAndInt templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        valueInt1 = 0;
        valueInt2 = 0;
    }

    private void Setup(int int1Arg, int int2Arg)
    {
        valueInt1 = int1Arg;
        valueInt2 = int2Arg;
    }

    private void Setup(SerializableDataIntAndInt templateArg)
    {
        valueInt1 = templateArg.valueInt1;
        valueInt2 = templateArg.valueInt2;
    }

    #endregion


}
