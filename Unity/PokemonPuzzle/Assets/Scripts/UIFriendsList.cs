using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFriendsList : MonoBehaviour
{
    [SerializeField]
    GameObject PendingRoot;

    [SerializeField]
    GameObject UnacceptedRoot;

    [SerializeField]
    GameObject FriendsRoot;

    [SerializeField]
    GameObject FriendEntryPrefab;

    private void OnEnable()
    {
        RefreshFriendsList();
    }

    private void RetrievedFriends(bool _success, DatabaseHandler.ErrorResult _error, NameValueCollection _collection)
    {
        if (!_success) { return; }

        ClearFriendsList();

        int index = 0;
        while(_collection.DoesValueExist("Pending" + index))
        {
            UIFriendEntry entry = Instantiate(FriendEntryPrefab, PendingRoot.transform).GetComponent<UIFriendEntry>();
            entry.Init(_collection.GetString("Pending" + index), EFriendRequestType.FRT_Pending);
            index++;
        }

        index = 0;
        while (_collection.DoesValueExist("Unaccepted" + index))
        {
            UIFriendEntry entry = Instantiate(FriendEntryPrefab, UnacceptedRoot.transform).GetComponent<UIFriendEntry>();
            entry.Init(_collection.GetString("Unaccepted" + index), EFriendRequestType.FRT_Unaccepted);
            index++;
        }

        index = 0;
        while (_collection.DoesValueExist("Friends" + index))
        {
            UIFriendEntry entry = Instantiate(FriendEntryPrefab, FriendsRoot.transform).GetComponent<UIFriendEntry>();
            entry.Init(_collection.GetString("Friends" + index), EFriendRequestType.FRT_Friend);
            index++;
        }
    }

    public void ClearFriendsList()
    {
        foreach (Transform child in PendingRoot.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in UnacceptedRoot.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in FriendsRoot.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void RefreshFriendsList()
    {
        DatabaseHandler.Get().GetMyFriends(RetrievedFriends);
    }

    public void Close()
    {
        PanelManager.Get().ChangePanels(EGameScreens.GS_MainMenu);
    }
}
