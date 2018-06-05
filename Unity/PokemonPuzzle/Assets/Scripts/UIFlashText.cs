using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlashText : MonoBehaviour
{
    [SerializeField]
    Text toFlash;

    [SerializeField]
    AnimationCurve alphaCurve;

	// Use this for initialization
	void Start ()
    {
	    if(alphaCurve != null)
        {
            alphaCurve.postWrapMode = WrapMode.Loop;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(alphaCurve != null)
        {
            toFlash.color = new Color(toFlash.color.r, toFlash.color.g, toFlash.color.b, alphaCurve.Evaluate(Time.fixedTime));
        }
	}
}
