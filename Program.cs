using System.ComponentModel.Design.Serialization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Raylib_cs;

class Program
{
    static void Main()
    {
        // Define screen dimensions
        const int screenWidth = 1280;
        const int screenHeight = 800;
        int rotation = 0;
        List<FireBeam> fireBeams = new List<FireBeam>();

        // Initialize the window
        Raylib.InitWindow(screenWidth, screenHeight, "Simple Tank Movement");
        Raylib.SetTargetFPS(60);

        Image tankImage = Raylib.LoadImage("C:\\dev\\tankgame\\TankGame\\tank.png");

        Raylib.ImageResize(ref tankImage, (int)(tankImage.Width * 0.1), (int)(tankImage.Height * 0.1));

        Texture2D tankTexture = Raylib.LoadTextureFromImage(tankImage);

        Raylib.UnloadImage(tankImage);

        Rectangle sourceRec = new Rectangle(0, 0, tankTexture.Width, tankTexture.Height);

        Vector2 origin = new Vector2(tankTexture.Width / 2, tankTexture.Height / 2);

        // Tank properties
        Vector2 tankPosition = new Vector2(screenWidth / 2.0f, screenHeight / 2.0f);
        float tankSpeed = 4.0f;
        int tankSize = 40;
        int beamThickness = 4;
        int gunWidthOffset = 80;
        int gunHeigthOffset = 36;
        Vector2 gunPoistion = new Vector2(screenWidth / 2.0f + tankTexture.Width - gunWidthOffset, screenHeight / 2.0f + (tankTexture.Height - beamThickness) / 2 - gunHeigthOffset);

        // Main game loop
        while (!Raylib.WindowShouldClose())
        {
            // -------------------------------------------------------------
            // Input / Update
            // -------------------------------------------------------------
            
            // Basic 4-way movement
            if (Raylib.IsKeyDown(KeyboardKey.Up))
            {
                tankPosition.Y -= tankSpeed; // Move Up
                rotation = 270;
                gunPoistion = new Vector2(tankPosition.X + (tankTexture.Height - beamThickness) / 2 - gunHeigthOffset, tankPosition.Y - tankTexture.Width + gunWidthOffset);
            }
            if (Raylib.IsKeyDown(KeyboardKey.Down))
            {
                tankPosition.Y += tankSpeed; // Move Down
                rotation = 90;
                gunPoistion = new Vector2(tankPosition.X + (tankTexture.Height - beamThickness) / 2 - gunHeigthOffset, tankPosition.Y + tankTexture.Width - gunWidthOffset);
            }
            if (Raylib.IsKeyDown(KeyboardKey.Left))
            {
                tankPosition.X -= tankSpeed; // Move Left
                rotation = 180;
                gunPoistion = new Vector2(tankPosition.X - tankTexture.Width + gunWidthOffset, tankPosition.Y + (tankTexture.Height - beamThickness) / 2 - gunHeigthOffset);
            }
            if (Raylib.IsKeyDown(KeyboardKey.Right))
            {
                tankPosition.X += tankSpeed; // Move Right
                rotation = 0;
                gunPoistion = new Vector2(tankPosition.X + tankTexture.Width - gunWidthOffset, tankPosition.Y + (tankTexture.Height - beamThickness) / 2 - gunHeigthOffset);
            }
            if (Raylib.IsKeyDown(KeyboardKey.Space))
            {
                fireBeams.Add(new FireBeam(gunPoistion, rotation));
            }

            // Screen boundaries check (Keep tank inside the window)
            if (tankPosition.X < 0) tankPosition.X = 0;
            if (tankPosition.X > screenWidth - tankSize) tankPosition.X = screenWidth - tankSize;
            if (tankPosition.Y < 0) tankPosition.Y = 0;
            if (tankPosition.Y > screenHeight - tankSize) tankPosition.Y = screenHeight - tankSize;

            // -------------------------------------------------------------
            // Draw
            // -------------------------------------------------------------
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);

            Rectangle destRec = new Rectangle(tankPosition.X, tankPosition.Y, tankTexture.Width, tankTexture.Height);
            

            Raylib.DrawTexturePro(tankTexture, sourceRec, destRec, origin, rotation, Color.RayWhite);

            foreach(FireBeam beam in fireBeams)
            {
                beam.Draw();
                beam.move(20);
            }

            Raylib.EndDrawing();
        }

        // Close the window
        Raylib.CloseWindow();
    }
}

class FireBeam
{
    private Vector2 _start;
    private Vector2 _end;
    private int _rotation;
    private const int _length = 20;
    public FireBeam(Vector2 pos, int rotation)
    {
        _start = pos;
        if(rotation == 270)
        {
            _end = new Vector2(pos.X, pos.Y - _length);
        }
        if(rotation == 90)
        {
            _end = new Vector2(pos.X, pos.Y + _length);
        }
        if(rotation == 180)
        {
            _end = new Vector2(pos.X - _length, pos.Y);
        }
        if(rotation == 0)
        {
            _end = new Vector2(pos.X + _length, pos.Y);
        }
        _rotation = rotation;
    }
    public void move(int speed)
    {
        if(_rotation == 270)
        {
            _start = new Vector2(_start.X, _start.Y - speed);
            _end = new Vector2(_end.X, _end.Y - speed);
        }
        if(_rotation == 90)
        {
            _start = new Vector2(_start.X, _start.Y + speed);
            _end = new Vector2(_end.X, _end.Y + speed);
        }
        if(_rotation == 180)
        {
            _start = new Vector2(_start.X - _length, _start.Y);
            _end = new Vector2(_end.X - _length, _end.Y);
        }
        if(_rotation == 0)
        {
            _start = new Vector2(_start.X + _length, _start.Y);
            _end = new Vector2(_end.X + _length, _end.Y);
        }
    }
    public void Draw()
    {
        if(_end.X >= 0 && _end.X <= 1280 && _end.Y >= 0 && _end.Y <= 800)
        {
            Raylib.DrawLineEx(_start, _end, 4, Color.Red);
        }
    }
}

