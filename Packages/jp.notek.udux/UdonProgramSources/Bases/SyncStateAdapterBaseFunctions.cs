using UnityEngine;

namespace JP.Notek.Udux
{
    // ジェネリクスのエラー回避用
    public static class SyncStateAdapterBaseFunctions
    {
        public static void Start<SyncModelT, SyncCurrentStateT, SyncNewStateT, StoreT>(this SyncStateAdapterBase<SyncModelT, SyncCurrentStateT, SyncNewStateT, StoreT> instance)
            where SyncModelT : IState
            where SyncCurrentStateT : SyncModelT
            where SyncNewStateT : SyncModelT
            where StoreT : ReduceStoreBase<SyncModelT, SyncCurrentStateT, SyncNewStateT>
        {
            instance.StoreSubscribeOnChange();
            if (instance.GetIsOwner())
            {
                instance.SetOwnerTimeDifference();
                instance.DispatchOnChanged();
            }
        }

        public static void OnChange<SyncModelT, SyncCurrentStateT, SyncNewStateT, StoreT>(this SyncStateAdapterBase<SyncModelT, SyncCurrentStateT, SyncNewStateT, StoreT> instance)
            where SyncModelT : IState
            where SyncCurrentStateT : SyncModelT
            where SyncNewStateT : SyncModelT
            where StoreT : ReduceStoreBase<SyncModelT, SyncCurrentStateT, SyncNewStateT>
        {
            var requestId = instance.GetRequestId();
            if(requestId == -1)
                return;
            if (!instance.GetIsOwner())
                instance.TakeOwnership();
            instance.ReflectSyncState();
            instance.RequestSerialization();
            instance.DispatchOnRequestSuccess(requestId);
            instance.DispatchOnChanged();
        }

        public static void OnDeserialization<SyncModelT, SyncCurrentStateT, SyncNewStateT, StoreT>(this SyncStateAdapterBase<SyncModelT, SyncCurrentStateT, SyncNewStateT, StoreT> instance)
            where SyncModelT : IState
            where SyncCurrentStateT : SyncModelT
            where SyncNewStateT : SyncModelT
            where StoreT : ReduceStoreBase<SyncModelT, SyncCurrentStateT, SyncNewStateT>
        {
            if (instance._SyncTimeIdLocal != instance._SyncTimeId)
            {
                instance.GetOwnerTimeDifference();
            }
            instance.DispatchOnChanged();
        }

        public static void OnPlayerJoined<SyncModelT, SyncCurrentStateT, SyncNewStateT, StoreT>(this SyncStateAdapterBase<SyncModelT, SyncCurrentStateT, SyncNewStateT, StoreT> instance)
            where SyncModelT : IState
            where SyncCurrentStateT : SyncModelT
            where SyncNewStateT : SyncModelT
            where StoreT : ReduceStoreBase<SyncModelT, SyncCurrentStateT, SyncNewStateT>
        {
            if (instance.GetIsOwner())
            {
                SyncStateAdapterBaseFunctions.RequestSerialization(instance);
            }
        }

        public static float RequestSerialization<SyncModelT, SyncCurrentStateT, SyncNewStateT, StoreT>(this SyncStateAdapterBase<SyncModelT, SyncCurrentStateT, SyncNewStateT, StoreT> instance)
            where SyncModelT : IState
            where SyncCurrentStateT : SyncModelT
            where SyncNewStateT : SyncModelT
            where StoreT : ReduceStoreBase<SyncModelT, SyncCurrentStateT, SyncNewStateT>
        {
            if (Time.frameCount - instance._LatencySyncedFrame > instance._LatencySyncFrameDuration)
                instance.SetOwnerTimeDifference();
            instance.RequestSerializationBase();
            return instance.OwnerTimeDifference;
        }

        public static float SetOwnerTimeDifference<SyncModelT, SyncCurrentStateT, SyncNewStateT, StoreT>(this SyncStateAdapterBase<SyncModelT, SyncCurrentStateT, SyncNewStateT, StoreT> instance)
            where SyncModelT : IState
            where SyncCurrentStateT : SyncModelT
            where SyncNewStateT : SyncModelT
            where StoreT : ReduceStoreBase<SyncModelT, SyncCurrentStateT, SyncNewStateT>
        {
            instance._SyncTimeId = Random.value;
            instance._SyncTimeIdLocal = instance._SyncTimeId;
            instance._SyncTime = Time.time;
            instance._LatencySyncedFrame = Time.frameCount;
            instance.OwnerTimeDifference = 0;
            return instance.OwnerTimeDifference;
        }

        public static void GetOwnerTimeDifference<SyncModelT, SyncCurrentStateT, SyncNewStateT, StoreT>(this SyncStateAdapterBase<SyncModelT, SyncCurrentStateT, SyncNewStateT, StoreT> instance)
            where SyncModelT : IState
            where SyncCurrentStateT : SyncModelT
            where SyncNewStateT : SyncModelT
            where StoreT : ReduceStoreBase<SyncModelT, SyncCurrentStateT, SyncNewStateT>
        {
            instance._SyncTimeIdLocal = instance._SyncTimeId;
            instance.OwnerTimeDifference = Time.time - instance._SyncTime;
        }
    }
}