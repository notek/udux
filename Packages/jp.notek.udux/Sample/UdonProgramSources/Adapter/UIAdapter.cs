
using JP.Notek.Udux;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UduxSample
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class UIAdapter : UdonSharpBehaviour
    {
        [SerializeField] Dispatcher _Dispatcher;
        void Start()
        {

        }

        public void OnButtonEvent1x1Pressed()
        {
            _Dispatcher.OnTestActionA();
        }
        public void OnButtonEvent2x1Pressed()
        {
            _Dispatcher.OnTestActionB();
        }
        public void OnButtonEvent3x1Pressed()
        {
            _Dispatcher.OnTestActionC();
        }
    }
}
