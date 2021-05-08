
[System.Serializable]
public class LevelArenaSetData
{
    #region Class Variables

    public int setNum;
    public string setName;

    public int totalArenasInSet;

    #endregion




    #region Constructors

    public LevelArenaSetData()
    {
        Setup();
    }

    public LevelArenaSetData(LevelArenaSetData templateArg)
    {
        Setup(templateArg);
    }

    public LevelArenaSetData(LevelArenaSetDataTemplate templateArg)
    {
        Setup(templateArg.template);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        setNum = 0;
        setName = "SET_NAME";

        totalArenasInSet = 1;
    }

    private void Setup(LevelArenaSetData templateArg)
    {
        setNum = templateArg.setNum;
        setName = templateArg.setName;

        totalArenasInSet = templateArg.totalArenasInSet;
    }

    #endregion


}
