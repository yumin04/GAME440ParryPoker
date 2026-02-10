public static class PlayerStateHandler
{
    public static IState currentState;

    public static void ChangeState(IState newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}