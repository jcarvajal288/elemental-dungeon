using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest.map;

public class Room() {
    protected Vector2 _topLeft;
    protected Vector2 _bottomRight;
    
    protected readonly Dictionary<Vector2, Tile> _grid;
    protected readonly Terrain _fillTerrain;
    protected readonly Terrain _wallTerrain;

    public Room(Vector2 topLeft, Vector2 bottomRight) : this() {
        _topLeft = topLeft;
        _bottomRight = bottomRight;
        _fillTerrain = Terrain.OrcFloor;
        _wallTerrain = Terrain.OrcWall;
        _grid = new Dictionary<Vector2, Tile>();
        Build();
    }
    
    public bool ContainsPosition(Vector2 position) {
        return position.X >= _topLeft.X && position.X <= _bottomRight.X &&
               position.Y >= _topLeft.Y && position.Y <= _bottomRight.Y;
    }

    private void Build() {
        HashSet<Vector2> positions = RoomDigger.GetTileRegion(_topLeft, _bottomRight);
        foreach (Vector2 pos in positions) {
            _grid[pos] = Tile.CreateTileForTerrain(_fillTerrain);
        }

        List<Vector2> wallPositions = positions.Where(pos => {
            return (int)pos.X == (int)_topLeft.X || 
                   (int)pos.Y == (int)_topLeft.Y ||
                   (int)pos.X == (int)_bottomRight.X ||
                   (int)pos.Y == (int)_bottomRight.Y;
        }).ToList();
        foreach (Vector2 pos in wallPositions) {
            _grid[pos] = Tile.CreateTileForTerrain(_fillTerrain);
        }
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