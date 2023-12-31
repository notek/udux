using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;

namespace JP.Notek.Udux
{
    public class ReduceStoreBase<ModelT, CurrentStateT, NewStateT> : IReduceStore
    where ModelT : UdonSharpBehaviour
    where CurrentStateT : ModelT
    where NewStateT : ModelT
    {
        public NewStateT NewState;
        [SerializeField] protected CurrentStateT _CurrentState;
        protected IStoreObservable<ModelT>[] Views = { };

        protected virtual void Reset()
        {
            // NewState = GetComponent<NewStateT>();
            // _CurrentState = GetComponent<CurrentStateT>();
        }

        public virtual void SubscribeOnChange(IStoreObservable<ModelT> view) {
            // Views = Views.Add(view);
        }

        protected void TakeStateOwnership(UdonSharpBehaviour newState)
        {
            Networking.SetOwner(Networking.LocalPlayer, newState.gameObject);
        }
    }
}