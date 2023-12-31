using UdonSharp;

namespace JP.Notek.Udux
{
    public class IStoreObservable<StoreT> : UdonSharpBehaviour
    where StoreT : UdonSharpBehaviour
    {
        public virtual void OnChange(StoreT currentState, StoreT newState) { }
    }
}