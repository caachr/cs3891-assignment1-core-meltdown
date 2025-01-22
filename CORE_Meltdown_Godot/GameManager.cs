using Godot;
using System;

/// <summary>
/// Manages the game state at the highest level, including win/lose state, UI, and actions like quitting and restarting the game.
/// </summary>
public partial class GameManager : Node
{
    /// <summary>
    /// GameManager singleton instance
    /// </summary>
    public static GameManager Instance { get; private set;}

    /// <summary>
    /// How long the player has to complete the game, in seconds
    /// </summary>
    public float timeLimit;
    
    /// <summary>
    /// Game win status
    /// </summary>
    public bool gameWin;

    /// <summary>
    /// Game end status
    /// </summary>
    private bool gameEnd;

    // In-game UI
    [Export] private Node3D gameWinText;
    [Export] private Node3D reactorStableText;
    [Export] private Node3D reactorUnstableText;
    [Export] private Node3D gameLoseText;
    [Export] private Node3D[] hints;

    /// <summary>
    /// Timer instance
    /// </summary>
    private Timer timer;
    
    /// <summary>
    /// Audio manager instance
    /// </summary>
    private AudioManager audioManager;

    // Audio timing logic
    private int lastSecond = -1;
    private bool minute4Played;
    private bool minute3Played;
    private bool minute2Played;
    private bool minute1Played;

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

        // Initialize audio manager instance
        audioManager = GetNode<AudioManager>("/root/Main/AudioManager");

        minute4Played = false;
        minute3Played = false;
        minute2Played = false;
        minute1Played = false;
    }

    public override void _Process(double delta)
    {
        // Only check logic if the game has not yet ended.
        if (!gameEnd)
        {
            UpdateTimerDisplays();

            // Check if the game has been won or lost; if so, perform the appropriate game end events.
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

    /// <summary>
    /// Update timer text on each floor
    /// </summary>
    private void UpdateTimerDisplays()
    {
        // Deconstruct the time remaining into minutes, seconds, and centiseconds.
        var timeRemaining = timer.TimeLeft;
        var minutes = Mathf.Floor(timeRemaining / 60);
        var seconds = Mathf.Floor(timeRemaining % 60);
        var centiseconds = Mathf.Floor((timeRemaining % 1) * 100);

        // Format the time remaining as minutes, seconds, and centiseconds. E.g. 09:59.97
        var timeString = $"{minutes:00}:{seconds:00}.{centiseconds:00}";

        // Play a tick sound at each second.
        if (Mathf.Floor(timeRemaining) != lastSecond)
        {
            lastSecond = (int)Mathf.Floor(timeRemaining);
            var playerPosition = GetNode<Node3D>("/root/Main/Player").GlobalPosition;
            audioManager.Play("Tick", playerPosition);
        }
        
        // Play a sound effect when the number of minutes left changes.
        if (timeRemaining <= 240 && !minute4Played)
        {
            minute4Played = true;
            audioManager.Play("Minute4", GetNode<Node3D>("/root/Main/Player").GlobalPosition);
        }
        if (timeRemaining <= 180 && !minute3Played)
        {
            minute3Played = true;
            audioManager.Play("Minute3", GetNode<Node3D>("/root/Main/Player").GlobalPosition);
        }
        if (timeRemaining <= 120 && !minute2Played)
        {
            minute2Played = true;
            audioManager.Play("Minute2", GetNode<Node3D>("/root/Main/Player").GlobalPosition);
        }
        if (timeRemaining <= 60 && !minute1Played)
        {
            minute1Played = true;
            audioManager.Play("Minute1", GetNode<Node3D>("/root/Main/Player").GlobalPosition);
        }

        // Update the digit displays.
        foreach (MeshInstance3D display in GetTree().GetNodesInGroup("Timers"))
        {
            if (display != null)
            {
                TextMesh mesh = (TextMesh)display.Mesh;
                mesh.Text = $"{timeString}";
            }
        }
    }

    /// <summary>
    /// Perform events corresponding to a game win.
    /// </summary>
    private void GameWin()
    {
        // Pause the timer and show relevant UI text.
        timer.Paused = true;
        reactorUnstableText.Visible = false;
        reactorStableText.Visible = true;
        gameWinText.Visible = true;

        // Play SFX
        audioManager.Play("Win", reactorStableText.GlobalPosition);
    }

    /// <summary>
    /// Perform events corresponding to a game loss.
    /// </summary>
    private void GameLose()
    {
        // Show in-game UI.
        gameLoseText.Visible = true;

        // Lock all levers.
        foreach(Lever lever in GetTree().GetNodesInGroup("Levers"))
        {
            lever.locked = true;
        }
        
        // Play lose sound.
        audioManager.Play("TimeOut", GetNode<Node3D>("/root/Main/Player").GlobalPosition);
    }

    public override void _Input(InputEvent @event)
    {
        // Restart the game.
        if (@event.IsActionPressed("restart"))
        {
            GetTree().ReloadCurrentScene();
        }

        // Quit the game.
		if (@event.IsActionPressed("ui_cancel"))
		{
			audioManager.CleanUpAllPlayers();
            GetTree().Quit();
		}
    }
}