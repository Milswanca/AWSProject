using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour {

    [SerializeField]
    Image darkenator;

    int darkenatorCount = 0;

    public static UICanvas Get()
    {
        return MasterManager.instance.UICanvas;
    }

	public void TurnOnDarkenator()
    {
        darkenatorCount += 1;
        darkenator.gameObject.SetActive(true);
    }

    public void TurnOffDarkenator()
    {
        darkenatorCount -= 1;

        if(darkenatorCount <= 0)
        {
            darkenator.gameObject.SetActive(false);
        }
    }
}
