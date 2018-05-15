using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField]
    InputField inputTestViewProfile;

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
        PanelManager.Get().ChangePanels(EGameScreens.GS_ViewHighscores);
    }

    public void FriendsPressed()
    {
        PanelManager.Get().ShowPanel(EGameScreens.GS_FriendsList);
    }

    public void LogoutPressed()
    {
        DatabaseHandler.Get().Logout();
        PanelManager.Get().ChangePanels(EGameScreens.GS_Login);
    }
}
