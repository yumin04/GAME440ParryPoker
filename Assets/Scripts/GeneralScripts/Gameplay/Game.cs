using GenericHelpers;
using Managers.GameEssentials;
using Unity.Netcode;

namespace GeneralScripts.Gameplay {
	public class Game : NetworkSingleton<Game> {
		// 이거 NetworkVariable들로 만들어도 괜찮을듯?
		// 이것도 다음에 OnValueChange같은거써서
		// StartRoundAgain같은거 다시 만든다음에 
		private readonly NetworkVariable<int> player1Health = new();
		private readonly NetworkVariable<int> player2Health = new();
		private readonly NetworkVariable<int> roundNumber = new();

		private NetworkObject roundObject;
		// TODO: changeToStartRound가 바뀔때에 UserInterface.Instance.DisplayRoundNumber(roundNumber); 하면 됨

		public void OnEnable() {
			GameEvents.OnRoundEnd += OnRoundEnd;
		}

		public void OnDisable() {
			GameEvents.OnRoundEnd -= OnRoundEnd;
		}

		public override void OnNetworkSpawn() {
			base.OnNetworkSpawn();

			// TODO: 이거 아닌가?
			roundNumber.OnValueChanged += StartRound;

			// TODO: 이것도
			player1Health.OnValueChanged += OnPlayer1TookDamage;
			player2Health.OnValueChanged += OnPlayer2TookDamage;
			// show opponent character
			// show opponent health

			// Canvas랑 UI를 통한 간단한 animation으로 

			// TODO:
			// Host is always 0
			// Refactor Later
			var isPlayer1 = (NetworkManager.Singleton.LocalClientId == 0);
			UserInterface.Instance.Init(isPlayer1);

			UserInterface.Instance.DisplayVS();
			UserInterface.Instance.EnableRoundNumber();
			if (!IsServer) return;
			player1Health.Value = 100;
			player2Health.Value = 100;

			roundNumber.Value = 0;
		}

		public void DecreasePlayerHealth(ulong clientId, int amount) {
			DecreasePlayerHealthRPC(clientId, amount);
		}

		[Rpc(SendTo.Server)]
		private void DecreasePlayerHealthRPC(ulong clientId, int amount) {
			if (clientId == 0)
				DecreasePlayer1Health(amount);
			else
				DecreasePlayer2Health(amount);
		}

		private void DecreasePlayer1Health(int amount) {
			// TODO:
			// is Server?
			// 이미 already Server이긴 할텐데
			player1Health.Value -= amount;
		}

		private void DecreasePlayer2Health(int amount) {
			// TODO: 
			// is Server?
			player2Health.Value -= amount;
		}

		private void OnPlayer1TookDamage(int previousValue, int newValue) {
			// TODO:
			// Damage얼마나 받았는지 indicator 적고
			// DamageIndicator

			// TODO:
			// Hit Animation Play하고
			//

			UserInterface.Instance.ChangePlayer1Health(newValue);
			// 
			if (newValue > 0 || !IsServer) return;
			// TODO: This must be sent to each client via RPC
			// This isn't RPC.
			UserInterface.Instance.ChangePlayer1Health(0);
			UserInterface.Instance.Player2Win();
			// This *is* RPC
			EndGame();
		}

		private void OnPlayer2TookDamage(int previousValue, int newValue) {
			// TODO:
			// Damage얼마나 받았는지 indicator 적고
			// DamageIndicator

			// TODO:
			// Hit Animation play하고

			UserInterface.Instance.ChangePlayer2Health(newValue);
			// 
			if (newValue > 0 || !IsServer) return;
			// 이거는 RPC가 아니야
			UserInterface.Instance.ChangePlayer2Health(0);
			UserInterface.Instance.Player1Win();
			// 이거는 RPC인데
			EndGame();
		}

		public void StartGame() {
			// 여기서 player health display 올리고

			// TODO: 이거 맞는지 확인
			// 어차피 둘다 돌려지는건 사실이라
			// 이건 둘다 해야하는거고
			UserInterface.Instance.DisplayHealth();
			UserInterface.Instance.ChangePlayer1Health(player1Health.Value);
			UserInterface.Instance.ChangePlayer2Health(player2Health.Value);

			if (!IsServer) return;
			// CheckForHealth부분을 통한 StartRound를 시작하면 되지 않을까?
			roundNumber.Value += 1;
		}

		/*private bool CheckForWinner() {
			return false;
		}*/

		public void StartRound(int previousValue, int newValue) {
			// Slowly change camera position of each player
			GameEvents.OnRoundStart.Invoke();
			UserInterface.Instance.ChangeRoundNumber(newValue);
			if (!IsServer) return;
			if (roundObject) roundObject.Despawn();

			// After all animation and visuals are done, spawn round
			roundObject = GameInitializer.Instance.SpawnRound();
		}

		private void OnRoundEnd() {
			OnRoundEndRPC();
		}

		[Rpc(SendTo.Server)]
		private void OnRoundEndRPC() {
			roundNumber.Value += 1;
		}

		public void EndGame() {
			UserInterface.Instance.HideHealthDisplay();
			UserInterface.Instance.DisableRoundNumber();
			UserInterface.Instance.DisableSubRoundNumber();
			// TODO: Display Winner Here?
			if (IsServer) {
				EndGame_Server();
			}
			else {
				RequestEndGameRpc();
			}
		}

		[Rpc(SendTo.Server)]
		private void RequestEndGameRpc() {
			EndGame_Server();
		}

		private void EndGame_Server() {
			if (!IsServer) return;

			GameEvents.OnGameEnd?.Invoke();

			NetworkManager.SceneManager.LoadScene("ResultScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
		}
	}
}
