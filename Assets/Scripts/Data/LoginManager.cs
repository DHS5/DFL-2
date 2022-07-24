using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using LootLocker.Requests;

public class LoginManager : MonoBehaviour
{
    private enum ConnectionState { NO_CONNECTION, NO_SESSION, GUEST, CONNECTED }


    [Header("UI components")]
    [Header("Base Screen")]
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button newUserButton;
    [SerializeField] private Button loginAsGuestButton;
    [SerializeField] private Button disconnectButton;
    [Space]
    [SerializeField] private GameObject waitPopup;

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


    private string playerName;

    private bool connected = false;

    private ConnectionState connectionState;
    private ConnectionState State
    {
        get { return connectionState; }
        set
        {
            connectionState = value;
            ActuStateText();
        }
    }


    // ### Built-in ###

    private void Awake()
    {
        StartCoroutine(AutoLogin());
    }


    // ### Functions ###

    private void Result(string result)
    {
        resultScreen.SetActive(true);
        resultText.text = result;
    }

    public void CheckInternetConnection()
    {
        if (!connected)
        {
            waitPopup.SetActive(true);
            StartCoroutine(CheckInternetConnectionCR());
        }
        ActuDisconnectButton();
    }

    public void ActuDisconnectButton()
    {
        disconnectButton.interactable = (State == ConnectionState.CONNECTED || State == ConnectionState.GUEST);
    }

    private IEnumerator CheckInternetConnectionCR()
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();
        waitPopup.SetActive(false);

        if (request.error != null)
        {
            State = ConnectionState.NO_CONNECTION;
            loginButton.interactable = false;
            newUserButton.interactable = false;
            loginAsGuestButton.interactable = false;
            disconnectButton.interactable = false;
        }
        else
        {
            Debug.Log("Connected to internet");
            loginButton.interactable = true;
            newUserButton.interactable = true;
            loginAsGuestButton.interactable = true;
        }
    }


    public void Login()
    {
        State = ConnectionState.NO_SESSION;

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
                        playerName = response.public_uid;
                        Debug.Log("session started successfully");
                        Result("You are now connected !");
                        LootLockerSDKManager.GetPlayerName((response) =>
                        {
                            if (response.success)
                            {
                                playerName = response.name;
                                ActuStateText();
                            }
                        });
                        State = ConnectionState.CONNECTED;
                    }
                });
            }
        });
    }

    private IEnumerator AutoLogin()
    {
        State = ConnectionState.NO_SESSION;

        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();

        if (request.error == null)
        {
            connected = true;
            Debug.Log("Connected to internet");

            LootLockerSDKManager.CheckWhiteLabelSession(response =>
            {
                if (response == false)
                {
                    Debug.Log("No active session");
                }
                else
                {
                // Session is valid, start game session
                    LootLockerSDKManager.StartWhiteLabelSession((response) =>
                    {
                        if (response.success)
                        {
                            playerName = response.public_uid;
                            Debug.Log("Session started successfully");
                            LootLockerSDKManager.GetPlayerName((response) =>
                            {
                                if (response.success)
                                {
                                    playerName = response.name;
                                    ActuStateText();
                                }
                            });
                            State = ConnectionState.CONNECTED;
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
        State = ConnectionState.NO_SESSION;

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
                            newNickName = response.public_uid;
                        }
                        playerName = newNickName;
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
                                Debug.Log("Account Created");
                                Result("Account created successfully !");
                            }
                        });
                    });
                });
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

    public void LoginAsGuest()
    {
        State = ConnectionState.NO_SESSION;

        DisconnectPreviousSession();

        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Result("You are now connected as guest !");
                State = ConnectionState.GUEST;
            }
            else
            {
                Result("Error while logging in as guest, try again");
            }
        });
    }

    private void DisconnectPreviousSession()
    {
        if (State == ConnectionState.GUEST || State == ConnectionState.CONNECTED)
        {
            LootLockerSDKManager.EndSession((response) =>
            {
                if (!response.success)
                {
                    Debug.Log("Error while disconnecting previous session");
                }
                else
                {
                    Debug.Log("Disconnected previous session");
                }
            });
        }
    }



    // ### Tools ###

    public void ActuStateText()
    {
        switch (connectionState)
        {
            case ConnectionState.NO_CONNECTION:
                stateText.text = "State :\n No internet connection";
                break;
            case ConnectionState.NO_SESSION:
                stateText.text = "State :\n No session";
                break;
            case ConnectionState.GUEST:
                stateText.text = "State :\n Guest";
                break;
            case ConnectionState.CONNECTED:
                stateText.text = "State :\n" + playerName + " Connected";
                break;
            default:
                stateText.text = "State :\n Not found";
                break;
        }
    }
}
