using System;

namespace WebApplication1.Models
{
    public class Bullet : GameObject
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Speed { get; set; }
        public float Damage { get; set; }
        public bool IsActive { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public bool IsEnemyBullet { get; set; }
        public float Width { get; set; } = 5;
        public float Height { get; set; } = 5;

        public Bullet(float startX, float startY) : base(startX, startY)
        {
            X = startX;
            Y = startY;
            IsActive = true;
            Speed = 300;
            Damage = 10;
        }

        public override void Update(float deltaTime, Game game)
        {
            X += VelocityX * Speed * deltaTime;
            Y += VelocityY * Speed * deltaTime;

            // Ekran dışına çıktıysa mermiyi yok et
            if (X < 0 || X > 800 || Y < 0 || Y > 600)
            {
                IsActive = false;
            }
        }

        public void Destroy()
        {
            IsActive = false;
        }

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
}