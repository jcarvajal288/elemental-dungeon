using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonogameTest.map;
using MonogameTest.player;

namespace MonogameTest.dialogs;

public class RoomSelectionDialog {
    public RoomType SelectedRoomType { get; private set; } = RoomType.StartingRoom;

    public void Draw(SpriteBatch spriteBatch, BitmapFont font, Vector2 cameraTopLeft) {
        spriteBatch.DrawString(font, "What room will you dig?", cameraTopLeft, Color.White);
        foreach (var it in Enum.GetValues<RoomType>().Select((roomType, i) => new { roomType, i })) {
            Vector2 topLeft = cameraTopLeft with { Y = cameraTopLeft.Y + font.Size * (it.i + 1) };
            if (it.roomType == SelectedRoomType) {
                spriteBatch.DrawString(font, ">" + it.roomType, topLeft, Color.Yellow);
            } else {
                spriteBatch.DrawString(font, "  " + it.roomType, topLeft, Color.White);
            }
        }
    }
    
    public bool HasSelectedRoomType(PlayerAction playerAction) {
        RoomType[] roomTypes = Enum.GetValues<RoomType>();
        switch (playerAction) {
            case PlayerAction.SubmitAction:
                return true;
            case PlayerAction.MoveUp:
                SelectedRoomType += 1;
                if ((int)SelectedRoomType > roomTypes.Length - 1) {
                    SelectedRoomType = 0;
                }
                return false;
            case PlayerAction.MoveDown:
                SelectedRoomType -= 1;
                if ((int)SelectedRoomType < 0) {
                    SelectedRoomType = roomTypes[^1];
                }
                return false;
            default:
                return false;
        }
    }
}