using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.map;

public class Corridor {
    private Vector2 _topLeft;
    private Vector2 _bottomRight;
    private Terrain _floorTerrain = Terrain.OrcFloor;
    private Terrain _wallTerrain = Terrain.OrcWall;
    
    private readonly Dictionary<Vector2, Tile> _grid;

    public Corridor(Vector2 topLeft, Vector2 bottomRight, List<Vector2> tiles, bool isHorizontal) {
        _topLeft = topLeft;
        _bottomRight = bottomRight;
        _grid = new Dictionary<Vector2, Tile>();
        
        HashSet<Vector2> positions = RoomDigger.GetTileRegion(topLeft, bottomRight);
        foreach (Vector2 pos in positions) {
            _grid[pos] = Tile.CreateTileForTerrain(_floorTerrain);
        }
             
        List<Vector2> wallTiles = isHorizontal switch {
            true => tiles.Where(tile => (int)tile.Y == (int)topLeft.Y || (int)tile.Y == (int)bottomRight.Y).ToList(),
            false => tiles.Where(tile => (int)tile.X == (int)topLeft.X || (int)tile.X == (int)bottomRight.X).ToList(),
        };
        foreach (Vector2 pos in wallTiles) {
            _grid[pos] = Tile.CreateTileForTerrain(_wallTerrain);
        }
    }
    
    public bool ContainsPosition(Vector2 position) {
        return position.X >= _topLeft.X && position.X <= _bottomRight.X &&
               position.Y >= _topLeft.Y && position.Y <= _bottomRight.Y;
    }
    
    public void Draw(SpriteBatch spriteBatch) {
        foreach ((Vector2 position, Tile tile) in _grid) {
            tile.Draw(spriteBatch, position);
        }
    }

    public Tile GetTileAt(Vector2 position) {
        return _grid[position];
    }
}