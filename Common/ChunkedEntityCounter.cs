using System;
using System.Collections.Generic;

namespace CozyGame.Common;

public class ChunkedEntityCounter<T>
{
    private Dictionary<EntityId, T> _entityDict;

    public ChunkedEntityCounter(uint numChunks)
    {
        CachedCounts = new uint[numChunks, numChunks];
    }

    public uint[,] CachedCounts { get; }


    public void RegisterEntityDict(Dictionary<EntityId, T> dict)
    {
        _entityDict = dict;
    }

    public void RefreshCounts()
    {
        throw new NotImplementedException();
    }

    public void GetChunkBoundaries(Chunk chunk)
    {
        throw new NotImplementedException();
    }
}