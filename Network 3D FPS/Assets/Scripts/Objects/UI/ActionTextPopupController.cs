using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ActionTextPopupController : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private TextMeshPro popupText = null;

    private Rigidbody _rigBody = null;

    [Header("Popup Properties")]

    [Tooltip("How range of direction degree at which the object can decide to move in.")]
    [SerializeField]
    private float directionSpread = 10f;

    [Tooltip("How fast the object will move.")]
    [SerializeField]
    private float moveSpeed = 0.2f;

    [Tooltip("How long the object will remain active when spawned.")]
    [SerializeField]
    private float objectLifetime = 1.5f;

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // setup all vars that reference relevant components
        InitializeComponentReferences();
    }

    private void OnEnable()
    {
        // causes object to continuosuly move in a random direction
        SetMoveVelocityInRandomDirection();

        // turn off this object after lifetime is over
        DeactivateAfterLifetime();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up all vars that reference relevant components. 
    /// Call in Awake().
    /// </summary>
    private void InitializeComponentReferences()
    {
        _rigBody = GetComponent<Rigidbody>();
    }

    #endregion




    #region Setup Functions

    /// <summary>
    /// Sets up the popup visuals based on the given data.
    /// </summary>
    /// <param name="textArg"></param>
    /// <param name="textColorArg"></param>
    public void Setup(string textArg, Color textColorArg)
    {
        popupText.text = textArg;
        popupText.color = textColorArg;
    }

    #endregion




    #region Movement Functions

    /// <summary>
    /// Causes the object to continuously move in a random direction of the object based 
    /// on the set movement speed.
    /// </summary>
    private void SetMoveVelocityInRandomDirection()
    {
        //_spreadVector.x = Random.Range(-DIRECTION_SPREAD, DIRECTION_SPREAD);
        //Quaternion spreadQuat = Quaternion.Euler(_spreadVector);

        // get random spread value
        float randomSpread = Random.Range(-directionSpread, directionSpread);

        // get upward direction with some random spread
        Vector3 randomDirection = transform.up * randomSpread;

        // set this objects velocity towards the retrieved direction
        _rigBody.velocity = randomDirection * moveSpeed;
    }

    #endregion




    #region Object Functions

    /// <summary>
    /// Turn off this object after lifetime is over.
    /// </summary>
    private void DeactivateAfterLifetime()
    {
        // call internal function as coroutine
        StartCoroutine(DeactivateAfterLifetimeInternal());
    }

    private IEnumerator DeactivateAfterLifetimeInternal()
    {
        // wait lifetime span
        yield return new WaitForSeconds(objectLifetime);

        // turn off this object
        gameObject.SetActive(false);
    }

    #endregion


}
