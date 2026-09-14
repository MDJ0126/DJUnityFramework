using Game;
using System.Collections;

public abstract class GameMode : SingletonBehaviour<GameMode>
{
	#region Inspector

	public Pawn defaultPawn;

	// HUD Class

	public PlayerController playerController;

    // GameState Class

    // PlayerState Class

    // Spectator Class

    #endregion

    protected virtual IEnumerator Start()
    {
        yield return null;
        playerController.Possess(defaultPawn);
    }
}