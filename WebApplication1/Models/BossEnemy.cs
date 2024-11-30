namespace WebApplication1.Models;

public class BossEnemy : Enemy
{
    public BossEnemy() : base(health: 300, speed: 1, damage: 20, spawnX: 400, spawnY: 0, scoreValue: 50)
    {
    }

    public override void Move()
    {
        // Zikzak hareket algoritması
        SpawnX += Speed;
        SpawnY += Speed / 2;
    }

    public override void Attack()
    {
        // Daha fazla mermi ateşleme
        for (int i = -1; i <= 1; i++)
        {
            Bullets.Add(new Bullet(x: SpawnX + (Width / 2) + (i * 10), y: SpawnY, damage: Damage, speed: 4,direction: 1));
        }
    }
}

