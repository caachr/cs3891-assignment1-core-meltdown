using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages all of the audio for the game. Update this file when any new audio files are added to the project.
/// </summary>
public partial class AudioManager : Node3D
{
    /// <summary>
    /// Library of audio players
    /// </summary>
    private Dictionary<string, AudioStreamPlayer3D> audioPlayers = new Dictionary<string, AudioStreamPlayer3D>();

    public override void _Ready()
    {
        // Library of audio files.
        // When new audio files are added to the project, list them here and they will be fully functional for the next build.
        var audioFiles = new Dictionary<string, string>
        {
            { "LeverUp", "res://Audio/SFX/sfx_lever_up.wav" },
            { "LeverDown", "res://Audio/SFX/sfx_lever_down.wav" },
            { "Hatch", "res://Audio/SFX/sfx_hatch_open.wav" },
            { "Locked", "res://Audio/SFX/sfx_interactable_locked.wav" },
            { "Keypad", "res://Audio/SFX/sfx_keypad_button.wav" },
            { "KeypadSolved", "res://Audio/SFX/sfx_keypad_solved.wav" },
            { "Tick", "res://Audio/SFX/sfx_timer_tick.wav" },
            { "Minute4", "res://Audio/SFX/sfx_minute_mark_4.wav" },
            { "Minute3", "res://Audio/SFX/sfx_minute_mark_3.wav" },
            { "Minute2", "res://Audio/SFX/sfx_minute_mark_2.wav" },
            { "Minute1", "res://Audio/SFX/sfx_minute_mark_1.wav" },
            { "TimeOut", "res://Audio/SFX/sfx_time_out.wav" },
            { "Win", "res://Audio/SFX/sfx_win.wav" }
        };

        // Create audio players for every listed audio file
        foreach (var audio in audioFiles)
        {
            audioPlayers[audio.Key] = CreateAudioPlayer(audio.Value);
        }
    }

    private AudioStreamPlayer3D CreateAudioPlayer(string path)
    {
        var player = new AudioStreamPlayer3D();
        AddChild(player);
        player.Stream = GD.Load<AudioStream>(path);

        // Adjust audio player settings for gentler falloff
        player.UnitSize = 10.0f;
        player.MaxDistance = 20.0f;
        player.AttenuationModel = AudioStreamPlayer3D.AttenuationModelEnum.Logarithmic;
        player.VolumeDb = 2.0f;

        return player;
    }

    // Play the specified sound at the specified location.
    public void Play(string soundName, Vector3 position)
    {
        if (audioPlayers.TryGetValue(soundName, out var player))
        {
            player.Position = position;
            player.Play();
        }
        else
        {
            GD.PrintErr($"Sound '{soundName}' not found!");
        }
    }

    // Clean up all the audio players in preparation for game exit.
    public void CleanUpAllPlayers()
    {
        foreach (var player in audioPlayers.Values)
        {
            player.Stop();
            player.QueueFree();
        }
        audioPlayers.Clear();
    }
}
