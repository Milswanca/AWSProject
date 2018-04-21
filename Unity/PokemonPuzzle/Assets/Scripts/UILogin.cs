using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILogin : MonoBehaviour
{
    [SerializeField]
    InputField txtUsername;

    [SerializeField]
    InputField txtPassword;

    private EventSystem eventSystem;
    private bool bLoginInProgress = false;

    void Start()
    {
        eventSystem = EventSystem.current;// EventSystemManager.currentSystem;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null)
            {

                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                    inputfield.OnPointerClick(new PointerEventData(eventSystem));  //if it's an input field, also set the text caret

                eventSystem.SetSelectedGameObject(next.gameObject, new BaseEventData(eventSystem));
            }
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            AttemptLogin();
        }
    }

    public void AttemptLogin()
    {
        if(bLoginInProgress) { return; }

        bLoginInProgress = true;

        string user = txtUsername.text;
        string pass = txtPassword.text;

        UICanvas.Get().TurnOnDarkenator();
        DatabaseHandler.Get().Login(user, pass, LoginCallback);
    }

    public void Back()
    {
        PanelManager.Get().ChangePanels(EGameScreens.GS_MainEntry);
    }

    private void LoginCallback(bool _success, DatabaseHandler.ErrorResult _message)
    {
        bLoginInProgress = false;
        UICanvas.Get().TurnOffDarkenator();

        if (_success)
        {
            Debug.Log("Login Success");

            PanelManager.Get().ChangePanels(EGameScreens.GS_MainMenu);
        }
    }
}
