using GenericHelpers;
using Unity.Netcode;
using UnityEngine;

namespace Managers.GameEssentials {
	public class GameInitializer : Singleton<GameInitializer> {
		[SerializeField] private GameObject gamePrefab;
		[SerializeField] private GameObject roundPrefab;
		[SerializeField] private GameObject subRoundPrefab;

		// TODO: IsServer여기서 체크 하지 않아도 괜찮아
		// TODO: 죽이기
		public NetworkObject SpawnGame() {
			return !NetworkManager.Singleton.IsServer ? null : SpawnNetworkObject(gamePrefab);
		}

		public NetworkObject SpawnRound() {
			return !NetworkManager.Singleton.IsServer ? null : SpawnNetworkObject(roundPrefab);
		}

		public NetworkObject SpawnSubRound() {
			return !NetworkManager.Singleton.IsServer ? null : SpawnNetworkObject(subRoundPrefab);
		}

		private static NetworkObject SpawnNetworkObject(GameObject prefab) {
			var instance = Instantiate(prefab);
			
			var networkObject = instance.GetComponent<NetworkObject>();
			networkObject.Spawn();
			return networkObject;
		}
	}
}
