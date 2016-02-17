using System;
using System.Collections;

public class PersistentHealthEffect
{
    public string Key;
    public string Description;
    public float Duration;
    public float Period;
    public int Reason;
    public Func<Health, float> EffectBeginFunc;
    public Func<Health, float> EffectProcFunc;
    public Func<Health, float> EffectEndFunc;
    public Func<Health, bool> ResistFunc;
    public IEnumerator EffectCoroutineHandle;

    public bool HasBeginEffect
    {
        get { return ( EffectBeginFunc != null ); }
    }
    public bool HasProcEffect
    {
        get { return ( EffectProcFunc != null ); }
    }
    public bool HasEndEffect
    {
        get { return ( EffectEndFunc != null ); }
    }
    public bool HasResistFunc
    {
        get { return ( ResistFunc != null ); }
    }
    public bool IsOver
    {
        get { return ( EffectCoroutineHandle == null ); }
    }

    public float GetBeginEffect( Health health )
    {
        return
            HasBeginEffect
                ? EffectBeginFunc( health )
                : 0;
    }
    public float GetProcEffect( Health health )
    {
        return
            HasProcEffect
                ? EffectProcFunc( health )
                : 0;
    }
    public float GetEndEffect( Health health )
    {
        return
            HasEndEffect
                ? EffectEndFunc( health )
                : 0;
    }
    public bool GetResist( Health health )
    {
        return
            HasResistFunc
                ? ResistFunc( health )
                : false;
    }

    public PersistentHealthEffect(
        string key,
        string description,
        float duration,
        float period,
        int reason,
        Func<Health, float> effectBeginFunc = null,
        Func<Health, float> effectProcFunc = null,
        Func<Health, float> effectEndFunc = null,
        Func<Health, bool> resistFunc = null
    )
    {
        if ( duration <= 0 )
        {
            throw new ArgumentOutOfRangeException( "duration", "Must be larger than 0." );
        }
        if ( period <= 0 )
        {
            throw new ArgumentOutOfRangeException( "period", "Must be larger than 0." );
        }

        Key = key;
        Description = description;
        Duration = duration;
        Period = period;
        Reason = reason;
        EffectBeginFunc = effectBeginFunc;
        EffectProcFunc = effectProcFunc;
        EffectEndFunc = effectEndFunc;
        ResistFunc = resistFunc;
    }
}
