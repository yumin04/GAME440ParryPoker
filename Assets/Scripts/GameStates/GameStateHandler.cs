public static class GameStateHandler
{
    public static IGameState currentState;

    static GameStateHandler()
    {
        currentState = new DefaultState();
    }
    public static void ChangeState(IGameState newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}