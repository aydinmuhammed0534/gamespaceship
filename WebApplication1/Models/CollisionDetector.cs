using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Models
{
    public static class CollisionDetector
    {
        public static int CheckCollisions(Game game)
        {
            int scoreToAdd = 0;

            if (game.IsPaused) return scoreToAdd;

            // Check player bullet collisions with enemies
            foreach (var bullet in game.Player.Bullets.ToList())
            {
                if (bullet != null)
                {
                    if (bullet.IsActive)
                    {
                        foreach (var enemy in game.Enemies.ToList())
                        {
                            if (enemy != null && enemy.IsActive)
                            {
                                if (bullet.GetBounds().Intersects(enemy.GetBounds()))
                                {
                                    enemy.TakeDamage(bullet.Damage);
                                    bullet.IsActive = false;

                                    if (enemy != null && !enemy.IsActive)
                                    {
                                        scoreToAdd += enemy.ScoreValue;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            // Check enemy bullet collisions with player
            foreach (var enemy in game.Enemies)
            {
                if (enemy != null)
                {
                    foreach (var bullet in enemy.Bullets.ToList())
                    {
                        if (bullet != null)
                        {
                            if (bullet.IsActive)
                            {
                                if (bullet.GetBounds().Intersects(game.Player.GetBounds()))
                                {
                                    game.Player.TakeDamage(bullet.Damage);
                                    bullet.IsActive = false;
                                }
                            }
                        }
                    }
                }
            }

            // Check player collision with power-ups
            foreach (var powerUp in game.PowerUps.ToList())
            {
                if (powerUp != null)
                {
                    if (powerUp.IsActive)
                    {
                        if (game.Player.GetBounds().Intersects(powerUp.GetBounds()))
                        {
                            game.Player.ApplyPowerUp(powerUp.Type, powerUp.Value, powerUp.Duration);
                            powerUp.IsActive = false;
                        }
                    }
                }
            }

            // Check player collision with obstacles
            foreach (var obstacle in game.Obstacles)
            {
                if (obstacle != null)
                {
                    if (obstacle.IsActive)
                    {
                        if (game.Player.GetBounds().Intersects(obstacle.GetBounds()))
                        {
                            game.Player.TakeDamage(obstacle.Damage);
                        }
                    }
                }
            }

            // Check enemy collisions with obstacles
            foreach (var enemy in game.Enemies.ToList())
            {
                if (enemy != null)
                {
                    if (enemy.IsActive)
                    {
                        foreach (var obstacle in game.Obstacles)
                        {
                            if (obstacle != null)
                            {
                                if (obstacle.IsActive)
                                {
                                    if (enemy.GetBounds().Intersects(obstacle.GetBounds()))
                                    {
                                        enemy.TakeDamage(obstacle.Damage);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return scoreToAdd;
        }
    }
}
