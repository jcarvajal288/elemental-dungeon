using Microsoft.Xna.Framework;

namespace MonogameTest.map;

public class Room(Vector2 topLeft, Vector2 bottomRight) {
    private Vector2 _topLeft = topLeft;
    private Vector2 _bottomRight = bottomRight;
    
    public Vector2 TopLeft => _topLeft;
    public Vector2 BottomRight => _bottomRight;

    public bool ContainsPosition(Vector2 position) {
        return position.X >= _topLeft.X && position.X <= _bottomRight.X &&
               position.Y >= _topLeft.Y && position.Y <= _bottomRight.Y;
    }
}