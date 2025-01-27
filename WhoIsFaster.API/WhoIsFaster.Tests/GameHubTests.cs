using Microsoft.AspNetCore.SignalR;
using Moq;
using WhoIsFaster.API;

namespace WhoIsFaster.Tests
{
    public class GameHubTests
    {
        private readonly GameHub _gameHub;
        private readonly Mock<IHubContext<GameHub>> _hubContextMock;
        private readonly Mock<IClientProxy> _clientProxyMock;
        private readonly Mock<HubCallerContext> _hubCallerContextMock;
        private readonly Mock<IGroupManager> _groupManagerMock;

        public GameHubTests()
        {
            // Mocking the necessary dependencies for SignalR
            _hubContextMock = new Mock<IHubContext<GameHub>>();
            _clientProxyMock = new Mock<IClientProxy>();
            _groupManagerMock = new Mock<IGroupManager>();

            // Mocking the HubCallerContext to provide the Context.ConnectionId
            _hubCallerContextMock = new Mock<HubCallerContext>();
            _hubCallerContextMock.Setup(c => c.ConnectionId).Returns("mockConnectionId");

            // Assigning the mocked HubCallerContext to the GameHub
            _gameHub = new GameHub
            {
                Context = _hubCallerContextMock.Object,
                Groups = _groupManagerMock.Object
            };
        }

        [Fact]
        public async Task CreateRoom_ShouldReturnSuccess_WhenRoomIsCreated()
        {
            // Arrange
            var roomName = "Room1";
            var userName = "User1";

            // Act
            var result = await _gameHub.CreateRoom(roomName, userName);

            // Assert
            Assert.Equal("Success", result);
        }

        [Fact]
        public async Task CreateRoom_ShouldReturnError_WhenRoomAlreadyExists()
        {
            // Arrange
            var roomName = "Room1";
            var userName = "User1";

            await _gameHub.CreateRoom(roomName, userName); // Create the room initially

            // Act
            var result = await _gameHub.CreateRoom(roomName, userName); // Try to create it again

            // Assert
            Assert.Equal("A sala já existe.", result);
        }

        [Fact]
        public async Task JoinRoom_ShouldReturnError_WhenRoomDoesNotExist()
        {
            // Arrange
            var roomName = "NonExistentRoom";
            var userName = "User1";

            // Act
            var result = await _gameHub.JoinRoom(roomName, userName);

            // Assert
            Assert.Equal("A sala não existe.", result);
        }

        [Fact]
        public async Task JoinRoom_ShouldReturnError_WhenRoomIsFull()
        {
            // Arrange
            var roomName = "Room1";
            var userName1 = "User1";
            var userName2 = "User2";

            await _gameHub.CreateRoom(roomName, userName1); // Create the room

            // Act
            var result = await _gameHub.JoinRoom(roomName, userName2); // Add the second user
            var result2 = await _gameHub.JoinRoom(roomName, "User3"); // Try adding a third user

            // Assert
            Assert.Equal("Success", result);
            Assert.Equal("A sala está cheia.", result2);
        }

        //[Fact]
        //public async Task StartGame_ShouldReturnError_WhenNotEnoughPlayers()
        //{
        //    // Arrange
        //    var roomName = "Room1";
        //    var userName1 = "User1";

        //    await _gameHub.CreateRoom(roomName, userName1); // Create the room with only one player

        //    // Act
        //    var result = await _gameHub.StartGame(roomName);

        //    // Assert
        //    Assert.Equal("Necessário no mínimo 2 jogadores para iniciar o jogo.", result);
        //}

        //[Fact]
        //public async Task PlayerClick_ShouldSendGameEnd_WhenAllPlayersClicked()
        //{
        //    // Arrange
        //    var roomName = "Room1";
        //    var userName1 = "User1";
        //    var userName2 = "User2";

        //    await _gameHub.CreateRoom(roomName, userName1);
        //    await _gameHub.JoinRoom(roomName, userName2);

        //    // Mocking the group send method
        //    _hubContextMock.Setup(h => h.Clients.Group(roomName).SendAsync(It.IsAny<string>(), It.IsAny<object[]>()));

        //    // Act
        //    await _gameHub.PlayerClick(roomName); // User1 clicks
        //    await _gameHub.PlayerClick(roomName); // User2 clicks

        //    // Assert
        //    _hubContextMock.Verify(h => h.Clients.Group(roomName).SendAsync("GameEnded", It.IsAny<object[]>()), Times.Once);
        //}

        //[Fact]
        //public async Task OnDisconnectedAsync_ShouldRemovePlayerAndUpdateRoom()
        //{
        //    // Arrange
        //    var roomName = "Room1";
        //    var userName = "User1";

        //    await _gameHub.CreateRoom(roomName, userName);

        //    // Act
        //    await _gameHub.OnDisconnectedAsync(null);

        //    // Assert
        //    var room = GameHub.Rooms[roomName];
        //    Assert.Empty(room);
        //}
    }
}