using JP.Notek.Udux;
using UdonSharp;
using VRC.SDK3.Data;

namespace UduxSample
{
    public class ExampleModel : IState
    {
        public bool Value1 = false;
        public bool Value2 = false;
        public bool Value3 = false;
        public float OwnerTimeDifference = 0;
        public float SyncRequestId = -1;
        public bool ReqValue2 = false;
        public bool ReqValue3 = false;

        public void UpdateState(ExampleModel state)
        {
            Value1 = state.Value1;
            Value2 = state.Value2;
            Value3 = state.Value3;
            OwnerTimeDifference = state.OwnerTimeDifference;
            SyncRequestId = state.SyncRequestId;
            ReqValue2 = state.ReqValue2;
            ReqValue3 = state.ReqValue3;
        }
    }
}