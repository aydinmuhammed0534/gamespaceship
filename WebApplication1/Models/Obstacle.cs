using System;

namespace WebApplication1.Models
{
    public class Obstacle : GameObject
    {
        public int Health { get; set; }
        public int Damage { get; set; }

        public Obstacle(float x, float y, float width, float height) : base(x, y)
        {
            Health = 50;
            Damage = 30;
            Speed = 100f;
            Width = width;
            Height = height;
            SpriteUrl = "/images/obstacle.png";
        }

        public override void Update(float deltaTime, Game game)
        {
            if (!IsActive) return;

            // Move down
            Y += Speed * deltaTime;

            // Deactivate if off screen
            if (Y > 600)
            {
                IsActive = false;
                return;
            }

            // Check collision with player
            if (game.Player != null && GetBounds().Intersects(game.Player.GetBounds()))
            {
                game.Player.TakeDamage(Damage);
                IsActive = false;
            }
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health <= 0)
            {
                Health = 0;
                IsActive = false;
            }
        }
    }
}
