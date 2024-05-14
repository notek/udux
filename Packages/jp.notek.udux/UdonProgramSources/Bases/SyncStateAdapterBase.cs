using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;

namespace JP.Notek.Udux
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SyncStateAdapterBase<SyncModelT, SyncCurrentStateT, SyncNewStateT, StoreT> : IStoreObservable<SyncModelT>
    where SyncModelT : IState
    where SyncCurrentStateT : SyncModelT
    where SyncNewStateT : SyncModelT
    where StoreT : ReduceStoreBase<SyncModelT, SyncCurrentStateT, SyncNewStateT>
    {
        [SerializeField] public Dispatcher _Dispatcher;
        [SerializeField] public StoreT _Store;
        [SerializeField] public int _LatencySyncFrameDuration = 1000;


        [UdonSynced] public float _SyncTime;
        [UdonSynced] public float _SyncTimeId;
        public float OwnerTimeDifference = float.PositiveInfinity;
        public float _SyncTimeIdLocal;
        public int _LatencySyncedFrame = 0;

        void Start()
        {
            SyncStateAdapterBaseFunctions.Start(this);
        }
        public virtual void StoreSubscribeOnChange() { }
        public virtual float GetRequestId() { return -1; }
        public virtual void ReflectSyncState() { }
        public virtual void DispatchOnRequestSuccess(float requestId) { }
        public virtual void DispatchOnChanged() { }


        // エラー回避不可のため継承先で直接実装
        public override void OnChange(SyncModelT currentState, SyncModelT newState)
        {
            SyncStateAdapterBaseFunctions.OnChange(this);
        }

        public override void OnDeserialization()
        {
            SyncStateAdapterBaseFunctions.OnDeserialization(this);
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            SyncStateAdapterBaseFunctions.OnPlayerJoined(this);
        }

        new public float RequestSerialization()
        {
            return SyncStateAdapterBaseFunctions.RequestSerialization(this);
        }
        public void RequestSerializationBase()
        {
            base.RequestSerialization();
        }

        public bool GetIsOwner()
        {
            return Networking.IsOwner(Networking.LocalPlayer, gameObject);
        }

        public void TakeOwnership()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
    }
}