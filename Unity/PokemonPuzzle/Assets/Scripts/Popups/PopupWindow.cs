using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupWindow : MonoBehaviour
{
    internal protected PopupManager popupManager;

    public virtual void Show(PopupData _data)
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);

        if (popupManager)
        {
            popupManager.ActivePopupHidden();
        }
    }

    public virtual EPopupTypes GetPopupType()
    {
        return EPopupTypes.None;
    }
}
