namespace WebApplication1.Models;

public abstract class Enemy
{
    public int Health { get; protected set; }
    public int Speed { get; protected set; }
    public int Damage { get; protected set; }
    public int SpawnX { get;  set; }
    public int SpawnY { get; set; }
    public Spaceship Player { get; private set; }

    public int Width { get; protected set; } = 50; // Düşmanın genişliği
    public int Height { get; protected set; } = 50; // Düşmanın yüksekliği
    public List<Bullet> Bullets { get; private set; }

    public int ScoreValue { get; protected set; } // Yok edilince kazandırdığı skor

    protected Enemy(int health, int speed, int damage, int spawnX, int spawnY, int scoreValue)
    {
        Health = health;
        Speed = speed;
        Damage = damage;
        SpawnX = spawnX;
        SpawnY = spawnY;
        ScoreValue = scoreValue;
        Bullets = new List<Bullet>();
    }

    public abstract void Move(); // Düşmanın hareket algoritması
    public abstract void Attack(); // Düşmanın saldırı davranışı

    public void TakeDamage(int amount)
    {
        // Hasarı uygula ve can seviyesini güncelle
        Health -= amount;
        if (Health <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        // Düşmanın yok edilme işlemleri
        // (örneğin, skor artırma veya animasyon tetikleme)
        Console.WriteLine("Enemy destroyed!");
    }
}