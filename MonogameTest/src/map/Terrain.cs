using System.Collections.Generic;

namespace MonogameTest;

public enum Terrain {
    BrownBrickWall,
    BrownStoneWall,
    CobbleStoneFloor,
    CrystalWallLightBlue,
    CrystalWallLightRed,
    GreyBrickWall,
    LimestoneFloor,
    OrcFloor,
    OrcWall,
}

public static class TerrainExtensions {
    public static readonly List<Terrain> WalkableTerrain = [
        Terrain.OrcFloor,
        Terrain.LimestoneFloor,
        Terrain.CobbleStoneFloor,
    ];
}
    
