using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data Template", menuName = "Scriptable Objects/Inventory/Weapon Data Template")]
public class WeaponDataTemplate : ScriptableObject
{
    public List<string> potentialPrefixNames;

    public WeaponData template;
}
