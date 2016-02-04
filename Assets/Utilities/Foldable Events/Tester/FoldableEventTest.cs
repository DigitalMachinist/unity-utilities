using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class Event2 : UnityEvent<int> { }
[Serializable] public class Event3 : UnityEvent<int, int> { }
[Serializable] public class Event4 : UnityEvent<int, int, int> { }
[Serializable] public class Event5 : UnityEvent<int, int, int, int> { }

// Note that the event type is passed as the first type argument, followed by the types of each
// of the arguments accepted by the event. This is necessary to get the inspector to render the 
// event in a foldable container. Since non-serializable and/or generic types don't display in 
// the inspector, we just need to define non-generic and serializable types as below.
[Serializable] public class FoldableEvent2 : FoldableEvent<Event2, int> { }
[Serializable] public class FoldableEvent3 : FoldableEvent<Event3, int, int> { }
[Serializable] public class FoldableEvent4 : FoldableEvent<Event4, int, int, int> { }
[Serializable] public class FoldableEvent5 : FoldableEvent<Event5, int, int, int, int> { }
[Serializable] public class FoldablePromise2 : FoldablePromise<Event2, int> { }
[Serializable] public class FoldablePromise3 : FoldablePromise<Event3, int, int> { }
[Serializable] public class FoldablePromise4 : FoldablePromise<Event4, int, int, int> { }
[Serializable] public class FoldablePromise5 : FoldablePromise<Event5, int, int, int, int> { }

public class FoldableEventTest : MonoBehaviour
{
    public FoldableEvent  Event1;
    public FoldableEvent2 Event2;
    public FoldableEvent3 Event3;
    public FoldableEvent4 Event4;
    public FoldableEvent5 Event5;

    public FoldablePromise  Promise1;
    public FoldablePromise2 Promise2;
    public FoldablePromise3 Promise3;
    public FoldablePromise4 Promise4;
    public FoldablePromise5 Promise5;
}
