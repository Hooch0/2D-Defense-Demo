using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable 
{
    Action Destroyed { get; set; }
    Action<float> Damaged { get; set; }
    Teams TeamID { get; set; }
    bool IsTargetable { get; set; }

    float MaxHealth { get; }
    float Health { get; set; }

    Transform GetTransform();
}
