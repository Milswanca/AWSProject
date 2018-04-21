using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public void PlayPressed()
    {
        MasterManager.instance.ChangeGameState(EGameState.GS_Game);
        PanelManager.Get().ChangePanels(EGameScreens.GS_Game);
    }

    public void ProfilePressed()
    {
        PanelManager.Get().ChangePanels(EGameScreens.GS_EditProfile);
    }

    public void HighscoresPressed()
    {

    }

    public void LogoutPressed()
    {
        DatabaseHandler.Get().Logout();
        PanelManager.Get().ChangePanels(EGameScreens.GS_Login);
    }
}
