using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPopupTypes
{
    None,
    SingleTextSingleOption,
    ViewProfile
}

//Parent class for popup data
public class PopupData
{

}

//Popup queue data
public class PopupQueueData
{
    public PopupQueueData(EPopupTypes _type, PopupData _data)
    {
        PopupType = _type;
        PopupData = _data;
    }

    public EPopupTypes PopupType;
    public PopupData PopupData;
}

public class PopupManager : MonoBehaviour
{
    [SerializeField]
    private PopupWindow[] Popups;
    private PopupWindow activePopup;
    private Queue<PopupQueueData> popupQueue;
    private Dictionary<EPopupTypes, PopupWindow> popupMap;

    public bool IsShowingPopup { get { return activePopup != null; } }

    public static PopupManager Get()
    {
        return MasterManager.instance.PopupManager;
    }

    private void Awake()
    {
        popupQueue = new Queue<PopupQueueData>();
        activePopup = null;
        SetupPopupMap();
    }

    public void QueuePopup(EPopupTypes _popupType, PopupData _popupData)
    {
        PopupQueueData newQueue = new PopupQueueData(_popupType, _popupData);
        popupQueue.Enqueue(newQueue);

        TryShowNext();
    }

    public void ActivePopupHidden()
    {
        activePopup = null;
        TryShowNext();
    }

    private void TryShowNext()
    {
        if(IsShowingPopup) { return; }
        if(popupQueue.Count == 0) { return; }

        PopupQueueData data = popupQueue.Dequeue();

        if(!popupMap.ContainsKey(data.PopupType))
        {
            TryShowNext();
            return;
        }

        activePopup = popupMap[data.PopupType];
        activePopup.Show(data.PopupData);
    }

    private void SetupPopupMap()
    {
        popupMap = new Dictionary<EPopupTypes, PopupWindow>();

        foreach (PopupWindow i in Popups)
        {
            if (popupMap.ContainsKey(i.GetPopupType()))
            {
                Debug.LogError("Duplicate popup window type found: " + i.GetPopupType().ToString());
                continue;
            }

            popupMap.Add(i.GetPopupType(), i);
            i.gameObject.SetActive(false);
            i.popupManager = this;
        }
    }
}
