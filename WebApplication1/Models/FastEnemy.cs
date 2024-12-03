using System;

namespace WebApplication1.Models
{
    public class FastEnemy : Enemy
    {
        private float _dodgeTimer = 0;
        private float _dodgeInterval = 1.5f;
        private int _dodgeDirection = 1;

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
        }

        public override void Attack(float playerX, float playerY)
        {
            // Hızlı düşman yakın mesafeden saldırır
            // Mermi oluşturma işlemi Game sınıfında yapılacak
        }
    }
}
