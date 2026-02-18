using System;

public static class GameEvents
{
    public static Action OnRoundStart;
    // Call this when the game/round is finished (e.g. someone wins). The Go Home button will show.
    public static Action OnRoundEnd;

    public static Action OnGameEnd;
    
    public static Action OnLoadResultScene;
    
    public static Action HideAllInstantiatedCards;
}
