using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebApplication1.Models
{
    public class Game
    {
        public Spaceship? Player { get; private set; }
        public List<Enemy> Enemies { get; private set; }
        public bool IsGameOver { get; private set; }
        public int Score { get; private set; }

        private Random random;

        public Game()
        {
            Enemies = new List<Enemy>();
            Player = new Spaceship(health: 100, damage: 10, speed: 5);
            Score = 0;
            IsGameOver = false;
            random = new Random();
        }

        public void StartGame()
        {
            // Düşmanları oluştur
            GenerateEnemies();
        }

        private void GenerateEnemies()
        {
            for (int i = 0; i < 5; i++)
            {
                Enemies.Add(EnemyFactory.CreateRandomEnemy(random));
            }
        }

        public void UpdateGame()
        {
            if (IsGameOver) return;

            UpdatePlayerMovements();
            UpdateEnemyMovements();
            CheckCollisions();
            CheckGameOver();
        }

        private void UpdatePlayerMovements()
        {
            if (Player == null) return;
            
            foreach (var bullet in Player.Bullets.ToList())
            {
                bullet.Move();
            }
        }

        private void UpdateEnemyMovements()
        {
            foreach (var enemy in Enemies.ToList())
            {
                enemy.Move();

                // Düşmanın ateş etme olasılığı
                if (random.Next(100) < 10)
                {
                    enemy.Attack();
                }
            }
        }

        public void CheckCollisions()
        {
            CheckBulletCollisions();
            CheckSpaceshipCollisions();
        }

        private void CheckBulletCollisions()
        {
            if (Player == null) return;

            foreach (var bullet in Player.Bullets.ToList())
            {
                var hitEnemies = Enemies.Where(e => IsColliding(bullet, e)).ToList();
                foreach (var enemy in hitEnemies)
                {
                    enemy.TakeDamage(bullet.Damage);
                    Player.Bullets.Remove(bullet);

                    if (enemy.Health <= 0)
                    {
                        Score += enemy.ScoreValue;
                        Enemies.Remove(enemy);
                    }
                }
            }

            foreach (var enemy in Enemies)
            {
                foreach (var bullet in enemy.Bullets.ToList())
                {
                    if (IsColliding(bullet, Player))
                    {
                        Player.TakeDamage(bullet.Damage);
                        enemy.Bullets.Remove(bullet);
                    }
                }
            }
        }

        private bool IsColliding(Bullet bullet, Enemy enemy)
        {
            return bullet.X < enemy.SpawnX + enemy.Width &&
                   bullet.X + bullet.Width > enemy.SpawnX &&
                   bullet.Y < enemy.SpawnY + enemy.Height &&
                   bullet.Y + bullet.Height > enemy.SpawnY;
        }

        private bool IsColliding(Spaceship player, Enemy enemy)
        {
            return player.X < enemy.SpawnX + enemy.Width &&
                   player.X + player.Width > enemy.SpawnX &&
                   player.Y < enemy.SpawnY+ enemy.Height &&
                   player.Y + player.Height > enemy.SpawnY;
        }

        private void CheckSpaceshipCollisions()
        {
            if (Player == null) return;

            foreach (var enemy in Enemies.ToList())
            {
                if (IsColliding(Player, enemy))
                {
                    Player.TakeDamage(enemy.Damage);
                    enemy.TakeDamage(Player.Damage);

                    if (enemy.Health <= 0)
                    {
                        Score += enemy.ScoreValue;
                        Enemies.Remove(enemy);
                    }
                }
            }
        }

        public void EndGame()
        {
            IsGameOver = true;
            SaveScore();
        }

        private void SaveScore()
        {
            string scoreFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "SpacewarScores.txt"
            );

            using (StreamWriter writer = File.AppendText(scoreFilePath))
            {
                writer.WriteLine($"{DateTime.Now}: {Score}");
            }
        }

        private void CheckGameOver()
        {
            if (Player == null || Player.Health <= 0 || !Enemies.Any())
            {
                EndGame();
            }
        }

        private bool IsColliding(Bullet bullet, Spaceship player)
        {
            return bullet.X < player.X + player.Width &&
                   bullet.X + bullet.Width > player.X &&
                   bullet.Y < player.Y + player.Height &&
                   bullet.Y + bullet.Height > player.Y;
        }
    }

    // Düşman türlerini oluşturmak için fabrika
    public static class EnemyFactory
    {
        public static Enemy CreateRandomEnemy(Random random)
        {
            switch (random.Next(4))
            {
                case 0: return new BasicEnemy();
                case 1: return new FastEnemy();
                case 2: return new StrongEnemy();
                case 3: return new BossEnemy();
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
