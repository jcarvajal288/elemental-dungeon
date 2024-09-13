using MonogameTest.dialogs;
using MonogameTest.map;
using MonogameTest.player;

namespace MonogameTest;

public static class InputHandler {
    public static GameState HandleInput(GameState gameState, Player player, Map map, ref Excavator excavator, ref DigDialog digDialog) {
        PlayerAction playerAction = PlayerInput.ReadKeyboard(gameState);
        if (playerAction == PlayerAction.Exit) {
            return OnExit(gameState);
        } 
        
        if (gameState == GameState.Moving) {
            return OnPlayerMovement(player, map, playerAction, ref digDialog);
        }
        
        if (gameState == GameState.Digging) {
            return excavator.AdjustBlueprint(playerAction);
        }

        if (gameState == GameState.InDigDialog) {
            GameState newGameState = digDialog.HandleInput(playerAction);
            if (newGameState == GameState.Digging) {
                excavator = new Excavator(map, player, digDialog.IsRoomSelected);
                switch (digDialog.DigDirection) {
                    case PlayerAction.DigLeft:
                        excavator.StartDigLeft();
                        break;
                    case PlayerAction.DigRight:
                        excavator.StartDigRight();
                        break;
                    case PlayerAction.DigUp:
                        excavator.StartDigUp();
                        break;
                    case PlayerAction.DigDown:
                        excavator.StartDigDown();
                        break;
                }
            }
            return newGameState;
        }

        return gameState;
    }

    private static GameState OnPlayerMovement(Player player, Map map, PlayerAction playerAction, ref DigDialog digDialog) {
        player.SendAction(playerAction, map);
        int currentRoomId = map.GetRoomIdForPosition(player.Position);
        if (currentRoomId >= 0 && AttemptingDig(playerAction)) { // if player is not in a corridor
            if (map.IsValidDiggingPosition(player.Position, playerAction)) {
                digDialog = new DigDialog(playerAction);
                return GameState.InDigDialog;
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