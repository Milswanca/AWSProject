using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField]
    EGameScreens defaultScreen = EGameScreens.GS_MainEntry;

    public UIPanel CurrentScreen { get { return panelStack.Peek(); } }
    private Stack<UIPanel> panelStack;

    private Dictionary<EGameScreens, UIPanel> screenMap;

    public static PanelManager Get()
    {
        return MasterManager.instance.PanelManager;
    }

    private void Awake()
    {
        screenMap = new Dictionary<EGameScreens, UIPanel>();
        panelStack = new Stack<UIPanel>();
    }

    private void Start()
    {
        ChangePanels(defaultScreen);
    }

    public void ShowPanel(EGameScreens _panel)
    {
        panelStack.Push(screenMap[_panel]);

        if (CurrentScreen)
        {
            CurrentScreen.gameObject.SetActive(true);
            CurrentScreen.Activate();
        }
    }

    public void HideTopPanel()
    {
        if (panelStack.Count > 0)
        {
            UIPanel top = panelStack.Pop();
            top.gameObject.SetActive(false);
            top.Deactivate();
        }
    }

    public void CloseAllPanels()
    {
        while (panelStack.Count > 0)
        {
            HideTopPanel();
        }
    }

    public void ChangePanels(EGameScreens _panel)
    {
        CloseAllPanels();
        ShowPanel(_panel);
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
