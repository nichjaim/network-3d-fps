using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Ammo Template", menuName = "Scriptable Objects/Inventory/Inventory Ammo Template")]
public class InventoryAmmoTemplate : ScriptableObject
{
    public List<AmmoPouchTemplate> ammoPouchTemplates;
}
