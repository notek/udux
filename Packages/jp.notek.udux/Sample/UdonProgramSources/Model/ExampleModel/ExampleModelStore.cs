using System;
using JP.Notek.Udux;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;

namespace UduxSample
{
    [RequireComponent(typeof(ExampleModelCurrentState))]
    [RequireComponent(typeof(ExampleModelNewState))]
    public class ExampleModelStore : ReduceStoreBase<ExampleModel, ExampleModelCurrentState, ExampleModelNewState, ExampleModelSyncState>
    {
        protected override void Reset()
        {
            NewState = GetComponent<ExampleModelNewState>();
            _CurrentState = GetComponent<ExampleModelCurrentState>();
        }

        public void Start()
        {
            _SyncState.Subscribe(this);
        }

        public override void SubscribeOnChange(IStoreObservable<ExampleModel> view)
        {
            Views = Views.Add(view);
        }

        public override void Update()
        {
            base.Update();

            if (_IsStateDistributing)
            {
                if (Views.Length <= _StateDistributingI)
                {
                    _CurrentState.UpdateState(NewState);
                    _IsStateDistributing = false;
                    _StateDistributingI = 0;
                }
                else
                {
                    Views[_StateDistributingI++].OnChange(_CurrentState, NewState);
                }
            }
        }

        public override void Reduce(string action, DataToken value)
        {
            switch (action)
            {
                case "OnTestActionA":
                    NewState.Value1 = true;
                    break;
                case "OnTestActionB":
                    NewState.Value2 = false;
                    NewState.Value3 = false;
                    _SyncState.ReflectLocalState(NewState);
                    break;
                case "OnOwnershipRequested":
                    _SyncState.TakeOwnership();
                    _SyncState.SetOwnerTimeDifference();
                    NewState.OwnerTimeDifference = _SyncState.RequestSerialization();
                    break;
                case _OnOwnershipTransferredAction:
                    _SyncState.SetOwnerTimeDifference();
                    NewState.OwnerTimeDifference = _SyncState.RequestSerialization();
                    break;
                case _OnSyncStateChangedAction:
                    NewState.ReflectSyncState(_SyncState);
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
                    break;
                default:
                    return;
            }
        }
    }
}