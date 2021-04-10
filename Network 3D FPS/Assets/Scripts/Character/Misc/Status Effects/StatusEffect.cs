
public class StatusEffect
{
    /// <summary>
    /// Processes for when this status effect starts.
    /// </summary>
    /// <param name="effectedCharStatusArg"></param>
    public virtual void OnStatusEffectStart(CharacterStatusEffectsController 
        effectedCharStatusArg)
    {
        // IMPL in child
    }

    /// <summary>
    /// Processes for when this status effect ends.
    /// </summary>
    /// <param name="effectedCharacterArg"></param>
    public virtual void OnStatusEffectEnd(CharacterStatusEffectsController 
        effectedCharStatusArg)
    {
        // IMPL in child
    }


}
