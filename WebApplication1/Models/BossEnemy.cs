using System;

namespace WebApplication1.Models
{
    public class BossEnemy : Enemy
    {
        private float _attackTimer = 0;
        private float _attackInterval = 1.0f;
        private float _movementPhase = 0;
        private float _phaseSpeed = 1.0f;
        private float _amplitude = 100f;
        private bool _isEnraged = false;
        private float _enrageHealthThreshold = 0.3f; // 30% health

        public BossEnemy(float spawnX, float spawnY)
            : base(spawnX, spawnY, health: 500, speed: 100, damage: 30)
        {
            Width = 80;
            Height = 80;
            ScoreValue = 100;
        }

        public override void Move(float playerX, float playerY, float deltaTime)
        {
            _movementPhase += _phaseSpeed * deltaTime;

            // Sinüzoidal hareket
            float targetX = 400 + (float)Math.Sin(_movementPhase) * _amplitude;
            float dx = targetX - X;
            
            // Öfke durumunda daha hızlı hareket
            float currentSpeed = _isEnraged ? Speed * 1.5f : Speed;
            
            X += Math.Sign(dx) * currentSpeed * deltaTime;
            
            // Y pozisyonunu yavaşça ayarla
            float targetY = 100;
            float dy = targetY - Y;
            Y += Math.Sign(dy) * currentSpeed * 0.5f * deltaTime;

            // Ekran sınırları içinde tut
            X = Math.Max(0, Math.Min(X, 800 - Width));
            Y = Math.Max(0, Math.Min(Y, 600 - Height));

            // Öfke durumunu kontrol et
            if (!_isEnraged && Health <= MaxHealth * _enrageHealthThreshold)
            {
                _isEnraged = true;
                _attackInterval *= 0.7f; // Daha sık saldırı
                _phaseSpeed *= 1.5f; // Daha hızlı hareket
                _amplitude *= 1.2f; // Daha geniş hareket
            }
        }

        public override void Attack(float playerX, float playerY)
        {
            _attackTimer += 0.016f; // Yaklaşık 60 FPS için deltaTime

            if (_attackTimer >= _attackInterval)
            {
                if (_isEnraged)
                {
                    // Öfkeli durumda çoklu atış
                    float bulletSpeed = 300f;
                    int bulletCount = 8;
                    float angleStep = 2 * (float)Math.PI / bulletCount;

                    for (int i = 0; i < bulletCount; i++)
                    {
                        float angle = i * angleStep;
                        Bullets.Add(new Bullet(X + Width / 2, Y + Height / 2,
                            (float)Math.Cos(angle) * bulletSpeed,
                            (float)Math.Sin(angle) * bulletSpeed,
                            Damage, 10f)); // Damage parametresi eklendi
                    }
                }
                else
                {
                    // Normal durumda üçlü atış
                    float bulletSpeed = 250f;
                    float angle = (float)Math.Atan2(playerY - Y, playerX - X);
                    float spreadAngle = 0.2f;

                    // Merkez atış
                    Bullets.Add(new Bullet(X + Width / 2, Y + Height / 2,
                        (float)Math.Cos(angle) * bulletSpeed,
                        (float)Math.Sin(angle) * bulletSpeed,
                        Damage, 10f)); // Damage parametresi eklendi

                    // Yan atışlar
                    Bullets.Add(new Bullet(X + Width / 2, Y + Height / 2,
                        (float)Math.Cos(angle + spreadAngle) * bulletSpeed,
                        (float)Math.Sin(angle + spreadAngle) * bulletSpeed,
                        Damage, 10f)); // Damage parametresi eklendi
                    Bullets.Add(new Bullet(X + Width / 2, Y + Height / 2,
                        (float)Math.Cos(angle - spreadAngle) * bulletSpeed,
                        (float)Math.Sin(angle - spreadAngle) * bulletSpeed,
                        Damage, 10f)); // Damage parametresi eklendi
                }

                _attackTimer = 0;
            }
        }

        public void Update(float deltaTime)
        {
            Move(0, 0, deltaTime);
            Attack(0, 0);
        }
    }
}
