
using System;
using JP.Notek.Udux;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UduxSample
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ExampleObjectAdapter : IStoreObservable<ExampleModel>
    {
        [SerializeField] ExampleModelStore _Store;
        void Start()
        {
            _Store.SubscribeOnChange(this);
        }

        public override void OnChange(ExampleModel currentState, ExampleModel newState)
        {
            Debug.Log($"value1={newState.Value1} value2={newState.Value2} value3={newState.Value3}");
        }
    }
}