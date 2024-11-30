namespace WebApplication1.Models;

public class Spaceship
{
    public int Health { get; private set; }
    public int Damage { get; private set; }
    public int Speed { get; private set; }
    public List<Bullet> Bullets { get; private set; }
    public int X { get; private set; } // Uzay aracının X pozisyonu
    public int Y { get; private set; } // Uzay aracının Y pozisyonu
    public int Width { get; private set; } = 50; // Uzay aracının genişliği
    public int Height { get; private set; } = 50; // Uzay aracının yüksekliği
    public object SpawnX { get; set; }
    public object SpawnY { get; set; }

    public Spaceship(int health, int damage, int speed)
    {
        Health = health;
        Damage = damage;
        Speed = speed;
        Bullets = new List<Bullet>();
        X = 100; // Başlangıç X pozisyonu
        Y = 400; // Başlangıç Y pozisyonu
    }

    public void Move(string direction)
    {
        // Hareket yönüne göre pozisyonu güncelle
        switch (direction.ToLower())
        {
            case "up":
                Y -= Speed;
                break;
            case "down":
                Y += Speed;
                break;
            case "left":
                X -= Speed;
                break;
            case "right":
                X += Speed;
                break;
        }

        // Sınırları aşmamasını sağla (örneğin ekran sınırları)
        X = Math.Clamp(X, 0, 800); // 800, ekran genişliği sınırı
        Y = Math.Clamp(Y, 0, 600); // 600, ekran yüksekliği sınırı
    }

    public void Shoot()
    {
        // Yeni bir mermi oluştur ve listeye ekle
        // direction'ı 1 olarak sabitliyoruz (yukarıya hareket)
        var newBullet = new Bullet(x: X + Width / 2, y: Y, damage: Damage, speed: 10, direction: 1);
        Bullets.Add(newBullet);
    }


    public void TakeDamage(int amount)
    {
        // Hasarı uygula ve can seviyesini güncelle
        Health -= amount;
        if (Health < 0)
            Health = 0; // Can seviyesi negatif olmamalı
    }
}