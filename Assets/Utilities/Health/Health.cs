using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class HealthEvent : UnityEvent<Health, int> { }
[Serializable] public class HealthEffectEvent : UnityEvent<Health, PersistentHealthEffect, int> { }

public class PersistentHealthEffect
{
    public string Key;
    public string Description;
    public float Duration;
    public float Period;
    public Func<Health, int> EffectBeginFunc;
    public Func<Health, int> EffectProcFunc;
    public Func<Health, int> EffectEndFunc;
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

    public int GetBeginEffect( Health health )
    {
        return
            HasBeginEffect
                ? EffectBeginFunc( health )
                : 0;
    }
    public int GetProcEffect( Health health )
    {
        return
            HasProcEffect
                ? EffectProcFunc( health )
                : 0;
    }
    public int GetEndEffect( Health health )
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
        Func<Health, int> effectBeginFunc = null,
        Func<Health, int> effectProcFunc = null,
        Func<Health, int> effectEndFunc = null,
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
        EffectBeginFunc = effectBeginFunc;
        EffectProcFunc = effectProcFunc;
        EffectEndFunc = effectEndFunc;
        ResistFunc = resistFunc;
    }
}

public class Health : MonoBehaviour
{
    #region Fields / Events / Properties

    // Public / Inspector
    [Tooltip( "The current number of HP." )]
    public int HP;
    [Tooltip( "The total amount of HP available." )]
    public int MaxHP;
    [Tooltip( "Let's get real. How many HP can you REALLY have?" )]
    public int MaxMaxHP;
    [Tooltip( "What fraction of HP is considered \"in-danger\"?" )]
    [Range( 0f, 1f )]
    public float HPDangerFraction = 0.3f;

    // Events
    public HealthEvent Damaged;
    public HealthEvent Dead;
    public HealthEvent Healed;
    public HealthEvent HPChanged;
    public HealthEvent HPFull;
    public HealthEvent HPInDanger;
    public HealthEvent HPSafe;
    public HealthEvent HPSupercharged;
    public HealthEvent Revived;
    public HealthEffectEvent DegenBegan;
    public HealthEffectEvent DegenEnded;
    public HealthEffectEvent DegenProc;
    public HealthEffectEvent RegenBegan;
    public HealthEffectEvent RegenEnded;
    public HealthEffectEvent RegenProc;

    // Properties
    public Dictionary<string, PersistentHealthEffect> DegenEffects { get; private set; }
    public Dictionary<string, PersistentHealthEffect> RegenEffects { get; private set; }

    // Computed Properties
    public int DangerHP
    {
        get { return Mathf.RoundToInt( HPDangerFraction * MaxHP ); }
    }
    public bool IsDead
    {
        get { return GetIsDead( HP ); }
    }
    public bool IsFull
    {
        get { return GetIsFull( HP ); }
    }
    public bool IsInDanger
    {
        get { return GetIsInDanger( HP ); }
    }
    public bool IsSafe
    {
        get { return !GetIsInDanger( HP ); }
    }
    public bool IsSupercharged
    {
        get { return GetIsSupercharged( HP ); }
    }

    // Mini Helper Functions
    public bool GetIsDead( int hp )
    {
        return ( hp <= 0 );
    }
    public bool GetIsFull( int hp )
    {
        return ( hp >= MaxHP );
    }
    public bool GetIsInDanger( int hp )
    {
        return ( hp <= DangerHP );
    }
    public bool GetIsSafe( int hp )
    {
        return !GetIsInDanger( hp );
    }
    public bool GetIsSupercharged( int hp )
    {
        return ( hp >= MaxHP );
    }

    #endregion


    #region Public API

    public int ApplyDamage( int hpLost )
    {
        if ( IsDead )
        {
            Debug.LogWarning( "Revive this first to return it to life at 1 HP first!" );
            return 0;
        }

        var initial = HP;
        HP -= hpLost;

        HPChanged.Invoke( this, -hpLost );
        Damaged.Invoke( this, -hpLost );

        if ( IsDead && !GetIsDead( initial ) )
        {
            Dead.Invoke( this, -hpLost );
        }
        else if ( IsInDanger && !GetIsInDanger( initial ) )
        {
            HPInDanger.Invoke( this, -hpLost );
        }

        return -hpLost;
    }

