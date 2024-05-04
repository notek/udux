using UdonSharp;
using JP.Notek.Udux;

namespace UduxSample
{
    public class ExampleModelIndividualSyncState : IndividualSyncStateBase
    {
        [UdonSynced] public bool Value2 = false;
        [UdonSynced] public bool Value3 = false;

        public void ReflectLocalState(ExampleModelIndividual state)
        {
            this.Value2 = state.Value2;
            this.Value3 = state.Value3;
            RequestSerialization();
        }
        public override void Init()
        {
            Value2 = false;
            Value3 = false;
        }
    }
}