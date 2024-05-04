using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;

namespace JP.Notek.Udux
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class IReduceStore : UdonSharpBehaviour
    {
        bool[] _QueueHasValue = new bool[32];
        string[] _QueueAction = new string[32];
        DataToken[] _QueueValue = new DataToken[32];
        bool[] _VRCUrlQueueHasValue = new bool[32];
        string[] _VRCUrlQueueAction = new string[32];
        VRCUrl[] _VRCUrlQueueValue = new VRCUrl[32];
        int _QueueLength = 32;
        int _QueueReadHead = 0;
        int _QueueWriteHead = 0;
        protected bool _IsStateDistributing = false;
        protected int _StateDistributingI = 0;

        // 値変更の配布をスキップ可能にする
        protected bool _OmitDistribution = false;

        // _OmitDistribution == trueのとき1フレームで処理するイベントの数。
        //Reduceの処理量が多い場合この数値が大きいとfps低下を招く。この数値が小さいと処理の遅延が大きくなる。
        protected int _ProcessingUnit = 32;

        protected const string _OnOwnershipTransferredAction = "__INTERNAL__.OnOwnerShipTransferred";
        protected const string _OnSyncStateChangedAction = "__INTERNAL__.OnSyncStateChanged";
        protected const string _OnIndividualSyncStateOwnershipGivenAction = "__INTERNAL__.OnIndividualSyncStateOwnerShipGiven";
        protected const string _OnIndividualSyncStateOwnershipTransferredAction = "__INTERNAL__.OnIndividualSyncStateOwnerShipTransferred";
        protected const string _OnIndividualSyncStateChangedAction = "__INTERNAL__.OnIndividualSyncStateChanged";

        public virtual void Update()
        {
            if (_IsStateDistributing)
                return;

            for (int i = 0; i < (_OmitDistribution ? _ProcessingUnit : 1); i++)
            {
                var queueHasValue = _QueueHasValue[_QueueReadHead];
                var vRCUrlQueueHasValue = _VRCUrlQueueHasValue[_QueueReadHead];
                if (!queueHasValue && !vRCUrlQueueHasValue)
                    break;

                if (queueHasValue)
                {
                    Reduce(_QueueAction[_QueueReadHead], _QueueValue[_QueueReadHead]);
                    _QueueHasValue[_QueueReadHead] = false;
                }
                else if (vRCUrlQueueHasValue)
                {
                    Reduce(_VRCUrlQueueAction[_QueueReadHead], _VRCUrlQueueValue[_QueueReadHead]);
                    _VRCUrlQueueHasValue[_QueueReadHead] = false;
                }
                var nextHead = (_QueueReadHead + 1) % _QueueLength;
                _IsStateDistributing = !_OmitDistribution || (!_QueueHasValue[nextHead] && !_VRCUrlQueueHasValue[nextHead]);
                _QueueReadHead = nextHead;
            }
        }

        public void AddQueue(string action, DataToken value)
        {
            if (_QueueHasValue[_QueueWriteHead] || _VRCUrlQueueHasValue[_QueueWriteHead])
            {
                RescaleQueue();
            }
            _QueueHasValue[_QueueWriteHead] = true;
            _QueueAction[_QueueWriteHead] = action;
            _QueueValue[_QueueWriteHead] = value;
            _QueueWriteHead = (_QueueWriteHead + 1) % _QueueLength;
        }

        public void AddQueue(string action, VRCUrl value)
        {
            if (_QueueHasValue[_QueueWriteHead] || _VRCUrlQueueHasValue[_QueueWriteHead])
            {
                RescaleQueue();
            }
            _VRCUrlQueueHasValue[_QueueWriteHead] = true;
            _VRCUrlQueueAction[_QueueWriteHead] = action;
            _VRCUrlQueueValue[_QueueWriteHead] = value;
            _QueueWriteHead = (_QueueWriteHead + 1) % _QueueLength;
        }

        public void OnSyncStateChanged()
        {
            AddQueue(_OnSyncStateChangedAction, new DataToken());
        }

        public void OnSyncStateOwnershipTransferred()
        {
            AddQueue(_OnOwnershipTransferredAction, new DataToken());
        }
        public void OnIndividualSyncStateChanged(int syncStateIndex)
        {
            var d = new DataDictionary();
            d["syncStateIndex"] = syncStateIndex;
            AddQueue(_OnIndividualSyncStateChangedAction, d);
        }
        public void OnIndividualSyncStateOwnershipTransferred(int syncStateIndex)
        {
            var d = new DataDictionary();
            d["syncStateIndex"] = syncStateIndex;
            _QueueValue = _QueueValue.Add(d);
            AddQueue(_OnIndividualSyncStateOwnershipTransferredAction, d);
        }
        public void OnIndividualSyncStateOwnershipGiven(int syncStateIndex)
        {
            var d = new DataDictionary();
            d["syncStateIndex"] = syncStateIndex;
            AddQueue(_OnIndividualSyncStateOwnershipGivenAction, d);
        }

        public virtual void Reduce(string action, DataToken value) { }
        public virtual void Reduce(string action, VRCUrl value) { }
        protected void InitQueue(int queueLength)
        {
            _QueueHasValue = new bool[queueLength];
            _QueueAction = new string[queueLength];
            _QueueValue = new DataToken[queueLength];
            _VRCUrlQueueHasValue = new bool[queueLength];
            _VRCUrlQueueAction = new string[queueLength];
            _VRCUrlQueueValue = new VRCUrl[queueLength];

            _QueueLength = queueLength;
        }

        void RescaleQueue()
        {
            int queueLength = _QueueLength * 2;

            bool[] queueHasValue = new bool[queueLength];
            string[] queueAction = new string[queueLength];
            DataToken[] queueValue = new DataToken[queueLength];
            bool[] vRCUrlQueueHasValue = new bool[queueLength];
            string[] vRCUrlQueueAction = new string[queueLength];
            VRCUrl[] vRCUrlQueueValue = new VRCUrl[queueLength];

            Array.Copy(_QueueHasValue, 0, queueHasValue, 0, _QueueWriteHead);
            Array.Copy(_QueueHasValue, _QueueWriteHead, queueHasValue, _QueueWriteHead + _QueueLength, _QueueLength - _QueueWriteHead);
            Array.Copy(_QueueAction, 0, queueAction, 0, _QueueWriteHead);
            Array.Copy(_QueueAction, _QueueWriteHead, queueAction, _QueueWriteHead + _QueueLength, _QueueLength - _QueueWriteHead);
            Array.Copy(_QueueValue, 0, queueValue, 0, _QueueWriteHead);
            Array.Copy(_QueueValue, _QueueWriteHead, queueValue, _QueueWriteHead + _QueueLength, _QueueLength - _QueueWriteHead);
            Array.Copy(_VRCUrlQueueHasValue, 0, vRCUrlQueueHasValue, 0, _QueueWriteHead);
            Array.Copy(_VRCUrlQueueHasValue, _QueueWriteHead, vRCUrlQueueHasValue, _QueueWriteHead + _QueueLength, _QueueLength - _QueueWriteHead);
            Array.Copy(_VRCUrlQueueAction, 0, vRCUrlQueueAction, 0, _QueueWriteHead);
            Array.Copy(_VRCUrlQueueAction, _QueueWriteHead, vRCUrlQueueAction, _QueueWriteHead + _QueueLength, _QueueLength - _QueueWriteHead);
            Array.Copy(_VRCUrlQueueValue, 0, vRCUrlQueueValue, 0, _QueueWriteHead);
            Array.Copy(_VRCUrlQueueValue, _QueueWriteHead, vRCUrlQueueValue, _QueueWriteHead + _QueueLength, _QueueLength - _QueueWriteHead);

            _QueueHasValue = queueHasValue;
            _QueueAction = queueAction;
            _QueueValue = queueValue;
            _VRCUrlQueueHasValue = vRCUrlQueueHasValue;
            _VRCUrlQueueAction = vRCUrlQueueAction;
            _VRCUrlQueueValue = vRCUrlQueueValue;

            if (_QueueReadHead >= _QueueWriteHead)
                _QueueReadHead += _QueueLength;
            _QueueLength = queueLength;
        }
    }
}