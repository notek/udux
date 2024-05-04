
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
            Debug.Log($"value1={newState.Value1}");
            Debug.Log($"value2={newState.Value2}");
            Debug.Log($"value3={newState.Value3}");
        }
    }
}