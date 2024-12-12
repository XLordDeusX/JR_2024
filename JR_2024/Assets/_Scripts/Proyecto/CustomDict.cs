using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AudioClips
{
    Jump,
    Hit,
    Attack,
    Die,
}

[Serializable]
public class CustomDict<T, U>
{
    public T[] Keys;
    public U[] Values;
    private Dictionary<T, U> _dict;

    public void Initialize()
    {
        _dict = new();
        for (int i = 0; i < Keys.Length; i++)
        {
            _dict.Add(Keys[i], Values[i]);
        }
    }

    public U Find(T index)
    {
        return _dict[index];
    }
}
