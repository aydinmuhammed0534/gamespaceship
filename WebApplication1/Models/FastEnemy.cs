namespace WebApplication1.Models;
public class FastEnemy : Enemy
{
    public FastEnemy() : base(health: 30, speed: 5, damage: 3, spawnX: 200, spawnY: 0, scoreValue: 15)
    {
    }

    public override void Move()
    {
        // Daha hızlı hareket algoritması
        SpawnY += Speed;
    }

    public override void Attack()
    {
        // Daha az hasar veren mermiler
        Bullets.Add(new Bullet(x: SpawnX + Width / 2, y: SpawnY, damage: Damage, speed: 7,direction:1));
    }
}
