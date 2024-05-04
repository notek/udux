using JP.Notek.Udux;
using VRC.SDK3.Data;
using VRC.SDKBase;

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
            dispatcher.Dispatch("OnTestActionC", VRCUrl.Empty);
        }
        public static void OnTestActionD(this Dispatcher dispatcher)
        {
            dispatcher.Dispatch("OnTestActionD", VRCUrl.Empty);
        }
    }
}