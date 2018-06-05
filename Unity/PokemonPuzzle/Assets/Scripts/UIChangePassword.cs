using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChangePassword : MonoBehaviour
{
    [SerializeField]
    InputField oldPassword;

    [SerializeField]
    InputField newPassword;

    public void Accept()
    {
        if (oldPassword.text == "" || newPassword.text == "")
        {
            PopupSingleTextSingleOption.Data data = new PopupSingleTextSingleOption.Data("Password Error", "Please fill in both password fields", "Okay");
            PopupManager.Get().QueuePopup(EPopupTypes.SingleTextSingleOption, data);
            return;
        }

        DatabaseHandler.Get().ChangePassword(oldPassword.text, newPassword.text, PasswordChangeCallback);
        UICanvas.Get().TurnOnDarkenator();
    }

    void PasswordChangeCallback(bool _success, DatabaseHandler.ErrorResult _error)
    {
        UICanvas.Get().TurnOffDarkenator();

        if (_success)
        {
            Close();
        }
    }

    public void Close()
    {
        PanelManager.Get().HideTopPanel();
    }
}
