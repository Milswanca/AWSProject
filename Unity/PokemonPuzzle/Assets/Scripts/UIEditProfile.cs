using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEditProfile : MonoBehaviour
{
    [SerializeField]
    Text greeting;

    [SerializeField]
    Text displayPic;

    [SerializeField]
    Text username;

    private void OnEnable()
    {
        DatabaseHandler.Get().GetMyProfileInfo(OnRetrievedMyProfile);
    }

    private void OnRetrievedMyProfile(bool _success, DatabaseHandler.ErrorResult _error, NameValueCollection _params)
    {
        if (_success)
        {
            greeting.text = _params.GetString("Greeting");
            displayPic.text = _params.GetString("DisplayPic");
            username.text = _params.GetString("Username");
        }
    }

    public void BackPressed()
    {
        PanelManager.Get().ChangePanels(EGameScreens.GS_MainMenu);
    }
}
