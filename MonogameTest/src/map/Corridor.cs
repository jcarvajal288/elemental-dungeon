using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.map;

public class Corridor {
    protected Vector2 TopLeft;
    protected Vector2 BottomRight;
    protected Terrain FloorTerrain = Terrain.OrcFloor;
    protected Terrain WallTerrain = Terrain.OrcWall;
    
    protected Dictionary<Vector2, Tile> Grid;

    public Corridor(Vector2 topLeft, Vector2 bottomRight, List<Vector2> tiles, bool isHorizontal) {
        TopLeft = topLeft;
        BottomRight = bottomRight;
        Grid = new Dictionary<Vector2, Tile>();
        
        HashSet<Vector2> positions = RoomDigger.GetTileRegion(topLeft, bottomRight);
        foreach (Vector2 pos in positions) {
            Grid[pos] = Tile.CreateTileForTerrain(FloorTerrain);
        }
             
        List<Vector2> wallTiles = isHorizontal switch {
            true => tiles.Where(tile => (int)tile.Y == (int)topLeft.Y || (int)tile.Y == (int)bottomRight.Y).ToList(),
            false => tiles.Where(tile => (int)tile.X == (int)topLeft.X || (int)tile.X == (int)bottomRight.X).ToList(),
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
}