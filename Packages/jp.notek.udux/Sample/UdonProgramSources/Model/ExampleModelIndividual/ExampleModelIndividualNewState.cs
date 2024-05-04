namespace UduxSample
{
    public class ExampleModelIndividualNewState : ExampleModelIndividual
    {
        public override void ReflectSyncState(ExampleModelIndividualSyncState state)
        {
            this.Value2 = state.Value2;
            this.Value3 = state.Value3;
            this.OwnerTimeDifference = state.OwnerTimeDifference;
        }
    }
}