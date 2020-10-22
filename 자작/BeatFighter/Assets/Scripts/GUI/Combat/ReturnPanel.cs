using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPanel : PanelBase
{
    public override void Initialize() { }

    public void OnReturnButton()
    {
        CombatManager.Instance.Return();
    }
}
