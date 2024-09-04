using MonogameTest.dialogs;
using MonogameTest.map;
using MonogameTest.player;

namespace MonogameTest;

public static class InputHandler {
    public static GameState HandleInput(GameState gameState, Player player, Map map, RoomDigger roomDigger, DigDialog digDialog) {
        PlayerAction playerAction = PlayerInput.ReadKeyboard(gameState);
        if (playerAction == PlayerAction.Exit) {
            return OnExit(gameState);
        } 
        
        if (gameState == GameState.Moving) {
            return OnPlayerMovement(gameState, player, map, playerAction, roomDigger);
        }
        
        if (gameState == GameState.Digging) {
            return roomDigger.AdjustBlueprint(playerAction, map);
        }

        if (gameState == GameState.InDigDialog) {
            return digDialog.HandleInput(playerAction);
        }

        return gameState;
    }

    private static GameState OnPlayerMovement(GameState gameState, Player player, Map map, PlayerAction playerAction, RoomDigger roomDigger) {
        player.SendAction(playerAction, map);
        int currentRoomId = map.GetRoomIdForPosition(player.Position);
        if (currentRoomId >= 0) { // if player is not in a corridor
            if (roomDigger.IsNewDigValid(gameState, playerAction, player.Position, map, currentRoomId)) {
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