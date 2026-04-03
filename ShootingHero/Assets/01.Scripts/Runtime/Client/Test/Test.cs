using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;
using TMPro;
using UnityEngine;

namespace ShootingHero.Clients
{
    public class Test : MonoBehaviour, IPacketDispatcher
    {
        [SerializeField]
        private TMP_Text textField = null;
        
        [SerializeField]
        private TMP_InputField textInputField = null;

        private Queue<Action> jobQueue = null;
        private object jobQueueLocker = new object();

        private Session session = null;
        private Client client = null;

        public TMP_Text TextField => textField;

        private void Start()
        {
            jobQueue = new Queue<Action>();

            session = new Session();
            session.OnOpenedEvent += HandleSessionOpened;

            client = new ClientBuilder(session, this)
                .AddSingleton<Test>(this)
                .Build(typeof(Test).Assembly, typeof(C2S_TestPacket).Assembly);
            client.Connect("127.0.0.1", 9999);
        }

        private void Update()
        {
            if(jobQueue.Count <= 0)
                return;
            
            lock(jobQueueLocker)
            {
                while(jobQueue.TryDequeue(out Action job))
                {
                    job?.Invoke();
                }
            }
        }

        ValueTask IPacketDispatcher.Dispatch(Session session, IPacket packet)
        {
            lock(jobQueueLocker)
            {
                jobQueue.Enqueue(() =>
                {
                    PacketHandlerFactory packetHandlerFactory = client.GetInstance<PacketHandlerFactory>();
                    IPacketHandlerBase packetHander = packetHandlerFactory.Create(packet.GetType());
                    packetHander?.HandlePacket(session, packet);
                });
            }

            return new ValueTask();
        }

        public void SendMessageToServer()
        {
            if(session.IsOpened == false)
                return;

            if(string.IsNullOrEmpty(textInputField.text) == true)
                return;
            
            session.SendAsync(new C2S_TestPacket() {
                Message = textInputField.text
            });

            textInputField.text = "";
        }

        private void HandleSessionOpened(Session session)
        {
            session.SendAsync(new C2S_TestPacket() {
                Message = "Hello"
            });
        }
    }
}