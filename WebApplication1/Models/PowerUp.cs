using System;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public enum PowerUpType
    {
        Health,
        Speed,
        Shield
    }

    public class PowerUp : GameObject
    {
        public PowerUpType Type { get; set; }
        public float Value { get; set; }
        public float Duration { get; set; }
        public new bool IsActive { get; set; } = true;

        // Default constructor
        public PowerUp() : base(0, 0)
        {
            Type = PowerUpType.Shield;
            Value = 1.0f;
            Duration = 10.0f;
            Width = 50;
            Height = 50;
            SpriteUrl = "/images/powerup_default.png";
        }

        // Parameterized constructor
        public PowerUp(PowerUpType type, float x = 0, float y = 0) : base(x, y)
        {
            Type = type;
            Width = 50;
            Height = 50;
            SpriteUrl = GetSpriteUrlForType(type);

            // Set default values based on type
            switch (type)
            {
                case PowerUpType.Shield:
                    Value = 1.0f;
                    Duration = 10.0f;
                    break;
                case PowerUpType.Health:
                    Value = 25.0f;
                    Duration = 0f; // Instant use
                    break;
                case PowerUpType.Speed:
                    Value = 1.5f;
                    Duration = 5.0f;
                    break;
            }
        }

        public PowerUp(float x, float y, PowerUpType type) : base(x, y)
        {
            Type = type;
            Width = 50;
            Height = 50;
            SpriteUrl = GetSpriteUrlForType(type);

            // Set default values based on type
            switch (type)
            {
                case PowerUpType.Shield:
                    Value = 1.0f;
                    Duration = 10.0f;
                    break;
                case PowerUpType.Health:
                    Value = 25.0f;
                    Duration = 0f; // Instant use
                    break;
                case PowerUpType.Speed:
                    Value = 1.5f;
                    Duration = 5.0f;
                    break;
            }
        }

        private string GetSpriteUrlForType(PowerUpType type)
        {
            return type switch
            {
                PowerUpType.Shield => "/images/powerup_shield.png",
                PowerUpType.Health => "/images/powerup_health.png",
                PowerUpType.Speed => "/images/powerup_speed.png",
                _ => "/images/powerup_default.png"
            };
        }

        public override void Update(float deltaTime, Game game)
        {
            if (!IsActive || game.IsPaused) return;

            // Move downward
            Y += 2 * deltaTime;

            // Deactivate if off screen
            if (Y > 600)
            {
                IsActive = false;
            }

            // Check collision with player
            if (game.Player != null && base.GetBounds().Intersects(game.Player.GetBounds()))
            {
                game.Player.ApplyPowerUp(Type, Value, Duration);
                IsActive = false;
            }
        }
    }
}
