using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class NetworkConnectionCardController : MonoBehaviour
{
    #region Class Variables

    private NetworkConnection netConnection = null;

    [Tooltip("Text that displays the network address")]
    [SerializeField]
    private TextMeshProUGUI textNetAddress = null;

    #endregion




    #region Network Functions

    /// <summary>
    /// Sets up the card properties based on the given connection.
    /// </summary>
    /// <param name="netConnectionArg"></param>
    public void SetupCard(NetworkConnection netConnectionArg)
    {
        // set connection
        netConnection = netConnectionArg;

        // if given connection was null
        if (netConnectionArg == null)
        {
            // print warning to console
            Debug.LogWarning("Given NetworkConnection was null!");
            // DONT continue code
            return;
        }
        // set address text based on given connection
        textNetAddress.text = netConnectionArg.address;
    }

    #endregion


}
