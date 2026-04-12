using GenericHelpers;
using Unity.Netcode;
using UnityEngine.Serialization;

namespace Managers {
	public class NetworkData : NetworkSingleton<NetworkData> {
		public NetworkList<int> roundCardIDs;
		[FormerlySerializedAs("RoundReady")] public NetworkVariable<bool> roundReady = new();

		public override void OnNetworkSpawn() {
			if (IsServer) roundCardIDs = new NetworkList<int>();
		}
	}
}
