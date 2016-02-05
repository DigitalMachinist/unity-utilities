using System;
using System.Collections;﻿
using System.Collections.Generic;﻿
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class VibrateAxis : MonoBehaviour
{
    public Xbox360Gamepad Gamepad;
    public Xbox360GamepadAxis Axis;
    public bool IsVibratingSlowly = true;
    public bool IsVibratingQuickly = true;

    void Update()
    {
        if ( IsVibratingSlowly )
        {
            Gamepad.SetSlowVibration( Gamepad.GetAxis( Axis ) );
        }
        if ( IsVibratingQuickly )
        {
            Gamepad.SetFastVibration( Gamepad.GetAxis( Axis ) );
        }
    }
}
