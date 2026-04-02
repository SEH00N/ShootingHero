using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ShootingHero.Networks
{
    public class Room
    {
        public interface ICallback
        {
            void OnAdded(Room room, Session session);
            void OnRemoved(Room room, Session session);
        }

        private readonly ConcurrentDictionary<string, Session> sessions = null;
        private readonly ConcurrentDictionary<Session, Action<Session>> sessionClosedHandlers = null;

        private readonly int roomIDHash = 0;
        private readonly PacketSerializer packetSerializer = null;
        private readonly ICallback callback = null;

        public int RoomIDHash => roomIDHash;

        public Room(string roomID, PacketSerializer packetSerializer, ICallback callback)
        {
            this.roomIDHash = roomID.GetHashCode();
            this.packetSerializer = packetSerializer;
            this.callback = callback;

            sessions = new ConcurrentDictionary<string, Session>();
            sessionClosedHandlers = new ConcurrentDictionary<Session, Action<Session>>();
        }

        public void Add(string sessionID, Session session)
        {
            if (string.IsNullOrEmpty(sessionID) || session == null)
                return;

            if (sessions.TryAdd(sessionID, session) == false)
                return;

            callback.OnAdded(this, session);

            void HandleSessionClosed(Session _) => Remove(sessionID);
            sessionClosedHandlers[session] = HandleSessionClosed;
            session.OnClosedEvent += HandleSessionClosed;
        }

        public void Remove(string sessionID)
        {
            if (string.IsNullOrEmpty(sessionID))
                return;

            if (sessions.TryRemove(sessionID, out Session session) == false || session == null)
                return;

            if (sessionClosedHandlers.TryRemove(session, out Action<Session> closedHandler) == true)
                session.OnClosedEvent -= closedHandler;

            callback.OnRemoved(this, session);
        }

        public void Send(IPacket packet, Func<string, Session, bool> filter = null)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            RoomPacketSendQueueContext sharedContext = null;
            bool anySent = false;

            foreach (KeyValuePair<string, Session> kvp in sessions)
            {
                string sessionID = kvp.Key;
                Session session = kvp.Value;

                if (session == null || session.IsOpened == false)
                    continue;

                if (filter != null && filter(sessionID, session) == false)
                    continue;

                if (sharedContext == null)
                    sharedContext = new RoomPacketSendQueueContext(packetSerializer, packet, 1);
                else
                    sharedContext.AddReference();

                try
                {
                    session.SendAsync(sharedContext);
                    anySent = true;
                }
                catch
                {
                    sharedContext?.Dispose();
                    throw;
                }
            }

            if (anySent == false)
                sharedContext?.Dispose();
        }

        public Session Session(string sessionID)
        {
            if (string.IsNullOrEmpty(sessionID) == true)
                return null;

            sessions.TryGetValue(sessionID, out Session session);
            return session;
        }
    }
}
