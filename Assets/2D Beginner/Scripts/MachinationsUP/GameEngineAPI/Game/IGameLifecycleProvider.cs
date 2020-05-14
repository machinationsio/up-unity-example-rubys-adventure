namespace MachinationsUP.GameEngineAPI.Game
{
    /// <summary>
    /// Defines a contract that allows a Game Engine to be queried by Machinations.
    /// </summary>
    public interface IGameLifecycleProvider
    {

        /// <summary>
        /// Return current GameState.
        /// </summary>
        GameStates GetGameState ();

    }
}