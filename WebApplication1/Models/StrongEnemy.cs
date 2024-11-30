namespace WebApplication1.Models;

public class StrongEnemy : Enemy
{
    public StrongEnemy() : base(health: 100, speed: 1, damage: 10, spawnX: 300, spawnY: 0, scoreValue: 20)
    {
    }

    public override void Move()
    {
        // Yavaş ancak dayanıklı hareket
        SpawnY += Speed;
    }

    public override void Attack()
    {
        // Güçlü bir mermi ateşleme
        Bullets.Add(new Bullet(x: SpawnX + Width / 2, y: SpawnY, damage: Damage, speed: 3 ,direction:1));
    }
}
