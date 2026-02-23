public static class UserStateHandler
{
    public static IUserState currentState;

    static UserStateHandler()
    {
        currentState = new DefaultState();
    }
    public static void ChangeState(IUserState newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}