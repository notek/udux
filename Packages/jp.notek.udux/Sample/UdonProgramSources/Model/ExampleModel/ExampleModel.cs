using JP.Notek.Udux;

namespace UduxSample
{
    public class ExampleModel : IModel<ExampleModelSyncState>
    {
        public bool Value1 = false;
        public bool Value2 = false;
        public bool Value3 = false;
        public float OwnerTimeDifference = 0;

        public void UpdateState(ExampleModel state)
        {
            this.Value1 = state.Value1;
            this.Value2 = state.Value2;
            this.Value3 = state.Value3;
            this.OwnerTimeDifference = state.OwnerTimeDifference;
        }
    }
}