﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPanel : PanelBase
{
    public void OnReturnButton()
    {
        Combat.Instance.Return();
    }
}
