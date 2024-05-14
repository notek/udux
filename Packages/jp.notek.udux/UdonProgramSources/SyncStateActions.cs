using VRC.SDK3.Data;

namespace JP.Notek.Udux
{
    public static class SyncStateActions
    {
        public const string ON_CHANGED_ACTION = "SyncState.OnChanged";
        public static void OnChangedDispatch(Dispatcher dispatcher, IReduceStore store, DataToken d)
        {
            dispatcher.DispatchPrivate(store, ON_CHANGED_ACTION, d);
        }
        public const string ON_REQUEST_SUCCESS_ACTION = "SyncState.OnRequestSuccessed";
        public static void OnRequestSuccessedDispatch(Dispatcher dispatcher, IReduceStore store, float requestId)
        {
            var d = new DataDictionary();
            d["request_id"] = requestId;
            dispatcher.DispatchPrivate(store, ON_REQUEST_SUCCESS_ACTION, d);
        }
    }
}