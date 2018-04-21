using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameValueCollection
{
    Dictionary<string, string> nameValueMap;

    public NameValueCollection()
    {
        nameValueMap = new Dictionary<string, string>();
    }

    public void Add(string _name, string _value)
    {
        if(nameValueMap.ContainsKey(_name))
        {
            nameValueMap[_name] = _value;
        }

        nameValueMap.Add(_name, _value);
    }

    public int GetInt(string _name)
    {
        if(nameValueMap.ContainsKey(_name))
        {
            return System.Convert.ToInt32(nameValueMap[_name]);
        }

        return 0;
    }

    public string GetString(string _name)
    {
        if (nameValueMap.ContainsKey(_name))
        {
            return nameValueMap[_name];
        }

        return "";
    }

    public float GetFloat(string _name)
    {
        if (nameValueMap.ContainsKey(_name))
        {
            return System.Convert.ToSingle(nameValueMap[_name]);
        }

        return 0.0f;
    }

    public bool GetBool(string _name)
    {
        if (nameValueMap.ContainsKey(_name))
        {
            return System.Convert.ToBoolean(nameValueMap[_name]);
        }

        return false;
    }
}