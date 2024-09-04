using MonogameTest.map;
using MonogameTest.player;

namespace MonogameTest;

public static class InputHandler {
    public static GameState HandleInput(GameState gameState, Player player, Map map) {
        PlayerAction playerAction = PlayerInput.ReadKeyboard(gameState);
        if (playerAction == PlayerAction.Exit) {
            if (gameState == GameState.Moving) {
                return GameState.Exit;
            }
            return GameState.Moving;
        } 
        
        if (gameState == GameState.Moving) {
            player.SendAction(playerAction, map);
            int currentRoomId = map.GetRoomIdForPosition(player.Position);
            if (currentRoomId >= 0) { // if player is not in a corridor
                if (RoomDigger.IsNewDigValid(gameState, playerAction, player.Position, map, currentRoomId)) {
                    return GameState.Digging;
                }
            }
        } else {
            return RoomDigger.AdjustBlueprint(playerAction, map);
        }

        return gameState;
    }
}