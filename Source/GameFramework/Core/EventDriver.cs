using System;
using System.Collections.Generic;
using static GameFramework.Core.EventDriver;

namespace GameFramework.Core
{
    public static class EventDriver
    {
        private static readonly Dictionary<string, MessageDelegate> Listeners = new();
        public delegate void MessageDelegate(string InChannel, object Data);

        public static ListenerHandle RegisterListener(string InChannel, MessageDelegate InDelegate)
        {
            if (!Listeners.ContainsKey(InChannel))
            {
                Listeners.Add(InChannel, InDelegate);
                return new ListenerHandle(InChannel, InDelegate);
            }

            Listeners[InChannel] += InDelegate;
            return new ListenerHandle(InChannel, InDelegate);
        }

        public static void UnregisterListener(ListenerHandle Handle)
        {
            if (!Listeners.ContainsKey(Handle.Channel)) return;
            Listeners[Handle.Channel] -= Handle.Delegate;
        }

        public static void BroadcastMessage(string InChannel, object Data)
        {
            if (!Listeners.ContainsKey(InChannel)) return;
            if (Listeners[InChannel] == null) return;

            List<MessageDelegate> InvalidListenerHandles = null;
            Delegate[] Delegates = Listeners[InChannel].GetInvocationList();

            for (int i = 0; i < Delegates.Length; i++)
            {
                if (Delegates[i] is not MessageDelegate Delegate) continue;

                Delegate.Invoke(InChannel, Data);
            }

            if (InvalidListenerHandles == null) return;

            for (int i = 0; i < InvalidListenerHandles.Count; i++)
            {
                Listeners[InChannel] -= InvalidListenerHandles[i];
            }
        }
    }

    public class ListenerHandle
    {
        public ListenerHandle(string InChannel, MessageDelegate InDelegate)
        {
            Channel = InChannel;
            Delegate = InDelegate;
        }

        public readonly string Channel;
        public readonly MessageDelegate Delegate;
    }

    //test
    public struct MessageData
    {
        public string Name;
    }
}