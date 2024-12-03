namespace WebApplication1.Models;

public abstract class GameObject
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float Speed { get; set; }
    public bool IsActive { get; set; }
    public string? SpriteUrl { get; set; }
    public float MaxHealth { get; protected set; }

    protected GameObject(float x, float y)
    {
        X = x;
        Y = y;
        IsActive = true;
        MaxHealth = 100;
    }

    public abstract void Update(float deltaTime, Game game);

    public Rectangle GetBounds()
    {
        return new Rectangle
        {
            X = X,
            Y = Y,
            Width = Width,
            Height = Height
        };
    }
}

public class Rectangle
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }

    public bool Intersects(Rectangle other)
    {
        return !(X + Width < other.X ||
                other.X + other.Width < X ||
                Y + Height < other.Y ||
                other.Y + other.Height < Y);
    }
}