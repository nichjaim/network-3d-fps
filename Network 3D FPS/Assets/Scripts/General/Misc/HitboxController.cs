using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    [SerializeField]
    private CharacterHealthController charHealth = null;
    public CharacterHealthController CharHealth
    {
        get { return charHealth; }
    }


}
