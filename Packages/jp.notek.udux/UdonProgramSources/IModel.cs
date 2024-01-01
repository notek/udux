using UdonSharp;

namespace JP.Notek.Udux
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class IModel<SyncStateT> : UdonSharpBehaviour
    where SyncStateT : SyncStateBase
    {
        public virtual void ReflectSyncState(SyncStateT state) { }
    }
}