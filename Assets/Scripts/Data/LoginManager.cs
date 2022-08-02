using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using LootLocker.Requests;


public struct OnlinePlayerInfo
{
    public string email;
    public int id;
    public string pseudo;
}


public class LoginManager : MonoBehaviour
{
    private MenuMainManager main;


    [Header("UI components")]
    [Header("Home Screen")]
    [SerializeField] private Button homeLoginButton;
    [Space]
    [SerializeField] private GameObject waitPopup;


    [Header("Base Screen")]
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button newUserButton;
    [SerializeField] private Button loginAsGuestButton;
    [SerializeField] private Button disconnectButton;
    

    [Header("Login Screen")]
    [SerializeField] private TMP_InputField loginEmailIF;
    [SerializeField] private TMP_InputField loginPasswordIF;


    [Header("NewUser Screen")]
    [SerializeField] private TMP_InputField newUserEmailIF;
    [SerializeField] private TMP_InputField newUserPseudoIF;
    [SerializeField] private TMP_InputField newUserPasswordIF;

    [Header("Result Screen")]
    [SerializeField] private GameObject resultScreen;
    [SerializeField] private TextMeshProUGUI resultText;



    // ### Properties ###

    public ConnectionState State
    {
        get { return ConnectionManager.ConnectionState; }
        set
        {
            ConnectionManager.ConnectionState = value;
            ActuStateText();
            if (value == ConnectionState.GUEST || value == ConnectionState.CONNECTED)
            {
                
            }
            homeLoginButton.gameObject.SetActive(value == ConnectionState.NO_SESSION);
            StartCoroutine(main.DataManager.LoadDatas());
        }
    }


    // ### Built-in ###

    private void Awake()
    {
        main = GetComponent<MenuMainManager>();
    }

    private void Start()
    {
        //InitConnectionManager.PlayerInfo();
        StartCoroutine(AutoLogin());
    }


    // ### Functions ###

    //private void InitConnectionManager.PlayerInfo()
    //{
    //    ConnectionManager.PlayerInfo.email = "";
    //    ConnectionManager.PlayerInfo.id = 0;
    //    ConnectionManager.PlayerInfo.pseudo = "";
    //}

    private void Result(string result)
    {
        resultScreen.SetActive(true);
        resultText.text = result;
    }

    public void CheckInternetConnection()
    {
        StartCoroutine(CheckInternetConnectionCR());
    }
    private IEnumerator CheckInternetConnectionCR()
    {
        waitPopup.SetActive(true);

        yield return StartCoroutine(ConnectionManager.CheckInternetConnection());

        waitPopup.SetActive(false);

        if (!ConnectionManager.InternetConnected)
        {
            loginButton.interactable = false;
            newUserButton.interactable = false;
            loginAsGuestButton.interactable = false;
            disconnectButton.interactable = false;
        }
        else
        {
            loginButton.interactable = true;
            newUserButton.interactable = true;
            loginAsGuestButton.interactable = true;
        }

        ActuDisconnectButton();
    }

    public void ActuDisconnectButton()
    {
        disconnectButton.interactable = (State == ConnectionState.CONNECTED || State == ConnectionState.GUEST);
    }


    private IEnumerator AutoLogin()
    {
        yield return StartCoroutine(CheckInternetConnectionCR());

        if (ConnectionManager.InternetConnected)
        {
            if (ConnectionManager.ConnectionState == ConnectionState.NO_SESSION)
            {
                LootLockerSDKManager.CheckWhiteLabelSession(response =>
                {
                    if (response == false)
                    {
                        Debug.Log("No white label active session");
                        State = ConnectionState.NO_SESSION;
                    }
                    else
                    {
                    // Session is valid, start game session
                        LootLockerSDKManager.StartWhiteLabelSession((response) =>
                        {
                            if (response.success)
                            {
                                ConnectionManager.playerInfo.id = response.player_id;

                                Debug.Log("Session started successfully");
                                LootLockerSDKManager.GetPlayerName((response) =>
                                {
                                    if (response.success)
                                    {
                                        ConnectionManager.playerInfo.pseudo = response.name;
                                    }
                                    State = ConnectionState.CONNECTED;
                                });
                            }
                            else
                            {
                                Debug.Log("Starting session error");
                                return;
                            }

                        });

                    }
                });
            }
        }
        else
        {
            State = ConnectionState.NO_CONNECTION;
        }
    }

