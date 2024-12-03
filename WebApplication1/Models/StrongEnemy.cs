using System;

namespace WebApplication1.Models
{
    public class StrongEnemy : Enemy
    {
        private float _attackTimer = 0;
        private float _attackInterval = 1.5f;
        private float _dodgeTimer = 0;
        private float _dodgeInterval = 3.0f;
        private float _dodgeDistance = 100f;
        private bool _isDodging = false;
        private float _dodgeDirection = 1;

        public StrongEnemy(float spawnX, float spawnY)
            : base(spawnX, spawnY, health: 100, speed: 150, damage: 20)
        {
            ScoreValue = 20;
        }

        public override void Move(float playerX, float playerY, float deltaTime)
        {
            _dodgeTimer += deltaTime;

            if (_isDodging)
            {
                // Kaçış hareketi
                X += Speed * _dodgeDirection * deltaTime;

                // Ekran sınırlarına geldiğinde yön değiştir
                if (X <= 0 || X >= 800 - Width)
                {
                    _dodgeDirection *= -1;
                }

                if (_dodgeTimer >= _dodgeInterval)
                {
                    _isDodging = false;
                    _dodgeTimer = 0;
                }
            }
            else
            {
                // Normal takip hareketi
                float dx = playerX - X;
                float dy = playerY - Y;
                float distance = CalculateDistance(X, Y, playerX, playerY);

                if (distance > 0)
                {
                    X += (dx / distance) * Speed * deltaTime;
                    Y += (dy / distance) * Speed * deltaTime;
                }

                // Belirli aralıklarla kaçış hareketini başlat
                if (_dodgeTimer >= _dodgeInterval)
                {
                    _isDodging = true;
                    _dodgeTimer = 0;
                    _dodgeDirection = (float)new Random().NextDouble() > 0.5f ? 1 : -1;
                }
            }

            // Ekran sınırları içinde tut
            X = Math.Max(0, Math.Min(X, 800 - Width));
            Y = Math.Max(0, Math.Min(Y, 600 - Height));
        }

        public override void Attack(float playerX, float playerY)
        {
            _attackTimer += 0.016f; // Yaklaşık 60 FPS için deltaTime

            if (_attackTimer >= _attackInterval)
            {
                // Üçlü atış yapacak
                float bulletSpeed = 300f;
                float angle = (float)Math.Atan2(playerY - Y, playerX - X);
                
                // Merkezde bir mermi
                Bullets.Add(new Bullet(X + Width / 2, Y + Height,
                    (float)Math.Cos(angle) * bulletSpeed,
                    (float)Math.Sin(angle) * bulletSpeed,
                    Damage, 5f));

                // Sağa ve sola açılı iki mermi daha
                float spreadAngle = 0.3f; // Yaklaşık 15 derece
                Bullets.Add(new Bullet(X + Width / 2, Y + Height,
                    (float)Math.Cos(angle + spreadAngle) * bulletSpeed,
                    (float)Math.Sin(angle + spreadAngle) * bulletSpeed,
                    Damage, 5f));
                Bullets.Add(new Bullet(X + Width / 2, Y + Height,
                    (float)Math.Cos(angle - spreadAngle) * bulletSpeed,
                    (float)Math.Sin(angle - spreadAngle) * bulletSpeed,
                    Damage, 5f));

                _attackTimer = 0;
            }
        }
    }
}
