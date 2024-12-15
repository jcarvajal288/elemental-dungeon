using System.Collections.Generic;

namespace MonogameTest.map;

public enum Terrain {
    BrownBrickWall,
    BrownStoneWall,
    CrystalWallDarkGray,
    CrystalWallLightBlue,
    CrystalWallLightGreen,
    CrystalWallLightRed,
    CrystalWallWhite,
    MosaicFloor,
    LimestoneFloor,
    OrcFloor,
    OrcWall,
    ReliefWall
}

public static class TerrainExtensions {
    public static readonly List<Terrain> WalkableTerrain = [
        Terrain.OrcFloor,
        Terrain.MosaicFloor,
        Terrain.LimestoneFloor,
    ];

    public static readonly List<Terrain> DiggableTerrain = [
        Terrain.OrcWall,
        Terrain.CrystalWallDarkGray,
        Terrain.CrystalWallLightBlue,
        Terrain.CrystalWallLightGreen,
        Terrain.CrystalWallLightRed,
        Terrain.CrystalWallWhite
    ];
}
    
