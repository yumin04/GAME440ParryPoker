using GeneralScripts;
using GenericHelpers;
using UnityEngine;

namespace GameStates.States {
	public class ResultState : IGameState {
		public void OnEnter() {
			Debug.Log("OnEnter ResultState");
			GameEvents.OnLoadResultScene?.Invoke();
		}

		public void OnExit() { }
	}
}
