using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSingleTextSingleOption : PopupWindow
{
    public class Data : PopupData
    {
        public Data() { }
        public Data(string _heading, string _body, string _option)
        {
            Heading = _heading;
            Body = _body;
            Option = _option;
        }

        public string Heading;
        public string Body;
        public string Option;
    }

    [SerializeField]
    Text Heading;

    [SerializeField]
    Text Body;

    [SerializeField]
    Text Option;

    public override void Show(PopupData _data)
    {
        Data data = (Data)_data;

        if(data != null)
        {
            Heading.text = data.Heading;
            Body.text = data.Body;
            Option.text = data.Option;
        }

        base.Show(_data);
    }

    public override EPopupTypes GetPopupType()
    {
        return EPopupTypes.SingleTextSingleOption;
    }
}
