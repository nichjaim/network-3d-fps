using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawerController : MonoBehaviour
{
    #region Class Variables

    [SerializeField]
    private LineRenderer _lineRenderer = null;

    #endregion




    #region MonoBehaviour Functions

    private void OnDisable()
    {
        // remove all drawn lines
        RemoveLines();
    }

    #endregion




    #region Line Drawing Functions

    /// <summary>
    /// Draw a line with the given vertex points.
    /// </summary>
    /// <param name="vertexPositionsArg"></param>
    private void DrawLine(Vector3[] vertexPositionsArg)
    {
        _lineRenderer.positionCount = vertexPositionsArg.Length;
        _lineRenderer.SetPositions(vertexPositionsArg);
    }

    private void DrawLine(Vector3[] vertexPositionsArg, float lineLifetimeArg)
    {
        // draw the line
        DrawLine(vertexPositionsArg);

        // remove line after time delay
        RemoveLinesDelayed(lineLifetimeArg);
    }

    public void DrawLine(Vector3 startPosArg, Vector3 endPosArg)
    {
        Vector3[] vertexList = new Vector3[2];
        vertexList[0] = startPosArg;
        vertexList[1] = endPosArg;

        DrawLine(vertexList);
    }

    public void DrawLine(Vector3 startPosArg, Vector3 endPosArg, float lineLifetimeArg)
    {
        DrawLine(startPosArg, endPosArg);

        RemoveLinesDelayed(lineLifetimeArg);
    }

    #endregion




    #region Line Remove Functions

    /// <summary>
    /// Removes all drawn lines after a time delay.
    /// </summary>
    /// <param name="timeArg"></param>
    private void RemoveLinesDelayed(float timeArg)
    {
        // call internal function as a coroutine
        StartCoroutine(RemoveLinesDelayedInternal(timeArg));
    }

    private IEnumerator RemoveLinesDelayedInternal(float timeArg)
    {
        // wait for the given amount of time
        yield return new WaitForSeconds(timeArg);

        // remove all drawn lines
        RemoveLines();
    }

    /// <summary>
    /// Remove all drawn lines.
    /// </summary>
    private void RemoveLines()
    {
        _lineRenderer.positionCount = 0;
    }

    #endregion


}
