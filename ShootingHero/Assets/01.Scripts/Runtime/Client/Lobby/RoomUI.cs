using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ShootingHero.LobbyServer;
using UnityEngine;
using UnityEngine.UI;
using ShootingHero.Shared;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;
using TMPro;

namespace ShootingHero.Clients
{
    public class RoomUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject roomExitedObject = null;

        [SerializeField]
        private GameObject gameStartedObject = null;

        [SerializeField]
        private TMP_Text roomIDText = null;

        [SerializeField]
        private Button readyButton = null;

        [SerializeField]
        private Button startButton = null;

        [SerializeField]
        private Transform roomMemberElementUIContainer = null;

        [SerializeField]
        private RoomMemberElementUI roomMemberElementUIPrefab = null;

        private string roomUUID = null;
        private int roomCommandCursor = -1;

        private bool isRoomValid = true;
        private Dictionary<string, RoomMemberElementUI> roomMemberElementUIList = null;
        
        public void Initialize(string roomUUID)
        {
            this.roomUUID = roomUUID;
            roomCommandCursor = -1;

            isRoomValid = true;
            roomMemberElementUIList = new Dictionary<string, RoomMemberElementUI>();

            roomIDText.text = roomUUID;
            readyButton.gameObject.SetActive(true);
            startButton.gameObject.SetActive(false);

            UpdateLobbyCommandsAsync().Forget();
        }

        public void OnTouchCopyRoomID()
        {
            GUIUtility.systemCopyBuffer = roomIDText.text;
        }

        public async void OnTouchReadyButton()
        {
            ReadyRoomRequest request = new ReadyRoomRequest() {
                RoomUUID = roomUUID,
                Nickname = ClientInstance.Nickname
            };
            ReadyRoomResponse response = await new LobbyRoomRequest<ReadyRoomRequest, ReadyRoomResponse>(request).RequestAsync();
            if(response == null)
                return;
            
            readyButton.interactable = false;
        }

        public void OnTouchStartButton()
        {
            StartRoomRequest request = new StartRoomRequest() { RoomUUID = roomUUID };
            new LobbyRoomRequest<StartRoomRequest, StartRoomResponse>(request).RequestAsync().Forget();
        }

        public async void OnTouchExitButton()
        {
            ExitRoomRequest request = new ExitRoomRequest() {
                RoomUUID = roomUUID,
                Nickname = ClientInstance.Nickname
            };
            await new LobbyRoomRequest<ExitRoomRequest, ExitRoomResponse>(request).RequestAsync();
        }

        public void OnTouchReturnToRoomEntryButton()
        {
            SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }

        private async UniTask UpdateLobbyCommandsAsync()
        {
            try
            {
                UpdateRoomCommandRequest request = new UpdateRoomCommandRequest() {
                    RoomUUID = roomUUID,
                    Cursor = roomCommandCursor
                };
                UpdateRoomCommandResponse response = await new LobbyRoomRequest<UpdateRoomCommandRequest, UpdateRoomCommandResponse>(request).RequestAsync(destroyCancellationToken);
                if(response != null)
                    ApplyRoomCommands(response.RoomCommands);
            }
            catch(OperationCanceledException)
            {
                isRoomValid = false;
            }
            finally
            {
                if(isRoomValid != false)
                    UpdateLobbyCommandsAsync().Forget();
            }
        }

        private void ApplyRoomCommands(List<RoomCommand> roomCommands)
        {
            if(roomCommands == null || roomCommands.Count <= 0)
                return;
            
            roomCommandCursor += roomCommands.Count;
            foreach(RoomCommand roomCommand in roomCommands)
                ApplyRoomCommand(roomCommand);
        }

        private void ApplyRoomCommand(RoomCommand roomCommand)
        {
            switch(roomCommand.RoomCommandType)
            {
                case ERoomCommandType.Create:
                    {
                        string nickname = roomCommand.RoomCommandData;
                        if(nickname == ClientInstance.Nickname)
                        {
                            readyButton.gameObject.SetActive(false);
                            startButton.gameObject.SetActive(true);
                        }

                        RoomMemberElementUI roomMemberElementUI = AddRoomMember(nickname);
                        roomMemberElementUI.SetOwner();
                        break;   
                    }

                case ERoomCommandType.Close:
                    {
                        ExitRoom();
                        break;
                    }

                case ERoomCommandType.Join:
                    {
                        string nickname = roomCommand.RoomCommandData;
                        AddRoomMember(nickname);
                        break;
                    }

                case ERoomCommandType.Exit:
                    {
                        if(roomMemberElementUIList.TryGetValue(roomCommand.RoomCommandData, out RoomMemberElementUI roomMemberElementUI) == true)
                        {
                            Destroy(roomMemberElementUI.gameObject);
                            roomMemberElementUIList.Remove(roomCommand.RoomCommandData);
                        }

                        if(roomCommand.RoomCommandData == ClientInstance.Nickname)
                            ExitRoom();

                        break;
                    }

                case ERoomCommandType.Ready:
                    {
                        if(roomMemberElementUIList.TryGetValue(roomCommand.RoomCommandData, out RoomMemberElementUI roomMemberElementUI) == true)
                            roomMemberElementUI.SetReady();

                        break;
                    }

                case ERoomCommandType.Start:
                    {
                        isRoomValid = false;

                        TryEnterGameAsync(roomCommand.RoomCommandData).Forget();
                        gameStartedObject.SetActive(true);
                        break;
                    }
            }
        }

        private RoomMemberElementUI AddRoomMember(string nickname)
        {
            RoomMemberElementUI roomMemberElementUI = Instantiate(roomMemberElementUIPrefab, roomMemberElementUIContainer);
            roomMemberElementUIList[nickname] = roomMemberElementUI;
            roomMemberElementUI.Initialize(nickname);

            return roomMemberElementUI;
        }

        private void ExitRoom()
        {
            isRoomValid = false;
            roomExitedObject.SetActive(true);   
        }

        private async UniTask TryEnterGameAsync(string gameUUID)
        {
            try
            {
                GetGameConnectionRequest request = new GetGameConnectionRequest() { GameUUID = gameUUID };
                GetGameConnectionResponse response = await new LobbyGameRequest<GetGameConnectionRequest, GetGameConnectionResponse>(request).RequestAsync(destroyCancellationToken);
                if(response == null || string.IsNullOrEmpty(response.Host) == true || response.Port == -1)
                {
                    await UniTask.Delay(500);
                    TryEnterGameAsync(gameUUID).Forget();
                }

                GameClient gameClient = new GameClient();
                gameClient.Intialize(GameInstance.DataTableManager, GameManager.Instance, Random.Range(0, 4));
                gameClient.Connect(response.Host, response.Port);
            }
            catch { }
        }
    }
}