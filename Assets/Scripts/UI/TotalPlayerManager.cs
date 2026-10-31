using UnityEngine;

public class TotalPlayerManager : MonoBehaviour
{
    public static GameState CurrentState { get; private set; } = GameState.None;
    public static int HumanPlayerCount => (int)CurrentState;    
    
    public enum GameState
    {
        None = 0,
        OnePlayer = 1,
        TwoPlayers = 2,
        ThreePlayers = 3,
        FourPlayers = 4
    }

    private void Start()
    {
        CurrentState = GameState.None;
    }

   public void SelectOnePlayer()
    {
        CurrentState = GameState.OnePlayer;
    }

    public void SelectTwoPlayers()
    {
        CurrentState = GameState.TwoPlayers;
    }

    public void SelectThreePlayers()
    {
        CurrentState = GameState.ThreePlayers;
    }

    public void SelectFourPlayers()
    {
        CurrentState = GameState.FourPlayers;
    }
}