using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Slot Data Template", menuName = "Scriptable Objects/Inventory/Weapon Slot Data Template")]
public class WeaponSlotDataTemplate : ScriptableObject
{
    public WeaponDataTemplate weaponDataTemplate;
    public WeaponTypeSetTemplate weaponTypeSetTemplate;
}
