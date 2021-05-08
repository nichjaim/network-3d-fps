using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoController : MonoBehaviour
{
    #region Class Variables

    [SerializeField]
    private Color gizmoColor = Color.white;

    [SerializeField]
    private float gizmoSize = 0.4f;

    #endregion




    #region MonoBehaviour Functions

    protected virtual void OnDrawGizmos()
    {
        //draw a gizmo at this position
        DrawGizmoOnPosition(this.transform.position);
    }

    #endregion




    #region Gizmo Functions

    //creates a edior gizmo for this object's position
    protected void DrawGizmoOnPosition(Vector3 positionArg)
    {
        //set the gizmo color to the appropriate color chosen in the editor
        Gizmos.color = gizmoColor;

        //draw a gizmo at argument position
        Gizmos.DrawSphere(positionArg, gizmoSize);
    }

    #endregion


}
