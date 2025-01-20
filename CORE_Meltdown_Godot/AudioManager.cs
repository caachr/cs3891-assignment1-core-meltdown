using Godot;
using System;

public partial class AudioManager : Node3D
{
    private AudioStreamPlayer3D leverUpAudio;
    private AudioStreamPlayer3D leverDownAudio;
    private  AudioStreamPlayer3D hatchAudio;
    private AudioStreamPlayer3D lockedAudio;
    private AudioStreamPlayer3D keypadAudio;
    private AudioStreamPlayer3D timerTickAudio;

    public override void _Ready()
    {
        leverUpAudio = CreateAudioPlayer("res://Audio/SFX/sfx_lever_up.wav");
        leverDownAudio = CreateAudioPlayer("res://Audio/SFX/sfx_lever_down.wav");
        hatchAudio = CreateAudioPlayer("res://Audio/SFX/sfx_hatch_open.wav");
        lockedAudio = CreateAudioPlayer("res://Audio/SFX/sfx_interactable_locked.wav");
        keypadAudio = CreateAudioPlayer("res://Audio/SFX/sfx_keypad_button.wav");
        timerTickAudio = CreateAudioPlayer("res://Audio/SFX/sfx_timer_tick.wav");
    }

    private AudioStreamPlayer3D CreateAudioPlayer(string path)
    {
        var player = new AudioStreamPlayer3D();
        AddChild(player);
        player.Stream = GD.Load<AudioStream>(path);
        
        // Gentler falloff settings
        player.UnitSize = 10.0f;
        player.MaxDistance = 20.0f;
        player.AttenuationModel = AudioStreamPlayer3D.AttenuationModelEnum.Logarithmic;
        player.VolumeDb = 2.0f;  // Slightly louder base volume
        
        return player;
    }

    public void PlayLeverUpSFX(Vector3 position)
    {
        PlaySoundAtPosition(leverUpAudio, position);
    }

    public void PlayLeverDownSFX(Vector3 position)
    {
        PlaySoundAtPosition(leverDownAudio, position);
    }

    public void PlayHatchSFX(Vector3 position)
    {
        PlaySoundAtPosition(hatchAudio, position);
    }

    public void PlayLockedSFX(Vector3 position)
    {
        PlaySoundAtPosition(lockedAudio, position);
    }

    public void PlayKeypadSFX(Vector3 position)
    {
        PlaySoundAtPosition(keypadAudio, position);
    }

    public void PlayTimerTickSFX(Vector3 position)
    {
        PlaySoundAtPosition(timerTickAudio, position);
    }

    private void PlaySoundAtPosition(AudioStreamPlayer3D player, Vector3 position)
    {
        player.Position = position;
        player.Play();
    }
}