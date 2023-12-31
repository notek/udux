using UdonSharp;
using VRC.SDK3.Data;
using VRC.SDKBase;

namespace JP.Notek.Udux
{
    public class IReduceStore : UdonSharpBehaviour
    {
        string[] _QueueAction = { };
        DataToken[] _QueueValue = { };
        string[] _VRCUrlQueueAction = { };
        VRCUrl[] _VRCUrlQueueValue = { };
        void Update()
        {
            if(_QueueAction.Length != 0)
                Reduce(_QueueAction.Pop(out _QueueAction), _QueueValue.Pop(out _QueueValue));
            if(_VRCUrlQueueAction.Length != 0)
                Reduce(_VRCUrlQueueAction.Pop(out _VRCUrlQueueAction), _VRCUrlQueueValue.Pop(out _VRCUrlQueueValue));
        }

        public void AddQueue(string action, DataToken value) {
            _QueueAction = _QueueAction.Add(action);
            _QueueValue = _QueueValue.Add(value);
        }

        public void AddQueue(string action, VRCUrl value) {
            _VRCUrlQueueAction = _VRCUrlQueueAction.Add(action);
            _VRCUrlQueueValue = _VRCUrlQueueValue.Add(value);
        }

        public virtual void Reduce(string action, DataToken value) { }
        public virtual void Reduce(string action, VRCUrl value) { }
    }
}