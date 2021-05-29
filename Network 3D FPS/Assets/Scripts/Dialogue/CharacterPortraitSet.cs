using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Portrait Set", menuName = "Scriptable Objects/Dialogue/Character Portrait Set")]
public class CharacterPortraitSet : ScriptableObject
{
    #region Class Variables

    [SerializeField]
    private CharacterDialogueExpressionType defaultExpression = CharacterDialogueExpressionType.Neutral;

    [SerializeField]
    private List<SerializableDataCharacterDialogueExpressionTypeAndSprite> expressionToPortrait = 
        new List<SerializableDataCharacterDialogueExpressionTypeAndSprite>();

    #endregion




    #region Portrait Functions

    /// <summary>
    /// Returns the portrait sprite asscoaited with the given expression.
    /// </summary>
    /// <param name="expressionArg"></param>
    /// <returns></returns>
    public Sprite GetAppropriatePortrait(CharacterDialogueExpressionType expressionArg)
    {
        // get matching data based on the given expression
        SerializableDataCharacterDialogueExpressionTypeAndSprite matchingData = 
            GetSerialDataFromExpression(expressionArg);

        // if no matching data was found
        if (matchingData == null)
        {
            // get matching data based on the default expression
            matchingData = GetSerialDataFromExpression(defaultExpression);
        }

        // if matching data WAS found
        if (matchingData != null)
        {
            // return the data's sprite
            return matchingData.valueSprite;
        }
        // else NO match was ever found
        else
        {
            // return a NULL sprite
            return null;
        }
    }

    private SerializableDataCharacterDialogueExpressionTypeAndSprite GetSerialDataFromExpression(
        CharacterDialogueExpressionType expressionArg)
    {
        return expressionToPortrait.FirstOrDefault(iterData => iterData.valueExpression == expressionArg);
    }

    #endregion


}
