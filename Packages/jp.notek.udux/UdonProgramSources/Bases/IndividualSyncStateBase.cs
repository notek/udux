using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace JP.Notek.Udux
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class IndividualSyncStateBase : ISyncState
    {
        public int index;
        [UdonSynced] public int UserId = -1;
        IIndividualSyncStateObservable _Provider = null;
        public virtual void Init() { }

        void Start()
        {
            if (GetIsOwner() && IsUserMatchOwner())
            {
                SetOwnerTimeDifference();
            }
        }

        public void SetUser(VRCPlayerApi player)
        {
            UserId = player.playerId;
            base.RequestSerialization();
            Networking.SetOwner(player, gameObject);
        }

        public void UnsetUser()
        {
            UserId = -1;
            base.RequestSerialization();
        }

        public bool IsUserMatchOwner()
        {
            return UserId == GetOwner().playerId;
        }

        public void Subscribe(IIndividualSyncStateObservable provider)
        {
            _Provider = provider;
        }

        public override void OnDeserialization()
        {
            if (_SyncTimeIdLocal != _SyncTimeId)
            {
                GetOwnerTimeDifference();
            }
            _Provider.OnIndividualSyncStateChanged(index);
        }

        public override void OnOwnershipTransferred(VRCPlayerApi player)
        {
            if (GetIsOwner() && IsUserMatchOwner())
            {
                Init();
                SetOwnerTimeDifference();
                RequestSerialization();
                _Provider.OnIndividualSyncStateOwnershipGiven(index);
            }
            else
            {
                _Provider.OnIndividualSyncStateOwnershipTransferred(index);
            }
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (GetIsOwner() && IsUserMatchOwner())
            {
                RequestSerialization();
            }
        }
    }
}