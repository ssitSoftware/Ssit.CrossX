using System;
using System.Numerics;
using Ssit.CrossX;

namespace SampleGame.Game;

internal class Entity
{
    private static readonly Random Random = new();
    
    public Vector2 Position;
    
    public Vector2 Direction;
    public float Speed;

    public RgbaColor Color;
    
    public Entity()
    {
        Position = new Vector2((float)Random.NextDouble() * 1000, (float)Random.NextDouble() * 1000);
        Color = RgbaColor.FromInt32((int) (Random.Next() | 0xff000000));
        Direction = new Vector2(Random.Next(2) == 0 ? 1 : -1, Random.Next(2) == 0 ? 1 : -1);

        Speed = 200 + (float)Random.NextDouble() * 1000;
    }
    
    public void Update(float dt, Size size)
    {
        
        Position += Direction * dt * Speed;

        var maxX = size.Width - 50;
        var maxY = size.Height - 50;
    
        var minX = 50;
        var minY = 50;
    
        if ( Position.X < minX )
        {
            Position.X = minY + (minY - Position.X);
            Direction.X = 1;
        }
    
        if ( Position.Y < minY )
        {
            Position.Y = minY + (minY - Position.Y);
            Direction.Y = 1;
        }
    
        if ( Position.X > maxX )
        {
            Position.X = maxX - (Position.X - maxX);
            Direction.X = -1;
        }
    
        if ( Position.Y > maxY )
        {
            Position.Y = maxY - (Position.Y - maxY);
            Direction.Y = -1;
        }
    }
}