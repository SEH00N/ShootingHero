using TMPro;
using UnityEngine;
using ShootingHero.LobbyServer;
using ShootingHero.Shared;

namespace ShootingHero.Clients
{
    public class RoomEntryUI : MonoBehaviour
    {
        [SerializeField]
        private RoomUI roomUI = null;

        [SerializeField]
        private TMP_InputField nicknameInputField = null;

        [SerializeField]
        private TMP_InputField joinRoomInputField = null;

        public async void OnTouchCreateRoomButton()
        {
            ClientInstance.Nickname = nicknameInputField.text.Trim();
            if(string.IsNullOrEmpty(ClientInstance.Nickname) == true)
                return;
            
            CreateRoomRequest request = new CreateRoomRequest() { Nickname = ClientInstance.Nickname };
            CreateRoomResponse response = await new LobbyRoomRequest<CreateRoomRequest, CreateRoomResponse>(request).RequestAsync();
            if(response == null || string.IsNullOrEmpty(response.RoomUUID) == true)
                return;
            
            roomUI.gameObject.SetActive(true);
            roomUI.Initialize(response.RoomUUID);

            gameObject.SetActive(false);
        }

        public async void OnTouchJoinRoomButton()
        {
            ClientInstance.Nickname = nicknameInputField.text.Trim();
            if(string.IsNullOrEmpty(ClientInstance.Nickname) == true)
                return;

            string roomUUID = joinRoomInputField.text.Trim();
            if(string.IsNullOrEmpty(roomUUID) == true)
                return;
            
            JoinRoomRequest request = new JoinRoomRequest() {
                Nickname = ClientInstance.Nickname,
                RoomUUID = roomUUID
            };
            JoinRoomResponse response = await new LobbyRoomRequest<JoinRoomRequest, JoinRoomResponse>(request).RequestAsync();
            if(response == null || response.Result == false)
                return;
            
            roomUI.gameObject.SetActive(true);
            roomUI.Initialize(roomUUID);

            gameObject.SetActive(false);
        }
    }
}
