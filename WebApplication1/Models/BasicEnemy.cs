using System;

namespace WebApplication1.Models
{
    public class BasicEnemy : Enemy
    {
        private float _attackTimer = 0;
        private float _attackInterval = 2.0f;

        public BasicEnemy(float spawnX, float spawnY) 
            : base(spawnX, spawnY, health: 50, speed: 100, damage: 10)
        {
        }

        public override void Move(float playerX, float playerY, float deltaTime)
        {
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
    }
}
