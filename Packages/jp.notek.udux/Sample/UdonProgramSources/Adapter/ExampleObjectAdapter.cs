
using System;
using JP.Notek.Udux;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UduxSample
{
    public class ExampleObjectAdapter : IStoreObservable<ExampleModel>
    {
        [SerializeField] ExampleModelStore _Store;
        void Start()
        {
            _Store.SubscribeOnChange(this);
        }

        public override void OnChange(ExampleModel currentState, ExampleModel newState)
        {
        }
    }
}