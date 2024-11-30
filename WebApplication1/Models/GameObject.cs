namespace WebApplication1.Models
{
    // Temel sınıf: GameObject
    public abstract class GameObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public GameObject(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        // Ortak davranışlar
        public abstract void Move(); // Bu metodu her nesne için özelleştirebilirsiniz
    }
}