using Godot;
using System;

// GameManager singleton
public partial class GameManager : Node
{
    // Singleton access logic variables
    public static GameManager Instance { get; private set;}

    // Game properties variables
    public float timeLimit = 600f; // 10 minutes
    
    // Game state variables
    private bool gameWon = false;

    // Timer logic variables
    private Timer timer;

    public override void _Ready()
    {
        // Set singleton instance
        Instance = this;

        // Initialize timer
        timer = new Timer();
        AddChild(timer);
        timer.WaitTime = timeLimit;
        timer.Start();
    }

    public override void _Process(double delta)
    {
        UpdateTimerDisplays();
        if (timer.IsStopped() && !gameWon)
        {
            GameOver();
        }
        if (gameWon)
        {
            timer.Paused = true;
            GameWin();
        }
    }

    // Update timer text on each floor
    private void UpdateTimerDisplays()
    {
        var timeRemaining = timer.TimeLeft;
        var minutes = Mathf.Floor(timeRemaining / 60);
        var seconds = Mathf.Floor(timeRemaining % 60);
        var centiseconds = Mathf.Floor((timeRemaining % 1) * 100);
        var timeString = $"{minutes:00}:{seconds:00}.{centiseconds:00}";

        foreach (MeshInstance3D display in GetTree().GetNodesInGroup("Timers"))
        {
            if (display != null)
            {
                TextMesh mesh = (TextMesh)display.Mesh;
                mesh.Text = $"{timeString}";
            }
        }
    }

    private void GameWin()
    {
        // TODO animations & sfx, game deactivation
    }

    private void GameOver()
    {
        // TODO animations & sfx, game deactivation
    }
}