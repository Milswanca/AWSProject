using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIViewProfile : MonoBehaviour
{
    [SerializeField]
    Text Username;

    [SerializeField]
    Text Greeting;

    [SerializeField]
    Image DisplayPic;

    [SerializeField]
    Transform FriendedRoot;

    private ProfileData profile;

    public void SetProfile(ProfileData _profile)
    {
        profile = _profile;
    }

    private void OnEnable()
    {
        Username.text = profile.Username;
        Greeting.text = profile.Greeting;
        DisplayPic.sprite = MasterManager.instance.GetProfileSprite(profile.DisplayPic);

        RefreshFriended();
    }

    public void RemoveFriendPressed()
    {
        DatabaseHandler.Get().RemoveFriend(profile.Username, UnfriendedComplete);
    }

    public void ClosePressed()
    {
        PanelManager.Get().HideTopPanel();
    }

    private void UnfriendedComplete(bool _success, DatabaseHandler.ErrorResult _error)
    {
        if(_success)
        {
            profile.Friends.Remove(DatabaseHandler.Username);
            RefreshFriended();
        }
    }

    private void RefreshFriended()
    {
        if (!FriendedRoot) { return; }
        if (profile.IsFriendsWith(DatabaseHandler.Username))
        {
            FriendedRoot.gameObject.SetActive(true);
        }
        else
        {
            FriendedRoot.gameObject.SetActive(false);
        }
    }
}
