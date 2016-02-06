Part of the [unity-utilities](https://github.com/DigitalMachinist/unity-utilities) GitHub repo by [@DigitalMachinist](https://github.com/DigitalMachinist).

The classes included in this library provide specialized attributes that affect the way fields appear in the Unity Inspector. They don't do anything to change behaviour of the fields to which they are applied, but they change how developers can interact with them in the Inspector (they don't change how they can be used in code).

Each of the included attributes is declared in a separate file, so you can pick and choose the attributes that benefit your project and only place those that you need into your project.

This library is compatible with both Unity Free and Unity Pro and should run on Unity versions back to 3.x (and possibly earlier).

**Note:** This library depends on the ```ReadOnlyAttribute``` class (also contained in this repo).

## Included attributes

 - ```EnumFlagsAttribute```
 - ```ReadOnlyAttribute```

### EnumFlagsAttribute

*Credit for this goes to Unify Community for [this implementation](http://wiki.unity3d.com/index.php/EnumFlagPropertyDrawer).*

This ```FlagsAttribute``` attribute provides a better interface for flags-style enumerations in the Unity Inspector. Typically, the Inspector displays enumerations as a dropdown selection which allows you to pick exactly 1 value from the enum. While this is suitable behaviour for some enumerations, when an enumeration was declared using the ```[System.Flags]``` attribute, several bits of the flags enum may be set simultaneously, which is at odds with the standard Inspector control.

This custom attribute drawer allows developers to set several bits simultaneously (just like a ```LayerMask``` field) through the Inspector, so that you can take full advantage of ```[System.Flags]``` in your applications.

#### Usage

Declaring an enumeration as a flags-type enum is simple. Just declare an enum as you normally would, but make sure that each of the possible values only sets a single bit of the backing integer value:

```csharp
[System.Flags]
public enum MyEnum
{
  First  = 1 << 0, // Each flag corresponds to 1 bit.
  Second = 1 << 1,
  Third  = 1 << 2
}
```

Add a public  field of the enum type you defined to a component and give it the ```[EnumFlags]``` attribute. Default values may or may not require some fancy tricks:

 - If you want none of the flags set by default, assign the field a value of 0.
 - If you want to set a single flag by default, just assign the field to that flag.
 - If you want to set several (but perhaps not all) of the flags by default, assign the field to by bitwise-OR-ing the flags that you want to set together.
 - If you want to set all flags by default, assign the field to -1 and cast -1 to your enumeration's type explicitly.
 - You can specify a special Inspector name for any enum field by providing a string name to the ```[EnumFlags]``` attribute (as below).

```csharp
public class MyComponent : MonoBehaviour
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
```

These fields will now show up in the Inspector with a multi-select dropdown that acts a lot like a ```LayerMask``` property drawer:

![Enum flags field in the inspector](https://raw.githubusercontent.com/DigitalMachinist/unity-utilities/master/Assets/Utilities/Attributes/EnumFlagsAttribute.png)

## ReadOnlyAttribute

*Credit for this implementation goes to @JamesZinger.*

The ```ReadOnlyAttribute``` attribute allows you to make a field that appears in the inspector read only.

### Usage

Simply prefix the ```[ReadOnly]``` attribute before the field you want to protect, like so:

```csharp
public class MyComponent : MonoBehaviour
{
  [ReadOnly]
  public int Number;
}
```

The read only field will be greyed-out and unmodifiable from the Inspector.

![Read only field in the inspector](https://raw.githubusercontent.com/DigitalMachinist/unity-utilities/master/Assets/Utilities/Attributes/ReadOnlyAttribute.png)
