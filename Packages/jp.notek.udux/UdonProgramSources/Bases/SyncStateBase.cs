using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace JP.Notek.Udux
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SyncStateBase : ISyncState
    {
        public bool Initialized = false;
        public VRCPlayerApi Owner { get { return GetOwner(); } }
        public bool IsOwner { get { return GetIsOwner(); } }
        IReduceStore _StoreSubscribed = null;
        void Start()
        {
            if (IsOwner)
            {
                SetOwnerTimeDifference();
                if (_StoreSubscribed != null)
                    _StoreSubscribed.OnSyncStateChanged();
            }
        }

        public void TakeOwnership()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }

        public void Subscribe(IReduceStore store)
        {
            _StoreSubscribed = store;
        }

        public override void OnDeserialization()
        {
            Initialized = true;
            if (_SyncTimeIdLocal != _SyncTimeId)
            {
                GetOwnerTimeDifference();
            }
            if (_StoreSubscribed != null)
                _StoreSubscribed.OnSyncStateChanged();
        }

        public override void OnOwnershipTransferred(VRCPlayerApi player)
        {
            if (_StoreSubscribed != null)
                _StoreSubscribed.OnSyncStateOwnershipTransferred();
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (IsOwner)
            {
                RequestSerialization();
            }
        }
    }
}