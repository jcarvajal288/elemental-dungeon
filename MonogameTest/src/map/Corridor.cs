using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.map;

public class Corridor {
    protected Vector2 TopLeft;
    protected Vector2 BottomRight;
    protected Terrain FloorTerrain = Terrain.MosaicFloor;
    protected Terrain WallTerrain = Terrain.ReliefWall;
    
    protected Dictionary<Vector2, Tile> Grid;

    protected int Id;

    private Tuple<int, int> _connectedRoomIds;

    protected Corridor() { }

    public Corridor(List<Vector2> tiles, Vector2 playerPosition, bool isCorridorHorizontal, bool isDiggingRoom, Map map, Tuple<int, int> connectedRoomIds) {
        TopLeft = new Vector2(x: tiles.Min(tile => tile.X), y: tiles.Min(tile => tile.Y));
        BottomRight = new Vector2(x: tiles.Max(tile => tile.X), y: tiles.Max(tile => tile.Y));
        Grid = new Dictionary<Vector2, Tile>();
        Id = -1;
        _connectedRoomIds = connectedRoomIds;

        HashSet<Vector2> positions = Map.GetTileRegion(TopLeft, BottomRight);
        foreach (Vector2 pos in positions.Where(pos => map.GetTileAt(pos).IsDiggable())) {
            Grid[pos] = Tile.CreateTileForTerrain(FloorTerrain);
        }

        List<Vector2> wallTiles = isDiggingRoom switch {
            true => GetWallTilesForRoomDig(),
            false => GetWallTilesForCorridorDig()
        };

        foreach (Vector2 pos in wallTiles) {
            Grid[pos] = Tile.CreateTileForTerrain(WallTerrain);
        }

        if (!isDiggingRoom) {
            List<int> touchedRoomIds = tiles.Select(map.GetRoomIdForPosition).Distinct().SkipWhile(id => id == -1).ToList();
            if (touchedRoomIds.Count > 0) {
                Room otherRoom = map.GetRoomForId(touchedRoomIds.First());
                otherRoom.AddDoorwayForCorridor(GetFloorTiles());
            }
        }
        
        return;

        List<Vector2> GetWallTilesForRoomDig() {
            List<Vector2> edges = GetEdgePositions();
            return isCorridorHorizontal switch {
                true => edges.Where(tile => (int)tile.Y == (int)TopLeft.Y || (int)tile.Y == (int)BottomRight.Y)
                    .ToList(),
                false => edges.Where(tile => (int)tile.X == (int)TopLeft.X || (int)tile.X == (int)BottomRight.X)
                    .ToList(),
            };
        }

        List<Vector2> GetWallTilesForCorridorDig() {
            List<Vector2> edges = GetEdgePositions();
            List<Vector2> edgesNextToPlayer = edges.Where(tile => {
                return isCorridorHorizontal switch {
                    true => Math.Abs((int)tile.Y - playerPosition.Y) <= 1 &&
                            Math.Abs((int)tile.X - playerPosition.X) <= 2,
                    false => Math.Abs((int)tile.X - playerPosition.X) <= 1 &&
                             Math.Abs((int)tile.Y - playerPosition.Y) <= 2,
                };
            }).ToList();
            List<Vector2> edgesInOtherRoom = edges.Where(tile => !map.GetTileAt(tile).IsDiggable()).ToList();
            return edges.Except(edgesNextToPlayer).Except(edgesInOtherRoom).ToList();
        }
    }

    public Tuple<int, int> GetConnectedRoomIds() {
        return _connectedRoomIds;
    }

    public Vector2 GetTopLeft() {
        return TopLeft;
    }

    public Vector2 GetBottomRight() {
        return BottomRight;
    }

    public int GetId() {
        return Id;
    }

    public bool ContainsPosition(Vector2 position) {
        return position.X >= TopLeft.X && position.X <= BottomRight.X &&
               position.Y >= TopLeft.Y && position.Y <= BottomRight.Y;
    }
    
    public void Draw(SpriteBatch spriteBatch) {
        foreach ((Vector2 position, Tile tile) in Grid) {
            tile.Draw(spriteBatch, position);
        }
    }

    public Tile GetTileAt(Vector2 position) {
        return Grid[position];
    }

    public List<Vector2> GetFloorTiles() {
        return Grid.Keys.Where(pos => Grid[pos].IsWalkable()).ToList();
    }

    public List<Vector2> GetEdgePositions() {
        List<Vector2> wallPositions = GetTilePositions().Where(pos => {
            return (int)pos.X == (int)TopLeft.X || 
                   (int)pos.Y == (int)TopLeft.Y ||
                   (int)pos.X == (int)BottomRight.X ||
                   (int)pos.Y == (int)BottomRight.Y;
        }).ToList();
        return wallPositions;
    }

    public List<Vector2> GetTilePositions() {
        return Grid.Keys.ToList();
    }

    private static bool OrthogonallyAdjacentToAnyOfTheseTiles(Vector2 pos, List<Vector2> tiles) {
        return tiles.Exists(tile => 
            Math.Abs((int)tile.X - (int)pos.X) == 1 && (int)tile.Y == (int)pos.Y || 
            Math.Abs((int)tile.Y - (int)pos.Y) == 1 && (int)tile.X == (int)pos.X
        );
    }

    public void AddDoorwayForCorridor(List<Vector2> corridorFloorTiles) {
        List<Vector2> edgePositions = GetEdgePositions();
        List<Vector2> doorwayPositions = edgePositions.Where(pos => OrthogonallyAdjacentToAnyOfTheseTiles(pos, corridorFloorTiles)).ToList();
        foreach (Vector2 position in doorwayPositions) {
            Grid[position] = Tile.CreateTileForTerrain(FloorTerrain);
        }
    }
}