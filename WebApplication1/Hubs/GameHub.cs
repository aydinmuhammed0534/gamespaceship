using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Hubs
{
    public class GameHub : Hub
    {
        public async Task UpdateGameState(GameState state)
        {
            await Clients.All.SendAsync("ReceiveGameState", state);
        }

        public async Task PlayerMoved(float x, float y)
        {
            await Clients.Others.SendAsync("PlayerMoved", x, y);
        }

        public async Task BulletFired(float x, float y)
        {
            await Clients.Others.SendAsync("BulletFired", x, y);
        }

        public async Task PowerUpCollected(int powerUpId)
        {
            await Clients.Others.SendAsync("PowerUpCollected", powerUpId);
        }

        public async Task UpdateScore(string playerId, int score)
        {
            await Clients.All.SendAsync("ScoreUpdated", playerId, score);
        }

        public async Task GameOver(string playerId, int finalScore)
        {
            await Clients.All.SendAsync("GameOver", playerId, finalScore);
        }
    }
}
