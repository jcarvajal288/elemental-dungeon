using System.Collections.Generic;

namespace MonogameTest;

public enum Terrain {
    OrcWall,
    OrcFloor,
    BrownBrickWall,
    LimestoneFloor,
    BrownStoneWall,
    CrystalWallLightBlue,
    CrystalWallLightRed,
}

public static class TerrainExtensions {
    public static readonly List<Terrain> WalkableTerrain = [
        Terrain.OrcFloor,
        Terrain.LimestoneFloor,
    ];
}
    
