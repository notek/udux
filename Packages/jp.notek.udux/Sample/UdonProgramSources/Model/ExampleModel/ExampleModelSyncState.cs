using UdonSharp;
using JP.Notek.Udux;

namespace UduxSample
{
    public class ExampleModelSyncState : SyncStateBase
    {
        [UdonSynced] public bool Value2 = false;
        [UdonSynced] public bool Value3 = false;

        public void ReflectLocalState(ExampleModel state){
            this.Value2 = state.Value2;
            this.Value3 = state.Value3;
            RequestSerialization();
        }
    }
}