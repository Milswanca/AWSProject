using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewProfilePopup : PopupWindow
{
    public class Data : PopupData
    {
        public Data() { }
        public Data(string _username, string _greeting, Sprite _displayPic)
        {
            Username = _username;
            Greeting = _greeting;
            DisplayPic = _displayPic;
        }

        public string Username;
        public string Greeting;
        public Sprite DisplayPic;
    }

    [SerializeField]
    Text Username;

    [SerializeField]
    Text Greeting;

    [SerializeField]
    Image DisplayPic;

    public override void Show(PopupData _data)
    {
        Data data = (Data)_data;

        if(data != null)
        {
            Username.text = data.Username;
            Greeting.text = "\"" + data.Greeting + "\"";
            DisplayPic.sprite = data.DisplayPic;
        }

        base.Show(_data);
    }

    public override EPopupTypes GetPopupType()
    {
        return EPopupTypes.ViewProfile;
    }
}
