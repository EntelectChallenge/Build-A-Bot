namespace BuildABot.AI.DataStructures.Spatial
{
    struct BoundingBox
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public readonly int Top => Y + Height;
        public readonly int Bottom => Y;
        public readonly int Left => X;
        public readonly int Right => X + Width;

        public readonly Point Position => new Point(X, Y);
        public readonly Point Size => new Point(Width, Height);

        public BoundingBox(int _X, int _Y, int _Width, int _Height)
        {
            X = _X;
            Y = _Y;
            Width = _Width;
            Height = _Height;
        }
    }
}
