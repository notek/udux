using VRC.SDK3.Data;

namespace JP.Notek.Udux
{
    public static class SyncStateActions
    {
        public const string ON_CHANGED_ACTION_SUFFIX = ".OnChanged";
        public static void OnChangedDispatch(string action, Dispatcher dispatcher, DataToken d)
        {
            dispatcher.Dispatch(action, d);
        }
        public const string ON_REQUEST_SUCCEED_ACTION_SUFFIX = ".OnRequestSucceed";
        public static void OnRequestSucceedDispatch(string action, Dispatcher dispatcher, float requestId)
        {
            var d = new DataDictionary();
            d["request_id"] = requestId;
            dispatcher.Dispatch(action, d);
        }
    }
}