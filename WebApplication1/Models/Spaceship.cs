using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class Spaceship : GameObject
    {
        public new float MaxHealth { get; private set; }
        public float Health { get; set; }
        public new float Speed { get; private set; }
        public new bool IsActive { get; private set; }
        public List<Bullet> Bullets { get; private set; }
        private float _shootCooldown = 0.2f;
        private float _lastShootTime = 0f;
        private float BulletSpeed = 10f;
        public float Damage { get; set; } = 10f;

        public Spaceship(float x, float y) : base(x, y)
        {
            MaxHealth = 100;
            Health = MaxHealth;
            Speed = 300f;
            IsActive = true;
            Width = 40;
            Height = 40;
            Bullets = new List<Bullet>();
            SpriteUrl = "/images/spaceship.png";
        }

        public override void Update(float deltaTime, Game game)
        {
            _lastShootTime += deltaTime;
            
            // Update bullets
            for (int i = Bullets.Count - 1; i >= 0; i--)
            {
                Bullets[i].Update(deltaTime, game);
                if (!Bullets[i].IsActive)
                {
                    Bullets.RemoveAt(i);
                }
            }
        }

        public void MoveLeft(float deltaTime)
        {
            X -= Speed * deltaTime;
            X = Math.Max(0, X);
        }

        public void MoveRight(float deltaTime)
        {
            X += Speed * deltaTime;
            X = Math.Min(800 - Width, X);
        }

        public void MoveUp(float deltaTime)
        {
            Y -= Speed * deltaTime;
            Y = Math.Max(0, Y);
        }

        public void MoveDown(float deltaTime)
        {
            Y += Speed * deltaTime;
            Y = Math.Min(600 - Height, Y);
        }

        public void Shoot()
        {
            if (_lastShootTime >= _shootCooldown)
            {
                var bullet = new Bullet(X + Width / 2, Y)
                {
                    VelocityX = 0,
                    VelocityY = -BulletSpeed,
                    Damage = this.Damage
                };
                Bullets.Add(bullet);

                _lastShootTime = 0;
            }
        }

        public new Rectangle GetBounds()
        {
            return new Rectangle
            {
                X = X,
                Y = Y,
                Width = Width,
                Height = Height
            };
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                IsActive = false;
            }
        }

        public void ApplyPowerUp(PowerUp powerUp)
        {
            switch (powerUp.Type)
            {
                case PowerUpType.Health:
                    Health = Math.Min(MaxHealth, Health + powerUp.Value);
                    break;
                case PowerUpType.Speed:
                    Speed *= 1.5f;
                    break;
                case PowerUpType.Shield:
                    // Kalkan uygulaması
                    break;
            }
        }

        public void ApplyPowerUp(PowerUpType powerUp, float value, float duration)
        {
            throw new NotImplementedException();
        }
    }
}