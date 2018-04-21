using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICreateAccount : MonoBehaviour {
    [SerializeField]
    InputField txtUsername;

    [SerializeField]
    InputField txtPassword;

    public void AttemptCreateAccount()
    {
        string user = txtUsername.text;
        string pass = txtPassword.text;

        UICanvas.Get().TurnOnDarkenator();
        DatabaseHandler.Get().CreateAccount(user, pass, CreateAccountCallback);
    }

    public void Back()
    {
        MasterManager.instance.PanelManager.ChangePanels(EGameScreens.GS_MainEntry);
    }

    private void CreateAccountCallback(bool _success, DatabaseHandler.ErrorResult _message)
    {
        UICanvas.Get().TurnOffDarkenator();

        if(_success)
        {
            MasterManager.instance.PanelManager.ChangePanels(EGameScreens.GS_Login);
        }
    }
}
