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
        public float Latency = float.PositiveInfinity;

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
                SetLatency();
                if (_StoreSubscribed != null)
                    _StoreSubscribed.OnSyncStateDeserialization();
            }
        }

        public float TakeOwnership()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            SetLatency();
            RequestSerialization();
            return Latency;
        }

        public void SubscribeOnDeserialization(IReduceStore store)
        {
            _StoreSubscribed = store;
        }

        public override void OnDeserialization()
        {
            Initialized = true;
            if (_SyncTimeIdLocal != _SyncTimeId)
            {
                GetLatency();
            }
            if (_StoreSubscribed != null)
                _StoreSubscribed.OnSyncStateDeserialization();
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (IsOwner)
            {
                RequestSerialization();
            }
        }

        new public void RequestSerialization()
        {
            if (Time.frameCount - _LatencySyncedFrame > _LatencySyncFrameDuration)
                SetLatency();
            base.RequestSerialization();
        }

        private void SetLatency()
        {
            _SyncTimeId = Random.value;
            _SyncTimeIdLocal = _SyncTimeId;
            _SyncTime = Time.time;
            _LatencySyncedFrame = Time.frameCount;
            Latency = 0;
        }

        private void GetLatency()
        {
            _SyncTimeIdLocal = _SyncTimeId;
            Latency = Time.time - _SyncTime;
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