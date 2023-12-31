using UdonSharp;
using JP.Notek.Udux;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;

namespace JP.Notek.Udux
{
    public class DispatcherBase : UdonSharpBehaviour
    {
        [SerializeField] IReduceStore[] Stores;

        public void Dispatch(string action, DataToken value =  new DataToken())
        {
            foreach (var store in Stores)
            {
                store.AddQueue(action, value);
            }
        }
    }
}