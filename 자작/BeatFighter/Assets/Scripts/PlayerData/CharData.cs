using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharData
{
    public int level;
    public int exp;

    public void Initialize()
    {
        level = 1;
        exp = 0;
    }
}
