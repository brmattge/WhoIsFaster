using Microsoft.AspNetCore.SignalR;

namespace WhoIsFaster.API
{
    public class GameHub : Hub
    {
        private static Dictionary<string, List<PlayerModel>> Rooms = new();
        private static Dictionary<string, int> CurrentPlayerIndex = new();

        public async Task<string> CreateRoom(string roomName, string userName)
        {
            if (Rooms.ContainsKey(roomName))
            {
                return "A sala já existe.";
            }

            Rooms[roomName] = new List<PlayerModel>();
            CurrentPlayerIndex[roomName] = 0;

            var creator = new PlayerModel
            {
                Name = userName,
                ConnectionId = Context.ConnectionId,
                TotalTime = 0
            };

            Rooms[roomName].Add(creator);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

            await Clients.Group(roomName).SendAsync("UpdateRoom", Rooms[roomName]);

            await Clients.Caller.SendAsync("RoomCreated", roomName);

            return "Success";
        }

        public async Task<string> JoinRoom(string roomName, string userName)
        {
            if (!Rooms.ContainsKey(roomName))
            {
                return "A sala não existe.";
            }

            if (Rooms[roomName].Count >= 2)
            {
                return "A sala está cheia.";
            }

            if (Rooms[roomName].Any(player => player.Name == userName))
            {
                return "Já existe um usuário com esse nome.";
            }

            var player = new PlayerModel
            {
                Name = userName,
                ConnectionId = Context.ConnectionId,
                TotalTime = 0
            };

            Rooms[roomName].Add(player);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

            await Clients.Group(roomName).SendAsync("UpdateRoom", Rooms[roomName]);

            return "Success";
        }

        public async Task StartGame(string roomName)
        {
            if (Rooms.ContainsKey(roomName) && Rooms[roomName].Count == 2)
            {
                foreach (var player in Rooms[roomName])
                {
                    player.TotalTime = 0;
                }

                await Clients.Group(roomName).SendAsync("GameStarted");
            } else
            {
                await Clients.Caller.SendAsync("Error", "Necessário no mínimo 2 jogadores para iniciar o jogo.");
            }
        }

        public async Task PlayerClick(string roomName)
        {
            if (Rooms.ContainsKey(roomName))
            {
                var currentPlayer = Rooms[roomName].FirstOrDefault(player => player.ConnectionId == Context.ConnectionId);

                if (currentPlayer != null)
                {
                    var clickTime = DateTime.UtcNow;
                    var elapsedTime = (clickTime - currentPlayer.LastClickTime).TotalMilliseconds;
                    currentPlayer.TotalTime = elapsedTime;
                    currentPlayer.LastClickTime = clickTime;

                    await Clients.Group(roomName).SendAsync("PlayerClicked", currentPlayer.Name, elapsedTime);

                    if (Rooms[roomName].All(player => player.TotalTime > 0))
                    {
                        var winner = Rooms[roomName].OrderBy(player => player.TotalTime).First();

                        var playerTimes = Rooms[roomName].Select(player => new { player.Name, player.TotalTime }).ToList();

                        await Clients.Group(roomName).SendAsync("GameEnded", winner.Name, playerTimes);

                        Rooms.Remove(roomName);
                    }
                }
            }
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            foreach (var room in Rooms.Keys)
            {
                var player = Rooms[room].FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
                if (player != null)
                {
                    Rooms[room].Remove(player);
                    Clients.Group(room).SendAsync("UpdateRoom", Rooms[room]);

                    if (Rooms[room].Count == 0)
                    {
                        Rooms.Remove(room);
                    }
                    break;
                }
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
