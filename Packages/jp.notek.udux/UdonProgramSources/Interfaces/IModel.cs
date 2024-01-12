using UdonSharp;

namespace JP.Notek.Udux
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class IModel<SyncStateT> : UdonSharpBehaviour
    where SyncStateT : ISyncState
    {
        public virtual void ReflectSyncState(SyncStateT state) { }
    }
}