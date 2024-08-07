using System;
using UnityEngine;

namespace JP.Notek.Udux
{
    public class ReduceStoreBase<ModelT, CurrentStateT, NewStateT> : IReduceStore
    where ModelT : IState
    where CurrentStateT : ModelT
    where NewStateT : ModelT
    {
        [SerializeField] public Dispatcher _Dispatcher;
        [SerializeField] public NewStateT NewState;
        [SerializeField] public CurrentStateT _CurrentState;
        public IStoreObservable<ModelT>[] Views = { };
        public int _StateDistributingI = 0;


        public virtual void Start()
        {
            ReduceStoreBaseFunctions.Start(this);
        }

        protected virtual void Reset()
        {
            // NewState = GetComponent<NewStateT>();
            // _CurrentState = GetComponent<CurrentStateT>();
        }

        public virtual void ViewOnChange(IStoreObservable<ModelT> view) { }
        public virtual void UpdateState() { }

        public void SubscribeOnChange(IStoreObservable<ModelT> view)
        {
            ReduceStoreBaseFunctions.SubscribeOnChange(this, view);
        }

        void Update()
        {
            ReduceStoreBaseFunctions.Update(this);
        }
    }
}