// defined from .NET Framework 4.0 and NETFX_CORE

using System;

#if !NETFX_CORE

namespace UniRx
{
    public interface IObservable<T>
    {
        IDisposable Subscribe(IObserver<T> observer);
        object ThrottleFirst(int v);
    }
}

#endif

namespace UniRx
{
    public interface IGroupedObservable<TKey, TElement> : IObservable<TElement>
    {
        TKey Key { get; }
    }
}