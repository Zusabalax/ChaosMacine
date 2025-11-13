using UnityEngine;
using UnityEngine.Events;

public class StateMachine : MonoBehaviour
{
    public static StateMachine Instance;
    public enum State
    {
        Run,
        Fly,
        Pause
    }

    public State CurrentState { get; private set; }

    public event System.Action OnEnterRunState;
    public event System.Action OnEnterFlyState;
    public event System.Action OnEnterPauseState;

    public UnityEvent OnRunStateEnteredUnityEvent;
    public UnityEvent OnFlyStateEnteredUnityEvent;
    public UnityEvent OnPauseStateEnteredUnityEvent;

    private void Start()
    {
        Instance = this;
        SetState(State.Run);
    }

    public void SetState(State newState)
    {
        if (CurrentState == newState)
        {
            return; 
        }

        CurrentState = newState;
        Debug.Log($"Entrando no estado: {CurrentState}");

        switch (CurrentState)
        {
            case State.Run:
                OnEnterRunState?.Invoke();
                OnRunStateEnteredUnityEvent?.Invoke();
                break;
            case State.Fly:
                OnEnterFlyState?.Invoke();
                OnFlyStateEnteredUnityEvent?.Invoke();
                break;
            case State.Pause:
                OnEnterPauseState?.Invoke();
                OnPauseStateEnteredUnityEvent?.Invoke();
                break;
        }
    }
    [ContextMenu("Run State")]
    public void TransitionToRun()
    {
        SetState(State.Run);
    }
    [ContextMenu("Fly State")]
    public void TransitionToFly()
    {
        SetState(State.Fly);
    }
    [ContextMenu("Pause State")]
    public void TransitionToPause()
    {
        SetState(State.Pause);
    }
}