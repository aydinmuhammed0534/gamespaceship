using System;
using WebApplication1.Models;

namespace WebApplication1.Utils
{
    public static class CollisionDetector
    {
        public static bool CheckCollision(float x1, float y1, float width1, float height1,
                                        float x2, float y2, float width2, float height2)
        {
            return (x1 < x2 + width2 &&
                    x1 + width1 > x2 &&
                    y1 < y2 + height2 &&
                    y1 + height1 > y2);
        }

        public static bool BulletHitsEnemy(Bullet bullet, Enemy enemy)
        {
            return CheckCollision(bullet.X, bullet.Y, 5, 10,  // Mermi boyutları
                                enemy.X, enemy.Y, 40, 40);    // Düşman boyutları
        }

        public static bool BulletHitsSpaceship(Bullet bullet, Spaceship spaceship)
        {
            return CheckCollision(bullet.X, bullet.Y, 5, 10,  // Mermi boyutları
                                spaceship.X, spaceship.Y, 50, 50); // Uzay gemisi boyutları
        }

        public static bool EnemyHitsSpaceship(Enemy enemy, Spaceship spaceship)
        {
            return CheckCollision(enemy.X, enemy.Y, 40, 40,   // Düşman boyutları
                                spaceship.X, spaceship.Y, 50, 50); // Uzay gemisi boyutları
        }
    }
}
