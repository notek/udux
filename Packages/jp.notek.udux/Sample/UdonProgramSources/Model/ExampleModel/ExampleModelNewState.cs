namespace UduxSample
{
    public class ExampleModelNewState : ExampleModel
    {
        public override void ReflectSyncState(ExampleModelSyncState state)
        {
            this.Value2 = state.Value2;
            this.Value3 = state.Value3;
            this.OwnerTimeDifference = state.OwnerTimeDifference;
        }
    }
}