using GameStates.States;
using GenericHelpers;

namespace GameStates {
	public static class UserStateHandler {
		public static IUserState CurrentState;

		static UserStateHandler() {
			CurrentState = new DefaultState();
		}

		public static void ChangeState(IUserState newState) {
			CurrentState.OnExit();
			CurrentState = newState;
			CurrentState.OnEnter();
		}
	}
}
