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

    public void Init(string _name, EFriendRequestType _type)
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
    }

    public void Accept()
    {
        if(RequestType != EFriendRequestType.FRT_Unaccepted)
        {
            return;
        }

        //accept
    }

    public void Decline()
    {
        if (RequestType != EFriendRequestType.FRT_Unaccepted)
        {
            return;
        }

        //Decline
    }

    public void ViewProfile()
    {
        UICanvas.Get().TurnOnDarkenator();
        DatabaseHandler.Get().GetProfileInfo(FriendName, ViewProfileRetrieved);
    }

    private void ViewProfileRetrieved(bool _success, DatabaseHandler.ErrorResult _error, NameValueCollection _collection)
    {
        UICanvas.Get().TurnOffDarkenator();

        if (_success)
        {
            string username = _collection.GetString("Username");
            string greeting = _collection.GetString("Greeting");
            int dp = _collection.GetInt("DisplayPic");

            Sprite dpSprite = MasterManager.instance.GetProfileSprite(dp);

            PopupManager.Get().QueuePopup(EPopupTypes.ViewProfile, new ViewProfilePopup.Data(username, greeting, dpSprite));
        }
    }
}
