using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class HealthEvent : UnityEvent<Health, float> { }

[Serializable]
public class FoldableHealthEvent : FoldableEvent<HealthEvent, Health, float> { }

[Serializable]
public class HealthEffectEvent : UnityEvent<Health, PersistentHealthEffect, float> { }

[Serializable]
public class FoldableHealthEffectEvent : FoldableEvent<HealthEffectEvent, Health, PersistentHealthEffect, float> { }

[Serializable]
public class Health : MonoBehaviour
{
    #region Fields / Events / Properties
    
    [Header( "Configuration" )]
    [Tooltip( "The current number of HP." )]
    public float HP = 100f;
    [Tooltip( "The total amount of HP available." )]
    public float MaxHP = 100f;
    [Tooltip( "How many HP can you REALLY have (when supercharged)?" )]
    public float SuperMaxHP = 100f;
    [Tooltip( "What fraction of HP is considered \"in-danger\"?" )]
    [Range( 0f, 1f )]
    public float HPDangerFraction = 0.25f;
    [Tooltip( "History period (seconds)" )]
    [Range( 0f, 10f )]
    public float HistorySeconds = 2f;

    [Header( "Events" )]
    public FoldableHealthEvent Damaged;
    public FoldableHealthEvent Dead;
    public FoldableHealthEvent Healed;
    public FoldableHealthEvent HPChanged;
    public FoldableHealthEvent HPFull;
    public FoldableHealthEvent HPInDanger;
    public FoldableHealthEvent HPSafe;
    public FoldableHealthEvent HPSupercharged;
    public FoldableHealthEvent Revived;
    public FoldableHealthEffectEvent DegenBegan;
    public FoldableHealthEffectEvent DegenEnded;
    public FoldableHealthEffectEvent DegenProc;
    public FoldableHealthEffectEvent RegenBegan;
    public FoldableHealthEffectEvent RegenEnded;
    public FoldableHealthEffectEvent RegenProc;

    // Properties
    public Dictionary<string, PersistentHealthEffect> DegenEffects { get; private set; }
    public HealthHistory History { get; private set; }
    public Dictionary<string, PersistentHealthEffect> RegenEffects { get; private set; }

    // Computed Properties
    public float DangerHP
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
    public bool GetIsDead( float hp )
    {
        return ( hp <= 0 );
    }
    public bool GetIsFull( float hp )
    {
        return ( hp >= MaxHP );
    }
    public bool GetIsInDanger( float hp )
    {
        return ( hp <= DangerHP );
    }
    public bool GetIsSafe( float hp )
    {
        return !GetIsInDanger( hp );
    }
    public bool GetIsSupercharged( float hp )
    {
        return ( hp >= MaxHP );
    }

    #endregion

    #region Unity Messages

    void Awake()
    {
        History = new HealthHistory( HistorySeconds );
    }

    #endregion


    #region Public API

    public float ApplyDamage( float hpLost, int reason )
    {
        if ( IsDead )
        {
            Debug.LogWarning( "Revive this first to return it to life at 1 HP first!" );
            return 0;
        }

        var initial = HP;
        HP -= hpLost;
        History.Add( -hpLost, reason );

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

    public float ApplyHealing( float hpGained, int reason, bool isSuper = false )
    {
        if ( IsDead )
        {
            Debug.LogWarning( "Revive this first to return it to life at 1 HP first!" );
            return 0;
        }

        var hpGainedReally = 
            isSuper                                      // If this is a super-heal, 
                ? Mathf.Min( hpGained, SuperMaxHP - HP ) // Allow healing up to MaxMaxHP.
                : IsFull                                 // If not AND HP is full, 
                    ? 0                                  // Healing has no effect.
                    : Mathf.Min( hpGained, MaxHP - HP ); // All healing up to MaxHP.

        var initial = HP;
        HP += hpGainedReally;
        History.Add( hpGainedReally, reason );

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

    public void SetHP( int hp, int reason )
    {
        var diff = hp - HP;
        if ( diff > 0 )
        {
            ApplyHealing( diff, reason, true );
        }
        else if ( diff < 0 )
        {
            ApplyDamage( -diff, reason );
        }
    }

    public void Revive( int reason )
    {
        SetHP( 1, reason );
    }

    public void ApplyDegen( 
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
        var effect = new PersistentHealthEffect( 
            key, description, duration, period, reason, effectBeginFunc, effectProcFunc, effectEndFunc, resistFunc 
        );
        ApplyEffect( effect, DegenEffects, DegenBegan, DegenProc, DegenEnded );
    }

    public void ApplyRegen( 
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
        var effect = new PersistentHealthEffect(
            key, description, duration, period, reason, effectBeginFunc, effectProcFunc, effectEndFunc, resistFunc
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
        FoldableHealthEffectEvent beginEvent,
        FoldableHealthEffectEvent procEvent,
        FoldableHealthEffectEvent endEvent
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
        FoldableHealthEffectEvent endEvent 
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
        FoldableHealthEffectEvent procEvent,
        FoldableHealthEffectEvent endEvent
    )
    {
        var endTime = Time.time + effect.Duration;
        while ( Time.time >= endTime )
        {
            if ( effect.Period > 0f )
            {
                yield return new WaitForSeconds( effect.Period );
            }
            var hpGained = effect.GetProcEffect( this );
            if ( hpGained > 0 )
            {
                ApplyHealing( hpGained, effect.Reason );
            }
            else if ( hpGained < 0 )
            {
                ApplyDamage( -hpGained, effect.Reason );
            }
            procEvent.Invoke( this, effect, hpGained );
        }
        RemoveEffect( effect.Key, effectMap, endEvent );
    }

    #endregion
}
