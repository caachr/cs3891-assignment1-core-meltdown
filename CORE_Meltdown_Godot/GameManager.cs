using Godot;
using System;

// GameManager singleton
public partial class GameManager : Node
{
    // Singleton access logic
    public static GameManager Instance { get; private set;}

    // How long the player has to complete the game, in seconds
    public float timeLimit;
    
    // Game state
    public bool gameWin;
    private bool gameEnd;

    // In-game UI
    [Export] private Node3D gameWinText;
    [Export] private Node3D reactorStableText;
    [Export] private Node3D reactorUnstableText;
    [Export] private Node3D gameLoseText;
    [Export] private Node3D[] hints;

    // Timer logic
    private Timer timer;

    public override void _Ready()
    {
        // Set singleton instance
        Instance = this;

        // How long player has to complete game
        timeLimit = 60f;

        // Game state
        gameWin = false;
        gameEnd = false;

        // Initialize timer
        timer = new Timer();
        AddChild(timer);
        timer.WaitTime = timeLimit;
        timer.Start();

        // Initialize UI
        gameWinText.Visible = false;
        gameLoseText.Visible = false;
        foreach (Node3D hint in hints)
        {
            hint.Visible = false;
        }
    }

    public override void _Process(double delta)
    {
        if (!gameEnd)
        {
            UpdateTimerDisplays();
            if (timer.TimeLeft - 540f < 1)
            {
                hints[0].Visible = true;
            }
            if (timer.TimeLeft - 510f < 1)
            {
                hints[1].Visible = true;
            }
            if (timer.IsStopped())
            {
                gameEnd = true;
                gameWin = false;
                GameLose();
            }
            if (gameWin)
            {
                gameEnd = true;
                timer.Paused = true;
                GameWin();
            }
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
        timer.Paused = true;
        reactorUnstableText.Visible = false;
        reactorStableText.Visible = true;
        gameWinText.Visible = true;
    }

    private void GameLose()
    {
        gameLoseText.Visible = true;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("restart"))
        {
            GetTree().ReloadCurrentScene();
        }

		if (@event.IsActionPressed("ui_cancel"))
		{
			GetTree().Quit();
		}
    }
}