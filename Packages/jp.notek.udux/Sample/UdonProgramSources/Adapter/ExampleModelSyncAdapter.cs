
using JP.Notek.Udux;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

namespace UduxSample
{
    public class ExampleModelSyncAdapter : SyncStateAdapterBase<ExampleModel, ExampleModelCurrentState, ExampleModelNewState, ExampleModelStore>
    {
        [UdonSynced] public bool Value2 = false;
        [UdonSynced] public bool Value3 = false;

        public override void StoreSubscribeOnChange()
        {
            _Store.SubscribeOnChange(this);
        }

        public override void DispatchOnRequestSuccess(float requestId)
        {
            SyncStateActions.OnRequestSuccessedDispatch(_Dispatcher, _Store, requestId);
        }

        public override float GetRequestId() { return _Store.NewState.SyncRequestId; }

        public override void ReflectSyncState()
        {
            Value2 = _Store.NewState.ReqValue2;
            Value3 = _Store.NewState.ReqValue3;
        }

        public override void DispatchOnChanged()
        {
            var d = new DataDictionary();
            d["Value2"] = Value2;
            d["Value3"] = Value3;
            d["OwnerTimeDifference"] = OwnerTimeDifference;
            SyncStateActions.OnChangedDispatch(_Dispatcher, _Store, d);
        }
    }
}