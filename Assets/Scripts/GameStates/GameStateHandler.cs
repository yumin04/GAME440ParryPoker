using GameStates.States;
using GenericHelpers;

namespace GameStates {
	public static class GameStateHandler {
		public static IGameState CurrentState;

		static GameStateHandler() {
			CurrentState = new DefaultState();
		}

		public static void ChangeState(IGameState newState) {
			CurrentState.OnExit();
			CurrentState = newState;
			CurrentState.OnEnter();
		}
	}
}
