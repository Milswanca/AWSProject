using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEditProfile : MonoBehaviour
{
    private struct ProfileData
    {
        public ProfileData(int _displayPic, string _greeting)
        {
            DisplayPic = _displayPic;
            Greeting = _greeting;
        }

        public int DisplayPic;
        public string Greeting;

        public static bool operator == (ProfileData _me, ProfileData _other)
        {
            return _me.Greeting == _other.Greeting && _me.DisplayPic == _other.DisplayPic;
        }

        public static bool operator != (ProfileData _me, ProfileData _other)
        {
            return !(_other == _me);
        }
    }

    [SerializeField]
    InputField greeting;

    [SerializeField]
    Image displayPic;

    [SerializeField]
    Text username;

    private Sprite[] possibleDisplayPics;
    private ProfileData defaultProfileData;
    private ProfileData newProfileData;

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

            string greeting = _params.GetString("Greeting");
            int dp = _params.GetInt("DisplayPic");

            defaultProfileData = new ProfileData(dp, greeting);

            SetDisplayPic(dp);
            SetGreeting(greeting);
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
}
