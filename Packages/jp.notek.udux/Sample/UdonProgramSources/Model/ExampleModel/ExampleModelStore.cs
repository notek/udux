using System;
using JP.Notek.Udux;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;

namespace UduxSample
{
    [RequireComponent(typeof(ExampleModelCurrentState))]
    [RequireComponent(typeof(ExampleModelNewState))]
    public class ExampleModelStore : ReduceStoreBase<ExampleModel, ExampleModelCurrentState, ExampleModelNewState>
    {
        public override void UpdateState()
        {
            _CurrentState.UpdateState(NewState);
        }

        public override void ViewOnChange(IStoreObservable<ExampleModel> view)
        {
            view.OnChange(_CurrentState, NewState);
        }

        public override void Reduce(string action, DataToken value)
        {
            switch (action)
            {
                case "OnTestActionA":
                    NewState.Value1 = true;
                    break;
                case "OnTestActionB":
                    NewState.SyncRequestId = UnityEngine.Random.value;
                    NewState.ReqValue2 = true;
                    break;
                case SyncStateActions.ON_REQUEST_SUCCESS_ACTION:

                    var requestId = (float)value.DataDictionary["request_id"];
                    if(requestId == NewState.SyncRequestId)
                        NewState.SyncRequestId = -1;
                    break;
                case SyncStateActions.ON_CHANGED_ACTION:
                    NewState.Value2 = (bool)value.DataDictionary["Value2"];
                    NewState.Value3 = (bool)value.DataDictionary["Value3"];
                    NewState.OwnerTimeDifference = (float)value.DataDictionary["OwnerTimeDifference"];
                    break;
                default:
                    return;
            }
        }

        public override void Reduce(string action, VRCUrl value)
        {
            switch (action)
            {
                case "OnTestActionC":
                    NewState.SyncRequestId = UnityEngine.Random.value;
                    NewState.ReqValue3 = true;
                    break;
                case "OnTestActionD":
                    NewState.SyncRequestId = UnityEngine.Random.value;
                    NewState.Value1 = false;
                    NewState.ReqValue2 = false;
                    NewState.ReqValue3 = false;
                    break;
                default:
                    return;
            }
        }
    }
}