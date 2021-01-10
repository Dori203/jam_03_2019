using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Helpers {
    public abstract class ListeningMonoBehaviour : SerializedMonoBehaviour {
        protected class Listener : BaseListener {
            public string Event;
            public Action Callback;
        
            public override void Add() => Messenger.AddListener(Event, Callback);
            public override void Remove() => Messenger.RemoveListener(Event, Callback);
        }
        
        protected class BaseListener<T> : BaseListener {
            public string Event;
            public Action<T> Callback;

            public override void Add() => Messenger<T>.AddListener(Event, Callback);
            public override void Remove() => Messenger<T>.RemoveListener(Event, Callback);
        }
        
        protected class BaseListener<T, TU> : BaseListener {
            public string Event;
            public Action<T, TU> Callback;

            public override void Add() => Messenger<T, TU>.AddListener(Event, Callback);
            public override void Remove() => Messenger<T, TU>.RemoveListener(Event, Callback);
        }

        protected abstract class BaseListener {
            public abstract void Add();
            public abstract void Remove();
        }
        
        protected abstract List<BaseListener> Listeners { get; }

        public virtual void Start() => Listeners.ForEach(listener => listener.Add());
        public virtual void OnDestroy() => Listeners.ForEach(listener => listener.Remove());
    }
}