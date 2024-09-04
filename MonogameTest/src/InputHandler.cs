using MonogameTest.dialogs;
using MonogameTest.map;
using MonogameTest.player;

namespace MonogameTest;

public static class InputHandler {
    public static GameState HandleInput(GameState gameState, Player player, Map map) {
        PlayerAction playerAction = PlayerInput.ReadKeyboard(gameState);
        if (playerAction == PlayerAction.Exit) {
            return OnExit(gameState);
        } 
        
        if (gameState == GameState.Moving) {
            return OnPlayerMovement(gameState, player, map, playerAction);
        }
        
        if (gameState == GameState.Digging) {
            return RoomDigger.AdjustBlueprint(playerAction, map);
        }

        if (gameState == GameState.InDigDialog) {
            return DigDialog.HandleInput(playerAction);
        }

        return gameState;
    }

    private static GameState OnPlayerMovement(GameState gameState, Player player, Map map, PlayerAction playerAction) {
        player.SendAction(playerAction, map);
        int currentRoomId = map.GetRoomIdForPosition(player.Position);
        if (currentRoomId >= 0) { // if player is not in a corridor
            if (RoomDigger.IsNewDigValid(gameState, playerAction, player.Position, map, currentRoomId)) {
                return GameState.InDigDialog;
            }
        }
        return GameState.Moving;
    }

    private static GameState OnExit(GameState gameState) {
        if (gameState == GameState.Moving) {
            return GameState.Exit;
        }
        return GameState.Moving;
    }
}