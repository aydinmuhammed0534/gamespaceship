using.WebApplication1.Models.Spaceship
public class Spaceship
{
    public int Health { get; private set; }
    public int Damage { get; private set; }
    public int Speed { get; private set; }
    public int PosX { get; private set; } = 250; // Başlangıç X pozisyonu
    public int PosY { get; private set; } = 250; // Başlangıç Y pozisyonu

    public Spaceship(int health, int damage, int speed)
    {
        Health = health;
        Damage = damage;
        Speed = speed;
    }

    public void Move(string direction)
    {
        switch (direction.ToLower())
        {
            case "up":
                PosY = Math.Max(0, PosY - Speed);
                break;
            case "down":
                PosY = Math.Min(500, PosY + Speed);
                break;
            case "left":
                PosX = Math.Max(0, PosX - Speed);
                break;
            case "right":
                PosX = Math.Min(500, PosX + Speed);
                break;
            default:
                throw new ArgumentException("Geçersiz hareket yönü!");
        }
    }

    public void Shoot()
    {
        // Ateş etme mantığı
    }

    public void TakeDamage(int amount)
    {
        if (amount < 0)
            throw new ArgumentException("Hasar miktarı negatif olamaz!");
            
        Health -= amount;
        if (Health < 0) Health = 0;
    }
} 