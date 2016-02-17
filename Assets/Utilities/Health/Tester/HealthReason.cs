using System;

public enum HealthReason : int
{
    None = 1,
    Enemy,
    Environment,
    Self
}

public static class HealthReasonExtensions 
{
    public static int ToValue( this HealthReason reason )
    {
        return (int)reason;
    }

    public static HealthReason ToHealthReason( this int value )
    {
        return (HealthReason)value;
    }
}
