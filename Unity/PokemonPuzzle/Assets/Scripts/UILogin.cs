using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour
{
    [SerializeField]
    InputField txtUsername;

    [SerializeField]
    InputField txtPassword;

    public void AttemptLogin()
    {
        string user = txtUsername.text;
        string pass = txtPassword.text;

        UICanvas.Get().TurnOnDarkenator();
        DatabaseHandler.Get().Login(user, pass, LoginCallback);
    }

    public void Back()
    {
        PanelManager.Get().ChangePanels(EGameScreens.GS_MainEntry);
    }

    private void LoginCallback(bool _success, DatabaseHandler.ErrorResult _message)
    {
        UICanvas.Get().TurnOffDarkenator();

        if (_success)
        {
            Debug.Log("Login Success");

            PanelManager.Get().ChangePanels(EGameScreens.GS_MainMenu);
        }
    }
}
