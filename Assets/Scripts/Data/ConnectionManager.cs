using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public enum ConnectionState { NO_CONNECTION, NO_SESSION, GUEST, CONNECTED }

public static class ConnectionManager
{
    public static OnlinePlayerInfo playerInfo;

    private static ConnectionState connectionState;

    private static bool internetConnected;
    private static bool sessionConnected;


    // ### Properties ###

    public static ConnectionState ConnectionState
    {
        get { return connectionState; }
        set 
        {
            connectionState = value;
            SessionConnected = (value == ConnectionState.GUEST || value == ConnectionState.CONNECTED);
        }
    }

    public static bool InternetConnected
    {
        get { return internetConnected; }
        private set { internetConnected = value; }
    }
    public static bool SessionConnected
    {
        get { return sessionConnected; }
        set { sessionConnected = value; }
    }

    // ### Functions ###



    public static IEnumerator CheckInternetConnection()
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();

        if (request.error == null)
        {
            InternetConnected = true;
            Debug.Log("Connected to internet");
            if (!sessionConnected)
            {
                ConnectionState = ConnectionState.NO_SESSION;
            }
        }
        else
        {
            InternetConnected = false;
            ConnectionState = ConnectionState.NO_CONNECTION;
        }
    }
}
