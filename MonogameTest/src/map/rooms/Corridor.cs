using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.map.rooms;

public class Corridor {
    protected Vector2 TopLeft;
    protected Vector2 BottomRight;
    protected Terrain FloorTerrain = Terrain.CobbleStoneFloor;
    protected Terrain WallTerrain = Terrain.GreyBrickWall;
    
    protected Dictionary<Vector2, Tile> Grid;

    public Corridor(List<Vector2> tiles, bool isHorizontal) {
        TopLeft = new Vector2(x: tiles.Min(tile => tile.X), y: tiles.Min(tile => tile.Y));
        BottomRight = new Vector2(x: tiles.Max(tile => tile.X), y: tiles.Max(tile => tile.Y));
        Grid = new Dictionary<Vector2, Tile>();
        
        HashSet<Vector2> positions = RoomDigger.GetTileRegion(TopLeft, BottomRight);
        foreach (Vector2 pos in positions) {
            Grid[pos] = Tile.CreateTileForTerrain(FloorTerrain);
        }
             
        List<Vector2> wallTiles = isHorizontal switch {
            true => tiles.Where(tile => (int)tile.Y == (int)TopLeft.Y || (int)tile.Y == (int)BottomRight.Y).ToList(),
            false => tiles.Where(tile => (int)tile.X == (int)TopLeft.X || (int)tile.X == (int)BottomRight.X).ToList(),
        };
        foreach (Vector2 pos in wallTiles) {
            Grid[pos] = Tile.CreateTileForTerrain(WallTerrain);
        }
    }

    protected Corridor() { }

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
}