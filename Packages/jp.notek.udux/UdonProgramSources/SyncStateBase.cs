using UdonSharp;
using VRC.SDKBase;

namespace JP.Notek.Udux
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SyncStateBase : UdonSharpBehaviour
    {
        public bool Initialized = false;
        public bool IsOwner = false;
        public VRCPlayerApi Owner = null;
        IReduceStore _StoreSubscribed = null;


        public void TakeOwnership()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            Owner = Networking.LocalPlayer;
            IsOwner = true;
        }

        public void SubscribeOnDeserialization(IReduceStore store)
        {
            _StoreSubscribed = store;
        }

        public override void OnDeserialization()
        {
            Initialized = true;
            if (_StoreSubscribed != null)
                _StoreSubscribed.OnSyncStateDeserialization();
        }

        public override void OnOwnershipTransferred(VRCPlayerApi player)
        {
            Owner = player;
            IsOwner = false;
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (player.isLocal)
            {
                Owner = GetOwner();
                IsOwner = GetIsOwner();
            }
            if (IsOwner)
            {
                RequestSerialization();
            }
        }

        private VRCPlayerApi GetOwner()
        {
            return Networking.GetOwner(gameObject);
        }

        private bool GetIsOwner()
        {
            return Networking.IsOwner(Networking.LocalPlayer, gameObject);
        }
    }
}