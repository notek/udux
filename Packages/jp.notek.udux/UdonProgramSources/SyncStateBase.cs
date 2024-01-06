using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace JP.Notek.Udux
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SyncStateBase : UdonSharpBehaviour
    {
        public bool Initialized = false;
        public VRCPlayerApi Owner { get { return GetOwner(); } }
        public bool IsOwner { get { return GetIsOwner(); } }
        public float OwnerTimeDifference = float.PositiveInfinity;

        [UdonSynced] float _SyncTime;
        [UdonSynced] float _SyncTimeId;
        [SerializeField] int _LatencySyncFrameDuration = 1000;
        float _SyncTimeIdLocal;
        IReduceStore _StoreSubscribed = null;
        int _LatencySyncedFrame = 0;
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

        new public float RequestSerialization()
        {
            if (Time.frameCount - _LatencySyncedFrame > _LatencySyncFrameDuration)
                SetOwnerTimeDifference();
            base.RequestSerialization();
            return OwnerTimeDifference;
        }

        public float SetOwnerTimeDifference()
        {
            _SyncTimeId = Random.value;
            _SyncTimeIdLocal = _SyncTimeId;
            _SyncTime = Time.time;
            _LatencySyncedFrame = Time.frameCount;
            OwnerTimeDifference = 0;
            return OwnerTimeDifference;
        }

        private void GetOwnerTimeDifference()
        {
            _SyncTimeIdLocal = _SyncTimeId;
            OwnerTimeDifference = Time.time - _SyncTime;
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