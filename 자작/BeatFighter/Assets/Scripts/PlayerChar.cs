using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : BaseChar
{
    public override void SetTarget(BaseChar target)
    {
        base.SetTarget(target);
        Combat.Instance.SetTarget(target);
    }
}