    public void Login()
    {
        DisconnectPreviousSession();

        string email = loginEmailIF.text;
        string password = loginPasswordIF.text;
        LootLockerSDKManager.WhiteLabelLogin(email, password, true, response =>
        {
            if (!response.success)
            {
                Debug.Log("error while logging in");
                Result("Error while logging in, try again");
                return;
            }
            else
            {
                Debug.Log("Player was logged in succesfully");

                LootLockerSDKManager.StartWhiteLabelSession((response) =>
                {
                    if (!response.success)
                    {
                        Debug.Log("error starting LootLocker session");
                        Result("Error starting the session, try again");
                        return;
                    }
                    else
                    {
                        ConnectionManager.playerInfo.id = response.player_id;
                        ConnectionManager.playerInfo.email = email;

                        Result("You are now connected !");
                        LootLockerSDKManager.GetPlayerName((response) =>
                        {
                            if (response.success)
                            {
                                ConnectionManager.playerInfo.pseudo = response.name;
                            }
                            State = ConnectionState.CONNECTED;
                        });
                    }
                });
            }
        });
    }

    public void ResetPassword()
    {
        string email = loginEmailIF.text;
        LootLockerSDKManager.WhiteLabelRequestPassword(email, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("error requesting password reset");
                return;
            }

            Debug.Log("requested password reset successfully");
            Result("Password reset sent to your email !");
        });
    }

    public void NewUser()
    {
        DisconnectPreviousSession();

        string email = newUserEmailIF.text;
        string password = newUserPasswordIF.text;
        string newNickName = newUserPseudoIF.text;

        LootLockerSDKManager.WhiteLabelSignUp(email, password, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("Sign up error");
                Result("Sign up error, try again");
                return;
            }
            else
            {
                // Succesful response
                // Log in player to set name
                // Login the player
                LootLockerSDKManager.WhiteLabelLogin(email, password, true, response =>
                {
                    if (!response.success)
                    {
                        Debug.Log("Login error");
                        Result("Error while logging in, try again");
                        return;
                    }
                    // Start session
                    LootLockerSDKManager.StartWhiteLabelSession((response) =>
                    {
                        if (!response.success)
                        {
                            Debug.Log("Start session error");
                            Result("Start session error, try again");
                            return;
                        }
                        else
                        {
                            State = ConnectionState.CONNECTED;
                        }

                        // Set nickname to be public UID if nothing was provided
                        if (newNickName == "")
                        {
                            newNickName = response.player_id.ToString();
                        }
                        ConnectionManager.playerInfo.pseudo = newNickName;
                        ConnectionManager.playerInfo.id = response.player_id;
                        // Set new nickname for player
                        LootLockerSDKManager.SetPlayerName(newNickName, (response) =>
                        {
                            if (!response.success)
                            {
                                Debug.Log("Set player name error");
                                return;
                            }
                            else
                            {
                                Result("Account created successfully !");
                            }
                        });
                    });
                });
            }
        });
    }


    public void LoginAsGuest()
    {
        DisconnectPreviousSession();

        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Result("You are now connected as guest !");
                ConnectionManager.playerInfo.id = response.player_id;
                ConnectionManager.playerInfo.pseudo = response.player_id.ToString();
                State = ConnectionState.GUEST;
            }
            else
            {
                Result("Error while logging in as guest, try again");
            }
        });
    }

    public void Disconnect()
    {
        LootLockerSDKManager.EndSession((response) =>
        {
            if (!response.success)
            {
                Result("Error while disconnecting, try again");
            }
            else
            {
                Result("Disconnected successfully !");
                State = ConnectionState.NO_SESSION;
            }
        });
    }
    private void DisconnectPreviousSession()
    {
        if (State == ConnectionState.GUEST || State == ConnectionState.CONNECTED)
        {
            Disconnect();
        }
    }



    // ### Tools ###

    public void ActuStateText()
    {
        switch (State)
        {
            case ConnectionState.NO_CONNECTION:
                stateText.text = "State :\n No internet connection";
                break;
            case ConnectionState.NO_SESSION:
                stateText.text = "State :\n No session";
                break;
            case ConnectionState.GUEST:
                stateText.text = "State :\n" + ConnectionManager.playerInfo.id + " as Guest";
                break;
            case ConnectionState.CONNECTED:
                stateText.text = "State :\n" + (ConnectionManager.playerInfo.pseudo != "" ? ConnectionManager.playerInfo.pseudo : ConnectionManager.playerInfo.id) + " Connected";
                break;
            default:
                stateText.text = "State :\n Not found";
                break;
        }
    }
}
