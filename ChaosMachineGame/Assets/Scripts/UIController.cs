using UnityEngine;

public class UIController : MonoBehaviour
{
    public StateMachine stateMachine;

    [Header("HUD GameObjects")]
    public GameObject runHUD;
    public GameObject flyHUD;
    public GameObject pauseHUD;
    public GameObject gameOverHUD;

    void Start()
    {
        if (stateMachine == null)
        {
            Debug.LogError("StateMachine não atribuída no UIController!", this);
            return;
        }
        stateMachine.OnEnterRunState += HandleRunState;
        stateMachine.OnEnterFlyState += HandleFlyState;
        stateMachine.OnEnterPauseState += HandlePauseState;
        stateMachine.OnEnterGameOverState += HandleGameOverState;

        UpdateHUD(stateMachine.CurrentState);
    }

    private void HandleRunState()
    {
        UpdateHUD(StateMachine.State.Run);
    }

    private void HandleFlyState()
    {
        UpdateHUD(StateMachine.State.Fly);
    }

    private void HandlePauseState()
    {
        UpdateHUD(StateMachine.State.Pause);
    }
    private void HandleGameOverState()
    {
        UpdateHUD(StateMachine.State.GameOver);
    }

    private void UpdateHUD(StateMachine.State currentState)
    {
        if (runHUD != null) runHUD.SetActive(false);
        if (flyHUD != null) flyHUD.SetActive(false);
        if (pauseHUD != null) pauseHUD.SetActive(false);
        if (gameOverHUD != null) gameOverHUD.SetActive(false);

        switch (currentState)
        {
            case StateMachine.State.Run:
                if (runHUD != null) runHUD.SetActive(true);
                break;
            case StateMachine.State.Fly:
                if (flyHUD != null) flyHUD.SetActive(true);
                break;
            case StateMachine.State.Pause:
                if (pauseHUD != null) pauseHUD.SetActive(true);
                break;
            case StateMachine.State.GameOver:
                if (gameOverHUD != null) gameOverHUD.SetActive(true);
                break;
        }
        Debug.Log($"UIController: HUD para estado {currentState} ativado.");
    }

    private void OnDestroy()
    {
        if (stateMachine != null)
        {
            stateMachine.OnEnterRunState -= HandleRunState;
            stateMachine.OnEnterFlyState -= HandleFlyState;
            stateMachine.OnEnterPauseState -= HandlePauseState;
            stateMachine.OnEnterGameOverState -= HandleGameOverState;
        }
    }
}