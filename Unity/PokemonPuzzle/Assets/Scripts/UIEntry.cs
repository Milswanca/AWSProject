using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEntry : MonoBehaviour
{
    public void CreateAccount()
    {
        MasterManager.instance.PanelManager.ChangePanels(EGameScreens.GS_CreateAccount);
    }

    public void Login()
    {
        MasterManager.instance.PanelManager.ChangePanels(EGameScreens.GS_Login);
    }
}
