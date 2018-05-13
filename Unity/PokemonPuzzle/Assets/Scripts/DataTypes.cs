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
    GS_FriendsList
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