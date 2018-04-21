using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField]
    EGameScreens defaultScreen = EGameScreens.GS_MainEntry;

    public UIPanel CurrentScreen { get; private set; }

    private Dictionary<EGameScreens, UIPanel> screenMap;

    public static PanelManager Get()
    {
        return MasterManager.instance.PanelManager;
    }

    private void Awake()
    {
        screenMap = new Dictionary<EGameScreens, UIPanel>();
    }

    private void Start()
    {
        ChangePanels(defaultScreen);
    }

    public void ChangePanels(EGameScreens _panel)
    {
        if(CurrentScreen)
        {
            CurrentScreen.gameObject.SetActive(false);
            CurrentScreen.Deactivate();
        }

        CurrentScreen = GetScreen(_panel);

        if (CurrentScreen)
        {
            CurrentScreen.gameObject.SetActive(true);
            CurrentScreen.Activate();
        }
    }

    public void RegisterPanel(UIPanel _panel)
    {
        if(GetScreen(_panel.GameScreenID))
        {
            Debug.LogError("Two instances of Panel exists: " + _panel.GameScreenID);
            return;
        }

        screenMap[_panel.GameScreenID] = _panel;
    }

    public void DeregisterPanel(UIPanel _panel)
    {
        screenMap[_panel.GameScreenID] = null;
    }

    public UIPanel GetScreen(EGameScreens _screen)
    {
        if(screenMap.ContainsKey(_screen))
        {
            return screenMap[_screen];
        }
        else
        {
            return null;
        }
    }
}
