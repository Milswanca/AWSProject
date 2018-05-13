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

    public void ViewProfileTest()
    {
        UICanvas.Get().TurnOnDarkenator();
        DatabaseHandler.Get().GetProfileInfo(inputTestViewProfile.text, ViewProfileRetrieved);
    }

    private void ViewProfileRetrieved(bool _success, DatabaseHandler.ErrorResult _error, NameValueCollection _values)
    {
        UICanvas.Get().TurnOffDarkenator();

        if (_success)
        {
            string username = _values.GetString("Username");
            string greeting = _values.GetString("Greeting");
            int dp = _values.GetInt("DisplayPic");

            Sprite dpSprite = MasterManager.instance.GetProfileSprite(dp);


            PopupManager.Get().QueuePopup(EPopupTypes.ViewProfile, new ViewProfilePopup.Data(username, greeting, dpSprite));
        }
    }
}
