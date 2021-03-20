using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Pouch Template", menuName = "Scriptable Objects/Inventory/Ammo Pouch Template")]
public class AmmoPouchTemplate : ScriptableObject
{
    public WeaponTypeSetTemplate ammoTypeTemplate;

    public AmmoPouch template;
}
