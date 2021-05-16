using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Event Database", menuName = "Scriptable Objects/Dialogue/Dialogue Event Database")]
public class DialogueEventDatabase : ScriptableObject
{
    //public List<DialogueEventSet> dialogueSets;
    [MMReorderableAttribute(null, "Set", null)]
    public ReorderableListDialogueEventSet dialogueSets;
}
