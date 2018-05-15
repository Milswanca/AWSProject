using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBlockType
{
    BT_Heart,
    BT_Square,
    BT_Triangle,
    BT_Star,
    BT_Diamond
}

public enum EGameScreens
{
    GS_MainEntry,
    GS_CreateAccount,
    GS_Login,
    GS_MainMenu,
    GS_Game,
    GS_EditProfile,
    GS_ViewHighscores,
    GS_FriendsList,
    GS_ViewProfile
}

public enum EGameState
{
    GS_Menu,
    GS_Game
}

public enum EFriendRequestType
{
    FRT_Pending,
    FRT_Unaccepted,
    FRT_Friend
}

public class ProfileData
{
    public ProfileData(string _username, int _displayPic, string _greeting)
    {
        Username = _username;
        DisplayPic = _displayPic;
        Greeting = _greeting;
        Friends = new List<string>();
    }

    public ProfileData(NameValueCollection _profileCollection)
    {
        Friends = new List<string>();

        Username = _profileCollection.GetString("Username");
        Greeting = _profileCollection.GetString("Greeting");
        DisplayPic = _profileCollection.GetInt("DisplayPic");

        int index = 0;
        while (_profileCollection.DoesValueExist("Friend" + index))
        {
            Friends.Add(_profileCollection.GetString("Friend" + index));
            index++;
        }
    }

    public static bool operator ==(ProfileData _me, ProfileData _other)
    {
        return _me.Greeting == _other.Greeting && _me.DisplayPic == _other.DisplayPic && _me.Username == _other.Username && _other.Friends == _me.Friends;
    }

    public static bool operator !=(ProfileData _me, ProfileData _other)
    {
        return !(_other == _me);
    }

    public bool IsFriendsWith(string _friend)
    {
        return Friends.Contains(_friend);
    }

    public string Username;
    public string Greeting;
    public int DisplayPic;
    public List<string> Friends;
}