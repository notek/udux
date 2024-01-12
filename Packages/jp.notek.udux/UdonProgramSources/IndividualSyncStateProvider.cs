
using System.Linq;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace JP.Notek.Udux
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class IndividualSyncStateProvider : IIndividualSyncStateObservable
    {
        [SerializeField] public IndividualSyncStateBase[] SyncStates;
        public int MySyncStateIndex = -1;
        IReduceStore _StoreSubscribed = null;

        void Start()
        {
            for (int i = 0; i < SyncStates.Length; i++)
            {
                SyncStates[i].index = i;
                SyncStates[i].Subscribe(this);
            }
        }

        public void Subscribe(IReduceStore store)
        {
            _StoreSubscribed = store;
        }

        public int GetSyncStateIndex(int userId)
        {
            for (int i = 0; i < SyncStates.Length; i++)
            {
                if (SyncStates[i].UserId == userId)
                    return i;
            }
            return -1;
        }

        public int[] GetAvailableSyncStateIndex()
        {
            int[] availables = {};
            for (int i = 0; i < SyncStates.Length; i++)
            {
                if (SyncStates[i].UserId != -1)
                    availables.Add(i);
            }
            return availables;
        }

        public override void OnIndividualSyncStateChanged(int index)
        {
            if (_StoreSubscribed != null)
                _StoreSubscribed.OnIndividualSyncStateChanged(index);
        }
        public override void OnIndividualSyncStateOwnershipTransferred(int index)
        {
            if (_StoreSubscribed != null)
                _StoreSubscribed.OnIndividualSyncStateOwnershipTransferred(index);
        }

        public override void OnIndividualSyncStateOwnershipGiven(int index)
        {
            MySyncStateIndex = index;
            if (_StoreSubscribed != null)
                _StoreSubscribed.OnIndividualSyncStateOwnershipGiven(index);
        }

        public override void OnOwnershipTransferred(VRCPlayerApi player)
        {
            if (player.isLocal)
            {
                VRCPlayerApi[] players = new VRCPlayerApi[100];
                VRCPlayerApi.GetPlayers(players);
                foreach (var syncState in SyncStates)
                {
                    if (syncState.UserId != -1)
                    {
                        bool used = false;

                        foreach (var p in players)
                        {
                            if (syncState.UserId == p.playerId)
                            {
                                used = true;
                                break;
                            }
                        }
                        if (!used)
                        {
                            syncState.UnsetUser();
                        }
                    }
                }
            }
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (GetIsOwner())
            {
                foreach (var syncState in SyncStates)
                {
                    if (syncState.UserId == -1)
                    {
                        syncState.SetUser(player);
                        break;
                    }
                }
            }
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (GetIsOwner())
            {
                foreach (var syncState in SyncStates)
                {
                    if (syncState.UserId == player.playerId)
                    {
                        syncState.UnsetUser();
                        break;
                    }
                }
            }
        }

        private bool GetIsOwner()
        {
            return Networking.IsOwner(Networking.LocalPlayer, gameObject);
        }
    }
}