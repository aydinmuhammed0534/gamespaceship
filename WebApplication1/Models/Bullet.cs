namespace WebApplication1.Models;

public class Bullet:Game
{
    public int Speed { get; private set; } // Merminin hızı
    public int Damage { get; private set; } // Merminin verdiği hasar
    public int Direction { get; private set; } // Hareket yönü (1: yukarı, -1: aşağı)
    public int X { get; private set; } // Merminin yatay eksendeki konumu
    public int Y { get; private set; } // Merminin dikey eksendeki konumu
    public int Width { get; private set; } = 10; // Merminin genişliği
    public int Height { get; private set; } = 20; // Merminin yüksekliği

    public Bullet(int x, int y, int damage, int speed, int direction)
    {
        X = x;
        Y = y;
        Damage = damage;
        Speed = speed;
        Direction = direction; // 1: Yukarı (oyuncu), -1: Aşağı (düşman)
    }




    /// <summary>
    /// Merminin ekranda ilerlemesini sağlar.
    /// </summary>
    public void Move()
    {
        Y += Speed * Direction;
    }
 


    /// <summary>
    /// Mermi bir hedefe çarptığında hasarı uygular ve yok edilir.
    /// </summary>
    /// <param name="target">Hedef (oyuncu veya düşman)</param>
    public void OnHit(Enemy target)
    {
        target.TakeDamage(Damage); // Hedefe hasar uygula
        Destroy(); // Mermiyi yok et
    }

    /// <summary>
    /// Mermi yok edildiğinde yapılacak işlemler.
    /// </summary>
    public void Destroy()
    {
        Console.WriteLine("Bullet destroyed!");
        // Yok edilme işlemleri (örn. görsel efektler)
    }
}