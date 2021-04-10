using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    [SerializeField]
    private CharacterMasterController _charMaster = null;
    public CharacterMasterController CharMaster
    {
        get { return _charMaster; }
    }


}
