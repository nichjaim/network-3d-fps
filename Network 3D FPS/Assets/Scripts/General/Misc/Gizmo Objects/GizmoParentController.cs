using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoParentController : GizmoController
{
    #region Override Functions

    protected override void OnDrawGizmos()
    {
        //loop through all child objects
        foreach (Transform child in transform)
        {
            //draw gizmo on iterating child object's position
            DrawGizmoOnPosition(child.position);
        }
    }

    #endregion


}
