using UdonSharp;

namespace JP.Notek.Udux
{
    public class SyncStateBase : UdonSharpBehaviour
    {
        public bool Initialized = false;
        public virtual bool GetDeserialized()
        {
            return GetDeserializedByMode(true);
        }

        protected bool GetDeserializedByMode(bool isLocal)
        {
            if (isLocal)
            {
                return true;
            }
            else
            {
                return Initialized;
            }
        }
        public override void OnDeserialization()
        {
            Initialized = true;
        }
    }
}