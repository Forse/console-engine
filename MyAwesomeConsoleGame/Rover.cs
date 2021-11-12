﻿using System.Collections.Generic;
using ConsoleEngine.Infrastructure;
using ConsoleEngine.Infrastructure.Rendering;
using Microsoft.Xna.Framework;

namespace MyAwesomeConsoleGame
{
    public class Rover : GameObject
    {
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public const float Thrust = 0.8f;
        public const float Drag = 20f;
        public const float MaxPower = 1000;
        public double RemainingPower;
        public int DamageTaken = 0;
        
        
        public static readonly Sprite Sprite = Sprite.FromStringArray(new[]
        {
            "|#|",
            " # ",
            "|#|",
        });

        public Rover(MyAwesomeGame game) : base(game)
        {
            RemainingPower = MaxPower;
        }

        public void Update()
        {
            var prevPosition = Position;
            
            Velocity += -Drag * Velocity * (float)GameTime.Delta.TotalSeconds;
            Velocity += Acceleration * (float)GameTime.Delta.TotalSeconds;
            Position += Velocity;

            Game.Console.Draw(0, 3, $"NPos : {Position}");
            
            if (Game.World.Intersects(this, out var with))
            {
                Position = prevPosition ;
                Velocity = Vector2.Zero;
            }
            
            Acceleration = Vector2.Zero;
        }
        
        public void Draw()
        {
            var screenPos = GetScreenPos();
            
            Game.Console.Draw(
                (int)screenPos.X - 1, 
                (int)screenPos.Y - 1, 
                Sprite);
        }

        public void MoveNorth() => ApplyForce(new Vector2(0, -1f) * Thrust);
        public void MoveSouth() => ApplyForce(new Vector2(0, 1f) * Thrust);
        public void MoveWest() => ApplyForce(new Vector2(-1f, 0) * Thrust);
        public void MoveEast() => ApplyForce(new Vector2(1f, 0) * Thrust);


        public void ApplyForce(Vector2 force)
        {
            RemainingPower -= force.LengthSquared();
            Acceleration += force;
        }
    }
}