    public int ApplyHealing( int hpGained, bool isSuper = false )
    {
        if ( IsDead )
        {
            Debug.LogWarning( "Revive this first to return it to life at 1 HP first!" );
            return 0;
        }

        var initial = HP;
        HP += 
            isSuper                                      // If this is a super-heal, 
                ? Mathf.Min( hpGained, MaxMaxHP - HP )   // Allow healing up to MaxMaxHP.
                : IsFull                                 // If not AND HP is full, 
                    ? 0                                  // Healing has no effect.
                    : Mathf.Min( hpGained, MaxHP - HP ); // All healing up to MaxHP.

        if ( HP - initial == 0 )
        {
            return 0;
        }

        HPChanged.Invoke( this, hpGained );
        Healed.Invoke( this, hpGained );

        if ( IsSupercharged )
        {
            // Do this each time, allowing repeats. Also, both full and supercharged
            HPSupercharged.Invoke( this, hpGained );
        }

        if ( IsFull && !GetIsFull( initial ) )
        {
            HPFull.Invoke( this, hpGained );
        }
        else if ( IsSafe && !GetIsSafe( initial ) )
        {
            HPSafe.Invoke( this, hpGained );
        }

        return hpGained;
    }

    public void SetHP( int hp )
    {
        var diff = hp - HP;
        if ( diff > 0 )
        {
            ApplyHealing( diff );
        }
        else if ( diff < 0 )
        {
            ApplyDamage( -diff );
        }
    }

    public void Revive()
    {
        SetHP( 1 );
    }

    public void ApplyDegen( 
        string key, 
        string description, 
        float duration, 
        float period,
        Func<Health, int> effectBeginFunc = null,
        Func<Health, int> effectProcFunc = null,
        Func<Health, int> effectEndFunc = null,
        Func<Health, bool> resistFunc = null
    )
    {
        var effect = new PersistentHealthEffect( 
            key, description, duration, period, effectBeginFunc, effectProcFunc, effectEndFunc, resistFunc 
        );
        ApplyEffect( effect, DegenEffects, DegenBegan, DegenProc, DegenEnded );
    }

    public void ApplyRegen( 
        string key,
        string description,
        float duration, 
        float period,
        Func<Health, int> effectBeginFunc = null,
        Func<Health, int> effectProcFunc = null,
        Func<Health, int> effectEndFunc = null,
        Func<Health, bool> resistFunc = null
    )
    {
        var effect = new PersistentHealthEffect(
            key, description, duration, period, effectBeginFunc, effectProcFunc, effectEndFunc, resistFunc
        );
        ApplyEffect( effect, RegenEffects, RegenBegan, RegenProc, RegenEnded );
    }

    public void RemoveDegen( string key )
    {
        RemoveEffect( key, DegenEffects, DegenEnded );
    }

    public void RemoveRegen( string key )
    {
        RemoveEffect( key, RegenEffects, RegenEnded );
    }

    #endregion


    #region Private Helpers

    void ApplyEffect( 
        PersistentHealthEffect effect, 
        Dictionary<string, PersistentHealthEffect> effectMap, 
        HealthEffectEvent beginEvent,
        HealthEffectEvent procEvent,
        HealthEffectEvent endEvent
    )
    {
        if ( effectMap.ContainsKey( effect.Key ) )
        {
            RemoveEffect( effect.Key, effectMap, endEvent );
        }
        effectMap.Add( effect.Key, effect );
        beginEvent.Invoke( this, effect, effect.EffectBeginFunc( this ) );
        effect.EffectCoroutineHandle = EffectCoroutine( effect, effectMap, procEvent, endEvent );
        StartCoroutine( effect.EffectCoroutineHandle );
    }

    bool RemoveEffect( 
        string key, 
        Dictionary<string, PersistentHealthEffect> effectMap,
        HealthEffectEvent endEvent 
    )
    {
        if ( !effectMap.ContainsKey( key ) )
        {
            return false;
        }
        var effect = effectMap[ key ];
        if ( effect.EffectCoroutineHandle != null )
        {
            StopCoroutine( effect.EffectCoroutineHandle );
            effect.EffectCoroutineHandle = null;
        }
        endEvent.Invoke( this, effect, effect.GetEndEffect( this ) );
        effectMap.Remove( key );
        return true;
    }

    IEnumerator EffectCoroutine( 
        PersistentHealthEffect effect,
        Dictionary<string, PersistentHealthEffect> effectMap, 
        HealthEffectEvent procEvent,
        HealthEffectEvent endEvent
    )
    {
        var endTime = Time.time + effect.Duration;
        while ( Time.time >= endTime )
        {
            yield return new WaitForSeconds( effect.Period );
            var hpGained = effect.GetProcEffect( this );
            if ( hpGained > 0 )
            {
                ApplyHealing( hpGained );
            }
            else if ( hpGained < 0 )
            {
                ApplyDamage( -hpGained );
            }
            procEvent.Invoke( this, effect, hpGained );
        }
        RemoveEffect( effect.Key, effectMap, endEvent );
    }

    #endregion
}
