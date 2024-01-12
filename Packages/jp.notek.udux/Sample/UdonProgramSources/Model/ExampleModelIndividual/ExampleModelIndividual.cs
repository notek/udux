using JP.Notek.Udux;

namespace UduxSample
{
    public class ExampleModelIndividual : IModel<ExampleModelIndividualSyncState>
    {
        public bool Value1 = false;
        public bool Value2 = false;
        public bool Value3 = false;
        public int SubscribedStateIndex = -1;
        public float OwnerTimeDifference = 0;

        public void UpdateState(ExampleModelIndividual state)
        {
            this.Value1 = state.Value1;
            this.Value2 = state.Value2;
            this.Value3 = state.Value3;
            this.SubscribedStateIndex = state.SubscribedStateIndex;
            this.OwnerTimeDifference = state.OwnerTimeDifference;
        }
    }
}