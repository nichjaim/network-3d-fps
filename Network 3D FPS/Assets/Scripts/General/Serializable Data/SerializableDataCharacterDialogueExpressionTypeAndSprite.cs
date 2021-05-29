using UnityEngine;

[System.Serializable]
public class SerializableDataCharacterDialogueExpressionTypeAndSprite
{
    #region Class Variables

    public CharacterDialogueExpressionType valueExpression;
    public Sprite valueSprite;

    #endregion




    #region Constructors

    public SerializableDataCharacterDialogueExpressionTypeAndSprite()
    {
        Setup();
    }

    public SerializableDataCharacterDialogueExpressionTypeAndSprite(
        CharacterDialogueExpressionType expressionArg, Sprite spriteArg)
    {
        Setup(expressionArg, spriteArg);
    }

    public SerializableDataCharacterDialogueExpressionTypeAndSprite(
        SerializableDataCharacterDialogueExpressionTypeAndSprite templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        valueExpression = CharacterDialogueExpressionType.Neutral;
        valueSprite = null;
    }

    private void Setup(CharacterDialogueExpressionType expressionArg, Sprite spriteArg)
    {
        valueExpression = expressionArg;
        valueSprite = spriteArg;
    }

    private void Setup(SerializableDataCharacterDialogueExpressionTypeAndSprite templateArg)
    {
        valueExpression = templateArg.valueExpression;
        valueSprite = templateArg.valueSprite;
    }

    #endregion


}