
using System;
using JP.Notek.Udux;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UduxSample
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ExampleIndividualObjectAdapter : IStoreObservable<ExampleModelIndividual>
    {
        [SerializeField] ExampleModelIndividualStore _Store;
        void Start()
        {
            _Store.SubscribeOnChange(this);
        }

        public override void OnChange(ExampleModelIndividual currentState, ExampleModelIndividual newState)
        {
            Debug.Log($"individual.value1={newState.Value1}");
            Debug.Log($"individual.value2={newState.Value2}");
            Debug.Log($"individual.value3={newState.Value3}");
        }
    }
}