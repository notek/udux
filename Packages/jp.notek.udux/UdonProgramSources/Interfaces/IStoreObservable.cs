using UdonSharp;

namespace JP.Notek.Udux
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class IStoreObservable<ModelT> : UdonSharpBehaviour
    where ModelT : UdonSharpBehaviour
    {
        public virtual void OnChange(ModelT currentState, ModelT newState) { }
    }
}