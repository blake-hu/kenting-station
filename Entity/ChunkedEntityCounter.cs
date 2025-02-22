using System.Collections.Generic;
using System.Linq;
using System.Text;
using Godot;
using KentingStation.Common;

namespace KentingStation.Entity;

public class ChunkedEntityCounter<TEntity> where TEntity : Node2D
{
    private readonly Dictionary<ChunkCoordinate, uint> _cachedCounts = new();
    private readonly object _lock = new();

    public readonly uint ChunkSize;
    private Dictionary<EntityId, TEntity> _entityDict;
    private uint _timeSinceUpdate;

    public ChunkedEntityCounter(uint chunkSize)
    {
        ChunkSize = chunkSize;
    }

    public ChunkedEntityCounter(uint chunkSize, uint timeToLive)
    {
        ChunkSize = chunkSize;
        AutoUpdate = true;
        TimeToLive = timeToLive;
    }

    public bool AutoUpdate { get; set; }
    public uint TimeToLive { get; set; }

    public Dictionary<ChunkCoordinate, uint> CachedCounts
    {
        get
        {
            lock (_lock)
            {
                return _cachedCounts;
            }
        }
    }


    public void RegisterEntityDict(Dictionary<EntityId, TEntity> dict)
    {
        if (_entityDict is not null)
            throw new System.Exception(
                $"{nameof(ChunkedEntityCounter<TEntity>)}: Attempted to set non-empty {nameof(_entityDict)}. Entity dict should only be initialized once");
        _entityDict = dict;
    }

    public void Tick()
    {
        _timeSinceUpdate++;
        if (AutoUpdate && _timeSinceUpdate >= TimeToLive)
        {
            UpdateCounts();
            _timeSinceUpdate = 0;
        }
    }

    public void UpdateCounts()
    {
        lock (_lock)
        {
            _cachedCounts.Clear();
            foreach (var entity in _entityDict.Values)
            {
                var (chunkCoord, chunkSize) = Chunk.GetChunk(entity.Position, ChunkSize);
                var (chunkX, chunkY) = chunkCoord;
                _cachedCounts.TryAdd(chunkCoord, 0);
                _cachedCounts[new ChunkCoordinate(chunkX, chunkY)] += 1;
            }
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"---- ChunkedEntityCounter<{typeof(TEntity)}> ----");
        lock (_lock)
        {
            foreach (var kvp in _cachedCounts.OrderBy(kvp => kvp.Value).Reverse())
                sb.AppendLine($"- {kvp.Key}: {kvp.Value}");
        }

        return sb.ToString();
    }
}