using System;

namespace JP.Notek.Udux
{
    // ジェネリクスのエラー回避用
    static class ReduceStoreBaseFunctions
    {
        public static void Start<ModelT, CurrentStateT, NewStateT>(this ReduceStoreBase<ModelT, CurrentStateT, NewStateT> instance)
            where ModelT : IState
            where CurrentStateT : ModelT
            where NewStateT : ModelT
        {
            instance._Dispatcher.RegisterStore(instance);
        }
        public static void SubscribeOnChange<ModelT, CurrentStateT, NewStateT>(this ReduceStoreBase<ModelT, CurrentStateT, NewStateT> instance, IStoreObservable<ModelT> view)
            where ModelT : IState
            where CurrentStateT : ModelT
            where NewStateT : ModelT
        {
            instance.Views = instance.Views.Add(view);
        }
        public static void Update<ModelT, CurrentStateT, NewStateT>(this ReduceStoreBase<ModelT, CurrentStateT, NewStateT> instance)
            where ModelT : IState
            where CurrentStateT : ModelT
            where NewStateT : ModelT
        {
            if (instance.IsStateDistributing)
            {
                if (instance.Views == null || instance.Views.Length <= instance._StateDistributingI)
                {
                    instance.UpdateState();
                    instance.IsStateDistributing = false;
                    instance._StateDistributingI = 0;
                }
                else
                {
                    var view = instance.Views[instance._StateDistributingI++];
                    instance.ViewOnChange(view);
                }
            }
        }
    }
}