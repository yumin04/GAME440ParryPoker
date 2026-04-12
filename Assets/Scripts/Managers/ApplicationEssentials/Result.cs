using GameStates;
using GameStates.States;
using UnityEngine;

namespace Managers.ApplicationEssentials {
	public class Result : MonoBehaviour {
		public void Start() {
			GameStateHandler.ChangeState(new ResultState());
		}
	}
}
