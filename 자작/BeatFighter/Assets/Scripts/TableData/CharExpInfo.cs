﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharExpInfo : TableData.IData<int>
{
    public int level;
    public int requireExp;

    public int Key()
    {
        return level;
    }
}
