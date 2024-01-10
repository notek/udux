using JP.Notek.Udux;
using VRC.SDK3.Data;

namespace UduxSample
{
    public static class ActionCreator
    {
        public static void OnTestActionA(this Dispatcher dispatcher)
        {
            dispatcher.Dispatch("OnTestActionA", new DataToken());
        }
        public static void OnTestActionB(this Dispatcher dispatcher)
        {
            dispatcher.Dispatch("OnTestActionB", new DataToken());
        }
        public static void OnTestActionC(this Dispatcher dispatcher)
        {
            dispatcher.Dispatch("OnTestActionC", new DataToken());
        }
    }
}