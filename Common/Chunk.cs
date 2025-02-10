using System;
using Godot;

namespace CozyGame.Common;

public record struct ChunkCoordinate(int X, int Y)
{
    public override string ToString()
    {
        return $"Chunk({X}, {Y})";
    }
}

public record struct Chunk(ChunkCoordinate Coord, uint Size)
{
    public static Chunk GetChunk(Vector2 position, uint chunkSize)
    {
        var chunkX = (int)Math.Floor(position.X / chunkSize);
        var chunkY = (int)Math.Floor(position.Y / chunkSize);
        var chunkCoord = new ChunkCoordinate(chunkX, chunkY);
        return new Chunk(chunkCoord, chunkSize);
    }

    public Rect2 Boundary()
    {
        return new Rect2(new Vector2(Coord.X, Coord.Y) * Size, new Vector2(Size, Size));
    }

    public override string ToString()
    {
        return $"Chunk({Coord.X}, {Coord.Y}, Size {Size})";
    }
}