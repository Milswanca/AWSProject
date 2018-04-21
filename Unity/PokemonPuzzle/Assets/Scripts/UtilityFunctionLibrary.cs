using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class UtilityFunctionLibrary
{
    public static void RemoveAt<T>(ref T[] _arr, int _index)
    {
        for (int a = _index; a < _arr.Length - 1; a++)
        {
            // moving elements downwards, to fill the gap at [index]
            _arr[a] = _arr[a + 1];
        }

        // finally, let's decrement Array's size by one
        System.Array.Resize(ref _arr, _arr.Length - 1);
    }

    public static void Swap<T>(ref T[] _arr, int _swapIndex, int _withIndex)
    {
        T temp = _arr[_swapIndex];
        _arr[_swapIndex] = _arr[_withIndex];
        _arr[_withIndex] = temp;
    }

    public static T Splice<T>(ref T[] _arr, int _index)
    {
        T ret = _arr[_index];
        RemoveAt(ref _arr, _index);
        return ret;
    }

    public static T SpliceRandom<T>(ref T[] _arr)
    {
        int rand = _arr.RandomIndex();
        return Splice(ref _arr, rand);
    }

    public static int RandomIndex<T>(this T[] _arr)
    {
        return Random.Range(0, _arr.Length);
    }

    public static void AddUnique<T>(this List<T> _list, T _toAdd)
    {
        if(!_list.Contains(_toAdd))
        {
            _list.Add(_toAdd);
        }
    }
}