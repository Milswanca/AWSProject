using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEditProfile : MonoBehaviour
{
    [SerializeField]
    InputField greeting;

    [SerializeField]
    Image displayPic;

    [SerializeField]
    Text username;

    private Sprite[] possibleDisplayPics;
    private ProfileData defaultProfileData;
    private ProfileData newProfileData;

    private void Awake()
    {
        defaultProfileData = new ProfileData("", 0, "");
        newProfileData = new ProfileData("", 0, "");
    }

    private void OnEnable()
    {
        Sprite[] defaultPics = MasterManager.instance.GameGlobals.possibleDisplayPics;
        possibleDisplayPics = new Sprite[defaultPics.Length];
        System.Array.Copy(defaultPics, possibleDisplayPics, defaultPics.Length);
        DatabaseHandler.Get().GetMyProfileInfo(OnRetrievedMyProfile);
    }

    private void OnRetrievedMyProfile(bool _success, DatabaseHandler.ErrorResult _error, NameValueCollection _params)
    {
        if (_success)
        {
            username.text = _params.GetString("Username");

            defaultProfileData = new ProfileData(_params);

            SetDisplayPic(defaultProfileData.DisplayPic);
            SetGreeting(defaultProfileData.Greeting);
        }
    }

    public void AcceptPressed()
    {
        if(defaultProfileData != newProfileData)
        {
            DatabaseHandler.Get().UpdateProfileInfo(newProfileData.Greeting, newProfileData.DisplayPic, null);
        }

        PanelManager.Get().ChangePanels(EGameScreens.GS_MainMenu);
    }

    public void BackPressed()
    {
        PanelManager.Get().ChangePanels(EGameScreens.GS_MainMenu);
    }

    public void NextDP()
    {
        newProfileData.DisplayPic = Utility.WrapInt(newProfileData.DisplayPic + 1, 0, possibleDisplayPics.Length - 1);
        SetDisplayPic(newProfileData.DisplayPic);
    }

    public void PrevDP()
    {
        newProfileData.DisplayPic = Utility.WrapInt(newProfileData.DisplayPic - 1, 0, possibleDisplayPics.Length - 1);
        SetDisplayPic(newProfileData.DisplayPic);
    }

    public void GreetingUpdated()
    {
        SetGreeting(greeting.text);
    }

    void SetGreeting(string _greeting)
    {
        newProfileData.Greeting = _greeting;
        greeting.text = _greeting;
    }

    void SetDisplayPic(int _displayPic)
    {
        newProfileData.DisplayPic = _displayPic;
        displayPic.sprite = possibleDisplayPics[_displayPic];
    }

    public void ChangePassword()
    {
        PanelManager.Get().ShowPanel(EGameScreens.GS_ChangePassword);
    }
}
