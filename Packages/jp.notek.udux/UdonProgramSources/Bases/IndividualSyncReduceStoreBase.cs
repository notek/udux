using UdonSharp;
using UnityEngine;

namespace JP.Notek.Udux
{
    public class IndividualSyncReduceStoreBase<ModelT, CurrentStateT, NewStateT, SyncStateT> : IReduceStore
    where SyncStateT : IndividualSyncStateBase
    where ModelT : IModel<SyncStateT>
    where CurrentStateT : ModelT
    where NewStateT : ModelT
    {
        public NewStateT NewState;
        [SerializeField] protected CurrentStateT _CurrentState;
        [SerializeField] protected IndividualSyncStateProvider<SyncStateT> _SyncStateProvider;
        protected IStoreObservable<ModelT>[] Views = { };

        protected virtual void Reset()
        {
            // NewState = GetComponent<NewStateT>();
            // _CurrentState = GetComponent<CurrentStateT>();
            // _SyncState = GetComponent<SyncStateT>();
        }

        public virtual void SubscribeOnChange(IStoreObservable<ModelT> view)
        {
            // Views = Views.Add(view);
        }
    }
}