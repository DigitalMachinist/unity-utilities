using System;
using System.Collections;﻿
using System.Collections.Generic;﻿
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[Flags]
public enum MyEnum
{
    First  = 1 << 0,
    Second = 1 << 1,
    Third  = 1 << 2
}

public class EnumFlagsAttributeTest : MonoBehaviour
{
    [EnumFlags( "Default: Nothing" )]
    public MyEnum SetNoneByDefault = 0;

    [EnumFlags( "Default: Second" )]
    public MyEnum SetOneByDefault = MyEnum.Second;

    [EnumFlags( "Defaullt: Mixed" )]
    public MyEnum SetSeveralByDefault = MyEnum.Second | MyEnum.Third;

    [EnumFlags( "Default: Everything" )]
    public MyEnum SetAllByDefault = (MyEnum)( -1 );
}
