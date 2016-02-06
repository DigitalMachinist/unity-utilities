State Machine
=============

Part of the [unity-utilities](https://github.com/DigitalMachinist/unity-utilities) GitHub repo by [@DigitalMachinist](https://github.com/DigitalMachinist).

This library is based on [this](https://www.reddit.com/r/Unity3D/comments/39eh4x/tutorial_state_machine_behaviours_discouple/) thread from reddit in which [/u/LightStriker_Qc](https://www.reddit.com/user/LightStriker_Qc) proposed implementing an event-based interface to Mecanim states and [/u/loolo78](https://www.reddit.com/user/loolo78) implemented [this](https://www.youtube.com/watch?v=GjwoyqNdimY) as a proof of concept to expose a Mecanic state machine to a Unity application via C# events. This implementation builds upon /u/loolo78's work and expands the possibilities this kind of system affords significantly by making it possible to start and end control contexts without crossover between states, even when transition times are used crossfade visual elements such as UI screens.

This library is compatible with both Unity Free and Unity Pro and should run on Unity versions back to 3.x (and possibly earlier).

## The Problem

Managing the high-level state of a production quality game is no small task. There are many ways one can tackle this, and I have often resorted to libraries like [Stateless](https://github.com/dotnet-state-machine/stateless) to provide me with a state machine robust enough to make this manageable. However, as soon as I get into building UI screens for each state (typically using Mecanim to crossfade my screens using the animation system), I have to make sure that the state machine I'm using to control things keeps this Mecanim UI state machine in sync or all hell breaks loose.

## /u/loolo78's Solution

The work done by /u/loolo78 is great and accomplishes a lot! It makes it possible for Mecanim ```StateMachineBehaviour```s to be used to handle the state logic that my Stateless state machine would have performed and relieves me from trying to keep 2 separate state machines synchronized constantly. Furthermore, it exposes the state change events that occur inside the animator to the rest of the application, which can be handy as well.

However, there are some pain points still. Most annoying (in my mind) is that I am unable to set up transition times for my animation states to allow my UI screens to transition smoothly or I will end up with controls enabled for both screens at the same time during the transition because ```OnStateEnter``` will be called for the next state before ```OnStateExit``` is called for the initial state. This doesn't sound like a big deal, but it can get really confusing for players and can lead to some really unusual problems.

If I want to cleanly manage control context, I still need Stateless to take the lead for the Mecanim state machine so I don't have multiple control contexts in effect simultaneously. Obviously, this means **I don't really save any effort at all!**

## Taking Control

My mission when developing this state machine implementation was to solve this control issue so that I can handle UI screen animation, state logic and control context all at the same time, in the same place.

My ```State``` class derives from ```StateMachineBehaviour``` and implements 3 new methods in addition to ```OnStateEnter```, ```OnStateUpdate``` and ```OnStateExit```:

```csharp
public virtual void OnControlEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex );
public virtual void OnControlUpdate( Animator animator, AnimatorStateInfo stateInfo, int layerIndex );
public virtual void OnControlExit( Animator animator, AnimatorStateInfo stateInfo, int layerIndex );
```

If you write state scripts that derive from ```State```, you can override these methods to gain access to specially-timed events that guarantee that:
1. The next state's ```OnControlEnter``` is always called immediately after ```OnStateExit``` for the current state.
2. The previous state's ```OnControlExit``` is always called immediately after ```OnStateEnter``` for the current state.
3. ```OnControlExit``` is always called immediately after ```OnStateEnter```.

As a consequence of these guarantees, there will be a gap during state transitions where no control context is in effect. **This is enormously useful!** No longer do players experience weird control glitches or bugs during state transitions, as long as you use ```OnControlEnter``` and ```OnControlExit``` to set up and tear down your control contexts for each state!

## Usage

There are two classes in this library:

1. ```StateMachine```
2. ```State```

To make use of this library and start managing your state entirely with Mecanim, you'll need to set up a ```GameObject``` that contains all of the UI screens that you need to transition between and add both an ```Animator``` components and my custom component ```StateMachine``` to it.

The ```StateMachine``` component acts as an intermediary between ```State``` behaviour scripts and is necessary for the control context methods described above to be called. It provides events that give the rest of the application access to the state behaviour events emitted by each of the ```Animator```'s states. '*If no ```StateMachine``` component is a sibling to the ```Animator``` and error will be logged during each ```OnStateEnter``` and none of the control methods will be called.*

Create an animation controller and attach it to the ```Animator```. Set up the states and state transitions you need. Create and add a new class deriving from ```State``` to each state that needs special behaviour.

The ```State``` class gets all of the boilerplate to emit control events and handle errors out of the way so you don't have to write any. All you need to do is make sure that your ```base.<MethodName>()``` calls are in place to tie into ```State``` and it will handle the rest.

Try to follow this advice and your life will be easier:

 - Enable each state's control context (adding listening for control events) using ```OnControlEnter```
 - Disable each state's control context (removing listeners for control events) using ```OnControlExit```
 - Perform any control polling (stuff events aren't well-suited to) in ```OnControlUpdate```
 - Set up any special non-control state in ```OnStateEnter```
 - Clean up after any special non-control state in ```OnStateExit```
 - Perform any special per-frame state login in ```OnStateUpdate```
 - **Make sure to call the base methods!**

Any that's about it! Feel free to check out the source code if you need to know more.
