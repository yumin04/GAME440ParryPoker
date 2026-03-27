using System;
using Unity.Netcode;

public static class GameEvents
{
    public static Action OnRoundStart;
    // Call this when the game/round is finished (e.g. someone wins). The Go Home button will show.
    public static Action OnRoundEnd;

    public static Action OnGameEnd;
    
    public static Action OnLoadResultScene;
    
    public static Action HideAllInstantiatedCards;
    public static Action DestroyAllInstantiatedCards;

    public static Action OnHavingPriority;
    public static Action OnLosingPriority;
    
    public static Action OnSubRoundEnd;
    
    public static Action OnKeepClicked;
    public static Action<ulong, int> OnPlayerKeepCard;
    public static Action OnAttackClicked;
    public static Action HideWaitAndAttackPanel;

    public static Action OnAttackStart;

    public static Action OnAllLeversDown;
    public static Action OnSlotMachineFinished;
    
    
    public static Action OnAttackEnd;
    
    
    // 안사용중
    // public static Action KeepCard;



}
