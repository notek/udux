using System;
using JP.Notek.Udux;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;

namespace UduxSample
{
    [RequireComponent(typeof(ExampleModelIndividualCurrentState))]
    [RequireComponent(typeof(ExampleModelIndividualNewState))]
    public class ExampleModelIndividualStore : IndividualSyncReduceStoreBase<ExampleModelIndividual, ExampleModelIndividualCurrentState, ExampleModelIndividualNewState, ExampleModelIndividualSyncState>
    {
        protected override void Reset()
        {
            NewState = GetComponent<ExampleModelIndividualNewState>();
            _CurrentState = GetComponent<ExampleModelIndividualCurrentState>();
        }

        public void Start()
        {
            _SyncStateProvider.Subscribe(this);
        }

        public override void SubscribeOnChange(IStoreObservable<ExampleModelIndividual> view)
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

        public ExampleModelIndividualSyncState GetMySyncState()
        {
            return _SyncStateProvider.SyncStates[_SyncStateProvider.MySyncStateIndex];
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
                    GetMySyncState().ReflectLocalState(NewState);
                    break;
                case "OnSubscribeUserChanged":
                    var syncStateIndex = (int)((DataDictionary)value)["syncStateIndex"];
                    NewState.SubscribedStateIndex = syncStateIndex;
                    break;
                case _OnIndividualSyncStateOwnershipGivenAction:
                    var _syncStateIndex = (int)((DataDictionary)value)["syncStateIndex"];
                    if(_CurrentState.SubscribedStateIndex == -1)
                        NewState.SubscribedStateIndex = _syncStateIndex;
                    NewState.ReflectSyncState(GetMySyncState());
                    break;
                case _OnIndividualSyncStateChangedAction:
                    var __syncStateIndex = (int)((DataDictionary)value)["syncStateIndex"];
                    if(_CurrentState.SubscribedStateIndex == __syncStateIndex)
                        NewState.ReflectSyncState(_SyncStateProvider.SyncStates[__syncStateIndex]);
                    break;
                case _OnIndividualSyncStateOwnershipTransferredAction:
                    var ___syncStateIndex = (int)((DataDictionary)value)["syncStateIndex"];
                    if(_CurrentState.SubscribedStateIndex == ___syncStateIndex)
                        NewState.ReflectSyncState(_SyncStateProvider.SyncStates[___syncStateIndex]);
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