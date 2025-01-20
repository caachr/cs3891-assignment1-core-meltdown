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
    
    // Audio manager instance
    private AudioManager audioManager;

    // Audio timing logic
    int lastSecond = -1;
    bool minute4Played;
    bool minute3Played;
    bool minute2Played;
    bool minute1Played;

    public override void _Ready()
    {
        // Set singleton instance
        Instance = this;

        // How long player has to complete game
        timeLimit = 250f;

        // Game state
        gameWin = false;
        gameEnd = false;

        // Initialize timer
        timer = new Timer();
        AddChild(timer);
        timer.WaitTime = timeLimit;
        timer.OneShot = true;
        timer.Start();

        // Initialize UI
        gameWinText.Visible = false;
        gameLoseText.Visible = false;
        // foreach (Node3D hint in hints)
        // {
        //     hint.Visible = false;
        // }

        // Initialize audio manager instance
        audioManager = GetNode<AudioManager>("/root/Main/AudioManager");

        minute4Played = false;
        minute3Played = false;
        minute2Played = false;
        minute1Played = false;
    }

    public override void _Process(double delta)
    {
        if (!gameEnd)
        {
            UpdateTimerDisplays();
            // if (timer.TimeLeft - 240f < 1)
            // {
            //     hints[0].Visible = true;
            // }
            // if (timer.TimeLeft - 220f < 1)
            // {
            //     hints[1].Visible = true;
            // }
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

        if (Mathf.Floor(timeRemaining) != lastSecond)
        {
            lastSecond = (int)Mathf.Floor(timeRemaining);
            var playerPosition = GetNode<Node3D>("/root/Main/Player").GlobalPosition;
            audioManager.PlayTimerTickSFX(playerPosition);
        }

        if (timeRemaining <= 240 && !minute4Played)
        {
            minute4Played = true;
            audioManager.PlayMinute4SFX(GetNode<Node3D>("/root/Main/Player").GlobalPosition);
        }
        if (timeRemaining <= 180 && !minute3Played)
        {
            minute3Played = true;
            audioManager.PlayMinute3SFX(GetNode<Node3D>("/root/Main/Player").GlobalPosition);
        }
        if (timeRemaining <= 120 && !minute2Played)
        {
            minute2Played = true;
            audioManager.PlayMinute2SFX(GetNode<Node3D>("/root/Main/Player").GlobalPosition);
        }
        if (timeRemaining <= 60 && !minute1Played)
        {
            minute1Played = true;
            audioManager.PlayMinute1SFX(GetNode<Node3D>("/root/Main/Player").GlobalPosition);
        }

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

        audioManager.PlayWinSFX(reactorStableText.GlobalPosition);
    }

    private void GameLose()
    {
        gameLoseText.Visible = true;

        // Lock all levers.
        foreach(Lever lever in GetTree().GetNodesInGroup("Levers"))
        {
            lever.locked = true;
        }
        audioManager.PlayTimeOutSFX(GetNode<Node3D>("/root/Main/Player").GlobalPosition);
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