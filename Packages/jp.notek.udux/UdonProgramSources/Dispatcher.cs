using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;

namespace JP.Notek.Udux
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Dispatcher : UdonSharpBehaviour
    {
        [SerializeField] IReduceStore[] _Stores;

        public void Dispatch(string action, DataToken value =  new DataToken())
        {
            foreach (var store in _Stores)
            {
                store.AddQueue(action, value);
            }
        }

        public void Dispatch(string action, VRCUrl url)
        {
            foreach (var store in _Stores)
            {
                store.AddQueue(action, url);
            }
        }
    }
}