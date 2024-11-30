namespace WebApplication1.Models
{
    public class CollisionDetector
    {
        // Oyuncu uzay aracı ve düşman arasındaki çarpışmayı kontrol eder
        public bool CheckCollision(Spaceship player, Enemy enemy)
        {
            // Çarpışma kontrolü - X ve Y koordinatları ile boyutları karşılaştır
            if (player.X < enemy.SpawnX + enemy.Width &&
                player.X + player.Width > enemy.SpawnX &&
                player.Y < enemy.SpawnY + enemy.Height &&
                player.Y + player.Height > enemy.SpawnY)
            {
                return true; // Çarpışma varsa true döndür
            }
            return false; // Çarpışma yoksa false döndür
        }

        // Mermilerin düşmanlara çarpıp çarpmadığını kontrol eder
        public void CheckBulletCollision(List<Bullet> bullets, List<Enemy> enemies)
        {
            foreach (var bullet in bullets)
            {
                foreach (var enemy in enemies)
                {
                    // Mermi ve düşman arasındaki çarpışmayı kontrol et
                    if (bullet.X < enemy.SpawnX + enemy.Width &&
                        bullet.X + bullet.Width > enemy.SpawnX &&
                        bullet.Y < enemy.SpawnY + enemy.Height &&
                        bullet.Y + bullet.Height > enemy.SpawnY)
                    {
                        // Çarpışma gerçekleştiğinde mermi ve düşmanı yok et
                        bullet.OnHit(enemy);  // Mermi düşmana hasar verir
                        enemy.TakeDamage(bullet.Damage); // Düşman hasar alır

                        if (enemy.Health <= 0)
                        {
                            // Düşman öldüyse yok et
                            enemy.Destroy();
                        }

                        bullet.Destroy(); // Mermi yok edilir
                    }
                }
            }
        }
    }
}