using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    private readonly Func<T> preloadFunc;
    private readonly Action<T> getAction;
    private readonly Action<T> returnAction;

    private Queue<T> pool = new Queue<T>();


    public ObjectPool(Func<T> _preloadFunc, Action<T> _getAction, Action<T> _returnAction, int preloadCount)
    {
        preloadFunc = _preloadFunc;
        getAction = _getAction;
        returnAction = _returnAction;

        if (preloadFunc == null)
        {
            Debug.LogError("Preload function is null");
            return;
        }

        for (int i = 0; i < preloadCount; i++)
            Return(_preloadFunc());

    }

    public T Get()
    {
        T item = pool.Count > 0 ? pool.Dequeue() : preloadFunc();
        getAction(item);
        return item;
    }

    public void Return(T item)
    {
        returnAction(item);
        pool.Enqueue(item);
    }

}
