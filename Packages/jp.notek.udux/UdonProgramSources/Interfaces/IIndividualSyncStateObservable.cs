
using UdonSharp;

namespace JP.Notek.Udux
{
public class IIndividualSyncStateObservable : UdonSharpBehaviour
{
        public virtual void OnIndividualSyncStateChanged(int index) { }
        public virtual void OnIndividualSyncStateOwnershipTransferred(int index) { }
        public virtual void OnIndividualSyncStateOwnershipGiven(int index) { }
}
}