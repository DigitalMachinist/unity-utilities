Foldable Events
===============

Part of the [unity-utilities](https://github.com/DigitalMachinist/unity-utilities) GitHub repo by [@DigitalMachinist](https://github.com/DigitalMachinist).

This library contains two helpful classes that make using ```UnityEvent```s soak up less room in the Unity Inspector, and unlock new powerful strategies for managing difficult-to-reason-about dependency graphs like complex scene loads.

Your inspector could look like this:

![Nice clean events that don't scroll forever!](https://raw.githubusercontent.com/DigitalMachinist/unity-utilities/master/Assets/Utilities/Foldable%20Events/FoldableEventInspector.PNG)


## FoldableEvent

Simply a derived class from ```UnityEvent``` that can fold up to consume less space in the Inspector.

## FoldablePromise

A derived class from ```FoldableEvent``` that, once invoked, will automatically execute any new subscribers with the same arguments that were supplied to the original invocation. This is tremendously useful for sorting out dependency chains that develop surrounding complex scene loading processes.

Promises allow you to easily signal the readiness of resources and chain together dependencies such that they always load in the correct order and you can clearly see what depends on what without a lot of effort. This technique comes from my time working is JS as a webdev guy, and it's been tremendously helpful when applied to situations where previously script ordering was all I could do to sort things out.

## Usage

For situations where the ```UnityEvent``` that you need is the standard, non-generic ```UnityEvent``` that passes no arguments to its subscribers, simply use ```FoldableEvent``` or ```FoldablePromise``` instead of ```UnityEvent```.

```csharp
public class FoldableEventTest : MonoBehaviour
{
  public FoldableEvent Event;
  public FoldablePromise Promise;
}
```

In cases where you need the power of generics to pass arguments to event subscribers, you'll need to define a couple of types outside of your ```MonoBehaviour``` derived class to get the Unity Inspector to co-operate.

If you have used generic ```UnityEvent```s before this should come as no surprise, since this is always a necessary step when using ```UnityEvent<T>``` in your code such that it also appears in the Inspector.

For example, this is how can use ```UnityEvent<int>``` in your code and allow it to appear in the Inspector:

```csharp
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class Event2 : UnityEvent<int> { }

public class FoldableEventTest : MonoBehaviour
{
  public Event2 Event;
}
```

To do the same with ```FoldableEvent``` or ```FoldablePromise```, you'll have to define another type by passing your custom ```UnityEvent<T>``` derived type as a type argument, followed by each of the callback argument types it takes, like so:

```csharp
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class Event2 : UnityEvent<int> { }
[Serializable] public class FoldableEvent2 : FoldableEvent<Event2, int> { }
[Serializable] public class FoldablePromise2 : FoldablePromise<Event2, int> { }

public class FoldableEventTest : MonoBehaviour
{
  public FoldableEvent2 Event;
  public FoldablePromise2 Promise;
}
```

Yeah, it's a little bit of a hassle (thanks Unity!), but it's worth your time. The power of events in the Inspector is huge. Technical and non-technical people alike can suddenly accomplish a lot with little effort by making good use of these features!
