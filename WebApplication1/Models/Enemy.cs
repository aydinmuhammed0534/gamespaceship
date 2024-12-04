using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public abstract class Enemy : GameObject
    {
        public float Health { get; protected set; }
        public new float Speed { get; protected set; }
        public float Damage { get; protected set; }
        public new bool IsActive { get; protected set; }
        public int ScoreValue { get; protected set; }
        public List<Bullet> Bullets { get; protected set; }
        protected float DifficultyMultiplier { get; set; } = 1.0f;

        protected Enemy(float spawnX, float spawnY, float health, float speed, float damage)
            : base(spawnX, spawnY)
        {
            Health = health;
            Speed = speed;
            Damage = damage;
            IsActive = true;
            Width = 40;
            Height = 40;
            ScoreValue = 10;
            Bullets = new List<Bullet>();
        }

        public abstract void Move(float playerX, float playerY, float deltaTime);
        public abstract void Attack(float playerX, float playerY);

        protected virtual void Shoot(float playerX, float playerY)
        {
            float directionX = playerX - X;
            float directionY = playerY - Y;
            float length = (float)Math.Sqrt(directionX * directionX + directionY * directionY);
            directionX /= length;
            directionY /= length;

            var bullet = new Bullet(X, Y)
            {
                VelocityX = directionX * 5f,
                VelocityY = directionY * 5f,
                Damage = this.Damage * 0.5f,
                IsEnemyBullet = true
            };
            Bullets.Add(bullet);
        }

        public void UpdateBullets(float deltaTime, Game game)
        {
            for (int i = Bullets.Count - 1; i >= 0; i--)
            {
                Bullets[i].Update(deltaTime, game);
                if (!Bullets[i].IsActive)
                {
                    Bullets.RemoveAt(i);
                }
            }
        }

        public override void Update(float deltaTime, Game game)
        {
            if (!IsActive) return;

            Move(game.Player.X, game.Player.Y, deltaTime);
            Attack(game.Player.X, game.Player.Y);
            UpdateBullets(deltaTime, game);

            // Ekran dışına çıkan düşmanları kaldır
            if (Y > 600 + Height)
            {
                IsActive = false;
            }
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                IsActive = false;
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

        protected float CalculateDistance(float x1, float y1, float x2, float y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public void SetDifficultyMultiplier(float multiplier)
        {
            DifficultyMultiplier = multiplier;
            Speed *= multiplier;
            Damage *= multiplier;
            Health *= multiplier;
        }
    }
}