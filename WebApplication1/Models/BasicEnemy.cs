namespace WebApplication1.Models;

public class BasicEnemy : Enemy
{
    public BasicEnemy() : base(health: 50, speed: 2, damage: 5, spawnX: 100, spawnY: 0, scoreValue: 10)
    {
    }

    public override void Move()
    {
        // Düşmanın basit şekilde aşağıya hareket etmesi
        SpawnY += Speed;
    }

    public override void Attack()
    {
        // Basit bir mermi ateşleme
        Bullets.Add(new Bullet(x: SpawnX + Width / 2, y: SpawnY, damage: Damage, speed: 5,direction:1));
    }
}
