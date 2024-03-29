using UdonSharp;
using VRC.SDK3.Data;
using VRC.SDKBase;

namespace JP.Notek.Udux
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class IReduceStore : UdonSharpBehaviour
    {
        string[] _QueueAction = { };
        DataToken[] _QueueValue = { };
        string[] _VRCUrlQueueAction = { };
        VRCUrl[] _VRCUrlQueueValue = { };
        protected bool _IsStateDistributing = false;
        protected int _StateDistributingI = 0;

        protected const string _OnOwnershipTransferredAction = "__INTERNAL__.OnOwnerShipTransferred";
        protected const string _OnSyncStateChangedAction = "__INTERNAL__.OnSyncStateChanged";
        protected const string _OnIndividualSyncStateOwnershipGivenAction = "__INTERNAL__.OnIndividualSyncStateOwnerShipGiven";
        protected const string _OnIndividualSyncStateOwnershipTransferredAction = "__INTERNAL__.OnIndividualSyncStateOwnerShipTransferred";
        protected const string _OnIndividualSyncStateChangedAction = "__INTERNAL__.OnIndividualSyncStateChanged";

        public virtual void Update()
        {
            if (_IsStateDistributing)
                return;

            if (_QueueAction.Length != 0)
            {
                Reduce(_QueueAction.Pop(out _QueueAction), _QueueValue.Pop(out _QueueValue));
                _IsStateDistributing = true;
            }
            if (_VRCUrlQueueAction.Length != 0)
            {
                Reduce(_VRCUrlQueueAction.Pop(out _VRCUrlQueueAction), _VRCUrlQueueValue.Pop(out _VRCUrlQueueValue));
                _IsStateDistributing = true;
            }
        }

        public void AddQueue(string action, DataToken value)
        {
            _QueueAction = _QueueAction.Add(action);
            _QueueValue = _QueueValue.Add(value);
        }

        public void AddQueue(string action, VRCUrl value)
        {
            _VRCUrlQueueAction = _VRCUrlQueueAction.Add(action);
            _VRCUrlQueueValue = _VRCUrlQueueValue.Add(value);
        }

        public void OnSyncStateChanged()
        {
            _QueueAction = _QueueAction.Add(_OnSyncStateChangedAction);
            _QueueValue = _QueueValue.Add(new DataToken());
        }

        public void OnSyncStateOwnershipTransferred()
        {
            _QueueAction = _QueueAction.Add(_OnOwnershipTransferredAction);
            _QueueValue = _QueueValue.Add(new DataToken());
        }
        public void OnIndividualSyncStateChanged(int syncStateIndex)
        {
            _QueueAction = _QueueAction.Add(_OnIndividualSyncStateChangedAction);
            var d = new DataDictionary();
            d["syncStateIndex"] = syncStateIndex;
            _QueueValue = _QueueValue.Add(d);
        }
        public void OnIndividualSyncStateOwnershipTransferred(int syncStateIndex)
        {
            _QueueAction = _QueueAction.Add(_OnIndividualSyncStateOwnershipTransferredAction);
            var d = new DataDictionary();
            d["syncStateIndex"] = syncStateIndex;
            _QueueValue = _QueueValue.Add(d);
        }
        public void OnIndividualSyncStateOwnershipGiven(int syncStateIndex)
        {
            _QueueAction = _QueueAction.Add(_OnIndividualSyncStateOwnershipGivenAction);
            var d = new DataDictionary();
            d["syncStateIndex"] = syncStateIndex;
            _QueueValue = _QueueValue.Add(d);
        }

        public virtual void Reduce(string action, DataToken value) { }
        public virtual void Reduce(string action, VRCUrl value) { }
    }
}