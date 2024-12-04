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
        private float _shootTimer = 0;
        private const float SHOOT_INTERVAL = 2.5f; // Boss her 2.5 saniyede bir ateş eder
        private int _attackPattern = 0; // Farklı saldırı desenleri için

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

            _shootTimer += deltaTime;
            if (_shootTimer >= SHOOT_INTERVAL)
            {
                // Boss farklı ateş desenleri kullanır
                switch (_attackPattern)
                {
                    case 0:
                        ShootCircular(); // Dairesel ateş
                        break;
                    case 1:
                        ShootSpread(playerX, playerY); // Yelpaze şeklinde ateş
                        break;
                    case 2:
                        ShootBurst(playerX, playerY); // Hızlı art arda ateş
                        break;
                }
                _attackPattern = (_attackPattern + 1) % 3; // Saldırı desenini değiştir
                _shootTimer = 0;
            }
        }

        protected override void Shoot(float playerX, float playerY)
        {
            float directionX = playerX - X;
            float directionY = playerY - Y;
            float length = (float)Math.Sqrt(directionX * directionX + directionY * directionY);
            directionX /= length;
            directionY /= length;

            // Boss düşman daha güçlü ve hızlı mermiler atar
            var bullet = new Bullet(X, Y)
            {
                VelocityX = directionX * 7f,
                VelocityY = directionY * 7f,
                Damage = this.Damage * 0.8f,
                IsEnemyBullet = true
            };
            Bullets.Add(bullet);

            // Öfkeli durumda ek mermiler
            if (_isEnraged)
            {
                // Sol mermi
                var leftBullet = new Bullet(X, Y)
                {
                    VelocityX = (directionX * 6f) - 0.5f,
                    VelocityY = (directionY * 6f) - 0.5f,
                    Damage = this.Damage * 0.6f,
                    IsEnemyBullet = true
                };
                Bullets.Add(leftBullet);

                // Sağ mermi
                var rightBullet = new Bullet(X, Y)
                {
                    VelocityX = (directionX * 6f) + 0.5f,
                    VelocityY = (directionY * 6f) + 0.5f,
                    Damage = this.Damage * 0.6f,
                    IsEnemyBullet = true
                };
                Bullets.Add(rightBullet);
            }
        }

        private void ShootCircular()
        {
            int bulletCount = 8; // Daire etrafında 8 mermi
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = (float)(i * 2 * Math.PI / bulletCount);
                float dirX = (float)Math.Cos(angle);
                float dirY = (float)Math.Sin(angle);
                
                var bullet = new Bullet(X, Y)
                {
                    VelocityX = dirX * 6f,
                    VelocityY = dirY * 6f,
                    Damage = this.Damage * 0.8f,
                    IsEnemyBullet = true
                };
                Bullets.Add(bullet);
            }
        }

        private void ShootSpread(float playerX, float playerY)
        {
            int bulletCount = 5; // Yelpaze şeklinde 5 mermi
            float spreadAngle = 60f; // 60 derece yayılım
            float startAngle = -spreadAngle / 2;
            float angleStep = spreadAngle / (bulletCount - 1);

            float baseAngle = (float)Math.Atan2(playerY - Y, playerX - X);

            for (int i = 0; i < bulletCount; i++)
            {
                float currentAngle = baseAngle + (startAngle + i * angleStep) * (float)Math.PI / 180f;
                float dirX = (float)Math.Cos(currentAngle);
                float dirY = (float)Math.Sin(currentAngle);

                var bullet = new Bullet(X, Y)
                {
                    VelocityX = dirX * 7f,
                    VelocityY = dirY * 7f,
                    Damage = this.Damage * 0.6f,
                    IsEnemyBullet = true
                };
                Bullets.Add(bullet);
            }
        }

        private void ShootBurst(float playerX, float playerY)
        {
            // Hızlı art arda 3 mermi
            float dirX = playerX - X;
            float dirY = playerY - Y;
            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            dirX /= length;
            dirY /= length;

            for (int i = 0; i < 3; i++)
            {
                var bullet = new Bullet(X, Y)
                {
                    VelocityX = dirX * (8f + i), // Her mermi biraz daha hızlı
                    VelocityY = dirY * (8f + i),
                    Damage = this.Damage * 0.9f,
                    IsEnemyBullet = true
                };
                Bullets.Add(bullet);
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
                        var bullet = new Bullet(X + Width / 2, Y + Height)
                        {
                            VelocityX = (float)Math.Cos(angle) * bulletSpeed,
                            VelocityY = (float)Math.Sin(angle) * bulletSpeed,
                            Damage = this.Damage
                        };
                        Bullets.Add(bullet);
                    }
                }
                else
                {
                    // Normal durumda üçlü atış
                    float bulletSpeed = 250f;
                    float angle = (float)Math.Atan2(playerY - Y, playerX - X);
                    float spreadAngle = 0.2f;

                    // Merkez atış
                    var bullet = new Bullet(X + Width / 2, Y + Height)
                    {
                        VelocityX = (float)Math.Cos(angle) * bulletSpeed,
                        VelocityY = (float)Math.Sin(angle) * bulletSpeed,
                        Damage = this.Damage
                    };
                    Bullets.Add(bullet);

                    // Yan atışlar
                    var bullet2 = new Bullet(X + Width / 2, Y + Height)
                    {
                        VelocityX = (float)Math.Cos(angle + spreadAngle) * bulletSpeed,
                        VelocityY = (float)Math.Sin(angle + spreadAngle) * bulletSpeed,
                        Damage = this.Damage
                    };
                    Bullets.Add(bullet2);
                    var bullet3 = new Bullet(X + Width / 2, Y + Height)
                    {
                        VelocityX = (float)Math.Cos(angle - spreadAngle) * bulletSpeed,
                        VelocityY = (float)Math.Sin(angle - spreadAngle) * bulletSpeed,
                        Damage = this.Damage
                    };
                    Bullets.Add(bullet3);
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
