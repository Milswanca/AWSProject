using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public EGameScreens GameScreenID;
    protected bool IsScreenActive = false;

    [SerializeField]
    GameObject panelContent;

    private void Awake()
    {
        panelContent.SetActive(false);
    }

    void Start()
    {
        MasterManager.instance.PanelManager.RegisterPanel(this);
    }

    private void OnDestroy()
    {
        MasterManager.instance.PanelManager.DeregisterPanel(this);
    }

    public virtual void Activate()
    {
        IsScreenActive = true;
        panelContent.SetActive(true);
    }

    public virtual void Deactivate()
    {
        IsScreenActive = false;
        panelContent.SetActive(false);
    }
}
