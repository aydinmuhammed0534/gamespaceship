using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Models;

public class Game
{
    public Spaceship Player { get; private set; }
    public List<Enemy> Enemies { get; private set; }
    public List<PowerUp> PowerUps { get; private set; }
    public List<Obstacle> Obstacles { get; private set; }
    public bool IsGameOver { get; private set; }
    public int Score { get; private set; }
    public bool IsPaused { get; private set; }
    public int Level { get; private set; }
    public int HighScore { get; private set; }
    private int _highScore = 0;

    private float _enemySpawnTimer;
    private float _powerUpSpawnTimer;
    private float _obstacleSpawnTimer;
    private const float ENEMY_SPAWN_INTERVAL = 2f;
    private const float POWERUP_SPAWN_INTERVAL = 10f;
    private const float OBSTACLE_SPAWN_INTERVAL = 5f;
    private static readonly Random _random = new();

    public Game()
    {
        LoadHighScore();
        StartGame();
    }

    private void LoadHighScore()
    {
        string path = "highscore.txt";
        if (File.Exists(path))
        {
            string scoreStr = File.ReadAllText(path);
            if (int.TryParse(scoreStr, out int score))
            {
                HighScore = score;
                _highScore = score;
            }
        }
    }

    private void SaveHighScore()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
            _highScore = Score;
            File.WriteAllText("highscore.txt", Score.ToString());
        }
    }

    public void StartGame()
    {
        Player = new Spaceship(400, 500);
        Enemies = new List<Enemy>();
        PowerUps = new List<PowerUp>();
        Obstacles = new List<Obstacle>();
        IsGameOver = false;
        IsPaused = false;
        Score = 0;
        Level = 1;
        _enemySpawnTimer = 0;
        _powerUpSpawnTimer = 0;
        _obstacleSpawnTimer = 0;
    }

    public void Update(float deltaTime)
    {
        if (IsGameOver || IsPaused) return;

        UpdateTimers(deltaTime);
        UpdateEntities(deltaTime);
        int scoreToAdd = CheckCollisions();
        Score += scoreToAdd;
        UpdateLevel();

        if (!Player.IsActive)
        {
            EndGame();
        }
    }

    private void UpdateTimers(float deltaTime)
    {
        _enemySpawnTimer += deltaTime;
        _powerUpSpawnTimer += deltaTime;
        _obstacleSpawnTimer += deltaTime;

        if (_enemySpawnTimer >= GetEnemySpawnInterval())
        {
            SpawnEnemy();
            _enemySpawnTimer = 0;
        }

        if (_powerUpSpawnTimer >= POWERUP_SPAWN_INTERVAL)
        {
            SpawnPowerUp();
            _powerUpSpawnTimer = 0;
        }

        if (_obstacleSpawnTimer >= OBSTACLE_SPAWN_INTERVAL)
        {
            SpawnObstacle();
            _obstacleSpawnTimer = 0;
        }
    }

    private float GetEnemySpawnInterval()
    {
        return Math.Max(0.5f, ENEMY_SPAWN_INTERVAL - (Level * 0.1f));
    }

    private void UpdateEntities(float deltaTime)
    {
        Player.Update(deltaTime, this);

        for (int i = Enemies.Count - 1; i >= 0; i--)
        {
            Enemies[i].Update(deltaTime, this);
            if (!Enemies[i].IsActive)
            {
                Score += Enemies[i].ScoreValue;
                Enemies.RemoveAt(i);
            }
        }

        for (int i = PowerUps.Count - 1; i >= 0; i--)
        {
            PowerUps[i].Update(deltaTime, this);
            if (!PowerUps[i].IsActive)
            {
                PowerUps.RemoveAt(i);
            }
        }

        for (int i = Obstacles.Count - 1; i >= 0; i--)
        {
            Obstacles[i].Update(deltaTime, this);
            if (!Obstacles[i].IsActive)
            {
                Obstacles.RemoveAt(i);
            }
        }
    }

    private int CheckCollisions()
    {
        // Oyuncu mermilerinin düşmanlara çarpması
        foreach (var enemy in Enemies.ToList())
        {
            foreach (var bullet in Player.Bullets.ToList())
            {
                if (CollisionDetector.CheckCollision(bullet, enemy))
                {
                    enemy.TakeDamage(bullet.Damage);
                    bullet.IsActive = false;
                    if (!enemy.IsActive)
                    {
                        Score += enemy.ScoreValue;
                    }
                }
            }
        }

        // Düşman mermilerinin oyuncuya çarpması
        foreach (var enemy in Enemies)
        {
            foreach (var bullet in enemy.Bullets.ToList())
            {
                if (CollisionDetector.CheckCollision(bullet, Player))
                {
                    Player.TakeDamage(bullet.Damage);
                    bullet.IsActive = false;
                    if (!Player.IsActive)
                    {
                        EndGame();
                    }
                }
            }
        }

        // Düşmanların oyuncuya çarpması
        foreach (var enemy in Enemies)
        {
            if (CollisionDetector.CheckCollision(Player, enemy))
            {
                Player.TakeDamage(enemy.Damage * 0.5f);
                enemy.TakeDamage(Player.Damage * 0.5f);
                if (!Player.IsActive)
                {
                    EndGame();
                }
            }
        }
        return 0;
    }

    private void UpdateLevel()
    {
        int newLevel = 1 + (Score / 1000);
        if (newLevel > Level)
        {
            Level = newLevel;
            Player.Health = Math.Min(Player.Health + 20, Player.MaxHealth);
        }
    }

    private void SpawnEnemy()
    {
        float x = _random.Next(0, 800 - 30);
        float y = -30;

        Enemy enemy;
        int type = _random.Next(0, 100);

        enemy = type switch
        {
            < 40 => new BasicEnemy(x, y),
            < 65 => new FastEnemy(x, y),
            < 85 => new StrongEnemy(x, y),
            _ => new BossEnemy(x, y)
        };

        enemy.SetDifficultyMultiplier(1 + (Level * 0.1f));
        Enemies.Add(enemy);
    }

    private void SpawnPowerUp()
    {
        float x = _random.Next(0, 800 - 20);
        float y = -20;

        PowerUp powerUp = new PowerUp(x, y, (PowerUpType)_random.Next(0, 4));
        PowerUps.Add(powerUp);
    }

    private void SpawnObstacle()
    {
        float x = _random.Next(0, 800 - 40);
        float y = -40;
        int size = _random.Next(20, 41);

        Obstacle obstacle = new Obstacle(x, y, size, size);
        Obstacles.Add(obstacle);
    }

    public void EndGame()
    {
        IsGameOver = true;
        SaveHighScore();
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
    }

    public int GetHighScore()
    {
        return _highScore;
    }
}
