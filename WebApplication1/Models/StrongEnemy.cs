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
        private float _shootTimer = 0;
        private const float SHOOT_INTERVAL = 3.0f; // Güçlü düşman her 3 saniyede bir ateş eder

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

            _shootTimer += deltaTime;
            if (_shootTimer >= SHOOT_INTERVAL)
            {
                // Güçlü düşman 3 mermi birden atar
                ShootMultiple(playerX, playerY);
                _shootTimer = 0;
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
                var bullet = new Bullet(X + Width / 2, Y + Height)
                {
                    VelocityX = (float)Math.Cos(angle) * bulletSpeed,
                    VelocityY = (float)Math.Sin(angle) * bulletSpeed,
                    Damage = this.Damage
                };
                Bullets.Add(bullet);

                // Sağa ve sola açılı iki mermi daha
                float spreadAngle = 0.3f; // Yaklaşık 15 derece
                var bulletRight = new Bullet(X + Width / 2, Y + Height)
                {
                    VelocityX = (float)Math.Cos(angle + spreadAngle) * bulletSpeed,
                    VelocityY = (float)Math.Sin(angle + spreadAngle) * bulletSpeed,
                    Damage = this.Damage
                };
                Bullets.Add(bulletRight);
                var bulletLeft = new Bullet(X + Width / 2, Y + Height)
                {
                    VelocityX = (float)Math.Cos(angle - spreadAngle) * bulletSpeed,
                    VelocityY = (float)Math.Sin(angle - spreadAngle) * bulletSpeed,
                    Damage = this.Damage
                };
                Bullets.Add(bulletLeft);

                _attackTimer = 0;
            }
        }

        protected override void Shoot(float playerX, float playerY)
        {
            float directionX = playerX - X;
            float directionY = playerY - Y;
            float length = (float)Math.Sqrt(directionX * directionX + directionY * directionY);
            directionX /= length;
            directionY /= length;

            var bullet = new Bullet(X, Y)
            {
                VelocityX = directionX * 6f,
                VelocityY = directionY * 6f,
                Damage = this.Damage * 0.7f,
                IsEnemyBullet = true
            };
            Bullets.Add(bullet);
        }

        private void ShootMultiple(float playerX, float playerY)
        {
            // Düz mermi
            base.Shoot(playerX, playerY);
            
            // Sağa açılı mermi
            float angleRight = 15f * (float)Math.PI / 180f; // 15 derece
            float dirXRight = (float)(Math.Cos(angleRight) * (playerX - X) - Math.Sin(angleRight) * (playerY - Y));
            float dirYRight = (float)(Math.Sin(angleRight) * (playerX - X) + Math.Cos(angleRight) * (playerY - Y));
            ShootInDirection(dirXRight, dirYRight);
            
            // Sola açılı mermi
            float angleLeft = -15f * (float)Math.PI / 180f; // -15 derece
            float dirXLeft = (float)(Math.Cos(angleLeft) * (playerX - X) - Math.Sin(angleLeft) * (playerY - Y));
            float dirYLeft = (float)(Math.Sin(angleLeft) * (playerX - X) + Math.Cos(angleLeft) * (playerY - Y));
            ShootInDirection(dirXLeft, dirYLeft);
        }

        private void ShootInDirection(float dirX, float dirY)
        {
            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            dirX /= length;
            dirY /= length;

            var bullet = new Bullet(X, Y)
            {
                VelocityX = dirX * 5f,
                VelocityY = dirY * 5f,
                Damage = this.Damage
            };
            Bullets.Add(bullet);
        }
    }
}
