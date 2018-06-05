using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFriendEntry : MonoBehaviour
{
    [SerializeField]
    GameObject PendingRoot;

    [SerializeField]
    GameObject UnacceptedRoot;

    [SerializeField]
    GameObject FriendRoot;

    [SerializeField]
    Text FriendText;

    public string FriendName { get; private set; }
    public EFriendRequestType RequestType { get; private set; }

    private UIFriendsList friendsList = null;

    public void Init(string _name, EFriendRequestType _type, UIFriendsList _friendsList)
    {
        FriendName = _name;
        FriendText.text = _name;
        RequestType = _type;

        switch(RequestType)
        {
            case EFriendRequestType.FRT_Unaccepted:
                UnacceptedRoot.SetActive(true);
                break;
            case EFriendRequestType.FRT_Pending:
                PendingRoot.SetActive(true);
                break;
            case EFriendRequestType.FRT_Friend:
                FriendRoot.SetActive(true);
                break;
        }

        friendsList = _friendsList;
    }

    public void Accept()
    {
        if(RequestType != EFriendRequestType.FRT_Unaccepted)
        {
            return;
        }

        friendsList.FriendAccepted(FriendName);
    }

    public void Decline()
    {
        if (RequestType != EFriendRequestType.FRT_Unaccepted)
        {
            return;
        }

        friendsList.FriendDeclined(FriendName);
    }

    public void ViewProfile()
    {
        UICanvas.Get().TurnOnDarkenator();
        DatabaseHandler.Get().GetProfileInfo(FriendName, ViewProfileRetrieved);
    }

    //Show profile
    private void ViewProfileRetrieved(bool _success, DatabaseHandler.ErrorResult _error, NameValueCollection _collection)
    {
        UICanvas.Get().TurnOffDarkenator();

        if (_success)
        {
            UIViewProfile profileView = PanelManager.Get().GetScreen(EGameScreens.GS_ViewProfile).PanelContent.GetComponent<UIViewProfile>();

            if (profileView)
            {
                profileView.SetProfile(new ProfileData(_collection));
                PanelManager.Get().ShowPanel(EGameScreens.GS_ViewProfile);
            }
        }
    }
}
