using System;

namespace WebApplication1.Models
{
    public class Bullet
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Speed { get; private set; }
        public float Damage { get; private set; }
        public bool IsActive { get; set; }
        public float DirectionX { get; private set; }
        public float DirectionY { get; private set; }

        public Bullet(float startX, float startY, float directionX, float directionY, float speed, float damage)
        {
            X = startX;
            Y = startY;
            DirectionX = directionX;
            DirectionY = directionY;
            Speed = speed;
            Damage = damage;
            IsActive = true;
        }

        public void Update(float deltaTime, Game game)
        {
            X += DirectionX * Speed * deltaTime;
            Y += DirectionY * Speed * deltaTime;

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
                Width = 5,
                Height = 5
            };
        }
    }
}