using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class GameState
    {
        public PlayerState Player { get; set; }
        public List<EnemyState> Enemies { get; set; }
        public List<PowerUpState> PowerUps { get; set; }
        public List<BulletState> Bullets { get; set; }
        public List<ObstacleState> Obstacles { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }
        public bool IsGameOver { get; set; }
        public bool IsPaused { get; set; }
    }

    public class PlayerState
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Health { get; set; }
        public bool HasShield { get; set; }
        public bool HasSpeedBoost { get; set; }
        public bool HasDamageBoost { get; set; }
        public bool HasRapidFire { get; set; }
    }

    public class EnemyState
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Health { get; set; }
        public string Type { get; set; }
    }

    public class PowerUpState
    {
        public float X { get; set; }
        public float Y { get; set; }
        public string Type { get; set; }
    }

    public class BulletState
    {
        public float X { get; set; }
        public float Y { get; set; }
        public bool IsPlayerBullet { get; set; }
    }

    public class ObstacleState
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Size { get; set; }
    }
}
