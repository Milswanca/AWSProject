using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Utility
{
    public static int WrapInt(int _newValue, int _min, int _max)
    {
        int outVal = _newValue;

        if (outVal < _min)
        {
            outVal = _max;
        }
        else if (outVal > _max)
        {
            outVal = _min;
        }

        return outVal;
    }
}