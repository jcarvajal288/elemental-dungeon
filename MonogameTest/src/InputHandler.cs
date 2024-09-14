using MonogameTest.dialogs;
using MonogameTest.map;
using MonogameTest.player;

namespace MonogameTest;

public static class InputHandler {
    public static GameState HandleInput(GameState gameState, Player player, Map map, ref Excavator excavator) {
        PlayerAction playerAction = PlayerInput.ReadKeyboard(gameState);
        if (playerAction == PlayerAction.Exit) {
            return OnExit(gameState);
        } 
        
        if (gameState == GameState.Moving) {
            return HandlePlayerMovement(player, map, playerAction, ref excavator);
        }
        
        if (gameState == GameState.Digging) {
            return excavator.HandleInput(playerAction);
        }

        return gameState;
    }

    private static GameState HandlePlayerMovement(Player player, Map map, PlayerAction playerAction, ref Excavator excavator) {
        player.SendAction(playerAction, map);
        int currentRoomId = map.GetRoomIdForPosition(player.Position);
        if (currentRoomId >= 0 && AttemptingDig(playerAction)) { // if player is not in a corridor
            if (map.IsValidDiggingPosition(player.Position, playerAction)) {
                excavator = new Excavator(map, player, playerAction);
                return GameState.Digging;
            }
        }
        return GameState.Moving;
    }

    private static bool AttemptingDig(PlayerAction playerAction) {
        return  playerAction is PlayerAction.DigRight or PlayerAction.DigLeft
            or PlayerAction.DigUp or PlayerAction.DigDown;
    }

    private static GameState OnExit(GameState gameState) {
        if (gameState == GameState.Moving) {
            return GameState.Exit;
        }
        return GameState.Moving;
    }
}