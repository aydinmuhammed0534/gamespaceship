using System;

namespace WebApplication1.Models
{
    public class BasicEnemy : Enemy
    {
        private float _attackTimer = 0;
        private float _attackInterval = 2.0f;
        private float _shootTimer = 0;
        private const float SHOOT_INTERVAL = 2.0f; // Her 2 saniyede bir ateş eder

        public BasicEnemy(float spawnX, float spawnY) 
            : base(spawnX, spawnY, health: 50, speed: 100, damage: 10)
        {
        }

        public override void Move(float playerX, float playerY, float deltaTime)
        {
            _shootTimer += deltaTime;
            if (_shootTimer >= SHOOT_INTERVAL)
            {
                Shoot(playerX, playerY);
                _shootTimer = 0;
            }

            // Basit düşman direk oyuncuya doğru hareket eder
            float dx = playerX - X;
            float dy = playerY - Y;
            float distance = CalculateDistance(X, Y, playerX, playerY);

            if (distance > 0)
            {
                X += (dx / distance) * Speed * deltaTime;
                Y += (dy / distance) * Speed * deltaTime;
            }

            // Ekran sınırları içinde tut
            X = Math.Max(0, Math.Min(X, 800 - 40)); // 40 düşmanın genişliği
            Y = Math.Max(0, Math.Min(Y, 600 - 40)); // 40 düşmanın yüksekliği
        }

        public override void Attack(float playerX, float playerY)
        {
            _attackTimer += 0.016f; // Yaklaşık 60 FPS için deltaTime

            if (_attackTimer >= _attackInterval)
            {
                // Burada mermi oluşturma işlemi Game sınıfında yapılacak
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
                VelocityX = directionX * 5f,
                VelocityY = directionY * 5f,
                Damage = this.Damage * 0.5f,
                IsEnemyBullet = true
            };
            Bullets.Add(bullet);
        }
    }
}
