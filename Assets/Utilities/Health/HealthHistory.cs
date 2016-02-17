using System.Collections.Generic;
using System.Linq;
using UnityEngine;

struct HealthChange
{
    public float Amount;
    public int Reason;
    public float Timestamp;

    public HealthChange( float amount, int reason, float timestamp )
    {
        Amount = amount;
        Reason = reason;
        Timestamp = timestamp;
    }
}

public class HealthHistory
{
    Queue<HealthChange> data;

    public float Period { get; private set; }

    public Dictionary<int, float> Amounts
    {
        get
        {
            return Reasons.ToDictionary( reason => reason, reason => GetReasonAmount( reason ) );
        }
    }

    public IEnumerable<int> Reasons 
    {
        get
        {
            return data.Select( change => change.Reason );
        }
    }

    public HealthHistory( float period = 1f )
    {
        data = new Queue<HealthChange>();
    }

    public float GetReasonAmount( int reason )
    {
        ClearOldHealthChanges();
        return
            data
                .Where( change => reason == change.Reason )
                .Select( change => change.Amount )
                .Aggregate( ( total, amount ) => total + amount );
    }

    bool IsTooOld( HealthChange healthChange )
    {
        return Time.time > healthChange.Timestamp + Period;
    }

    public void Add( float amount, int reason )
    {
        data.Enqueue( new HealthChange( amount, reason, Time.time ) );
    }

    public void ClearOldHealthChanges()
    {
        while ( IsTooOld( data.Peek() ) )
        {
            data.Dequeue();
        }
    }
}