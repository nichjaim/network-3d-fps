
[System.Serializable]
public class SerializableDataCharacterProgressionTypeAndFloat
{
    #region Class Variables

    public CharacterProgressionType typeValue;
    public float floatValue;

    #endregion




    #region Constructors

    public SerializableDataCharacterProgressionTypeAndFloat()
    {
        Setup();
    }

    public SerializableDataCharacterProgressionTypeAndFloat(
        CharacterProgressionType typeValueArg, float floatValueArg)
    {
        Setup(typeValueArg, floatValueArg);
    }

    public SerializableDataCharacterProgressionTypeAndFloat(
        SerializableDataCharacterProgressionTypeAndFloat templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        typeValue = CharacterProgressionType.None;
        floatValue = 0f;
    }

    private void Setup(CharacterProgressionType typeValueArg, float floatValueArg)
    {
        typeValue = typeValueArg;
        floatValue = floatValueArg;
    }

    private void Setup(SerializableDataCharacterProgressionTypeAndFloat templateArg)
    {
        typeValue = templateArg.typeValue;
        floatValue = templateArg.floatValue;
    }

    #endregion


}
