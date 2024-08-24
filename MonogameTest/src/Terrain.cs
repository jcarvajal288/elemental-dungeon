using System.Collections.Generic;

namespace MonogameTest;

public enum Terrain {
    OrcWall,
    OrcFloor,
    CrystalWallLightBlue
}

public static class TerrainExtensions {
    public static readonly List<Terrain> WalkableTerrain = [
        Terrain.OrcFloor
    ];
}
    
