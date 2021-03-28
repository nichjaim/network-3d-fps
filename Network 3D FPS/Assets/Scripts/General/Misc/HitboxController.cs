using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    #region Class Variables

    [SerializeField]
    private CharacterHealthController charHealth = null;
    public CharacterHealthController CharHealth
    {
        get { return charHealth; }
    }

    #endregion


}
