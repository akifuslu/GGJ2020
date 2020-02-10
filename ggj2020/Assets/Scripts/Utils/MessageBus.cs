using System;
using UniRx;

namespace Assets.Scripts.Utils
{
    public class GameEvent
    {

    }

    public static class MessageBus
    {

        private static MessageBroker _messageBroker = new MessageBroker();

        public static IObservable<T> OnEvent<T>() where T : GameEvent
        {
            return _messageBroker.Receive<T>();
        }

        public static void Publish<T>(T evnt) where T : GameEvent
        {
            _messageBroker.Publish(evnt);
        }

        public static void Clear()
        {
            _messageBroker.Dispose();
        }
    }
}
