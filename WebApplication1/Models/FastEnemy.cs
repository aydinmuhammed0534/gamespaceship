using System;

namespace WebApplication1.Models
{
    public class FastEnemy : Enemy
    {
        private float _dodgeTimer = 0;
        private float _dodgeInterval = 1.5f;
        private int _dodgeDirection = 1;
        private float _shootTimer = 0;
        private const float SHOOT_INTERVAL = 1.0f; // Hızlı düşman her 1 saniyede bir ateş eder

        public FastEnemy(float spawnX, float spawnY)
            : base(spawnX, spawnY, health: 30, speed: 200, damage: 5)
        {
        }

        public override void Move(float playerX, float playerY, float deltaTime)
        {
            // Hızlı düşman kaçınma hareketleri yapar
            _dodgeTimer += deltaTime;
            if (_dodgeTimer >= _dodgeInterval)
            {
                _dodgeDirection *= -1;
                _dodgeTimer = 0;
            }

            // Oyuncuya doğru hareket et
            float distance = CalculateDistance(X, Y, playerX, playerY);
            float directionX = (playerX - X) / distance;
            float directionY = (playerY - Y) / distance;

            // Kaçınma hareketi ekle
            X += (directionX * Speed + _dodgeDirection * Speed * 0.5f) * deltaTime;
            Y += directionY * Speed * deltaTime;

            _shootTimer += deltaTime;
            if (_shootTimer >= SHOOT_INTERVAL)
            {
                Shoot(playerX, playerY);
                _shootTimer = 0;
            }
        }

        public override void Attack(float playerX, float playerY)
        {
            // Hızlı düşman yakın mesafeden saldırır
            // Mermi oluşturma işlemi Game sınıfında yapılacak
        }

        protected override void Shoot(float playerX, float playerY)
        {
            float directionX = playerX - X;
            float directionY = playerY - Y;
            float length = (float)Math.Sqrt(directionX * directionX + directionY * directionY);
            directionX /= length;
            directionY /= length;

            // Hızlı düşman daha hızlı ama daha zayıf mermiler atar
            var bullet = new Bullet(X, Y)
            {
                VelocityX = directionX * 8f,
                VelocityY = directionY * 8f,
                Damage = this.Damage * 0.3f,
                IsEnemyBullet = true
            };
            Bullets.Add(bullet);
        }
    }
}
