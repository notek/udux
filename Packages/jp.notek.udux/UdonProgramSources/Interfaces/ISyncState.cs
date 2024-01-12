
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace JP.Notek.Udux
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class ISyncState : UdonSharpBehaviour
    {
        public float OwnerTimeDifference = float.PositiveInfinity;

        [UdonSynced] float _SyncTime;
        [UdonSynced] protected float _SyncTimeId;
        [SerializeField] protected int _LatencySyncFrameDuration = 1000;
        protected float _SyncTimeIdLocal;
        protected int _LatencySyncedFrame = 0;

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

        protected void GetOwnerTimeDifference()
        {
            _SyncTimeIdLocal = _SyncTimeId;
            OwnerTimeDifference = Time.time - _SyncTime;
        }

        protected VRCPlayerApi GetOwner()
        {
            return Networking.GetOwner(gameObject);
        }

        protected bool GetIsOwner()
        {
            return Networking.IsOwner(Networking.LocalPlayer, gameObject);
        }
    }
}