using Godot;
using System;

public partial class AudioManager : Node3D
{
    private AudioStreamPlayer3D leverUpAudio;
    private AudioStreamPlayer3D leverDownAudio;
    private  AudioStreamPlayer3D hatchAudio;
    private AudioStreamPlayer3D lockedAudio;
    private AudioStreamPlayer3D keypadAudio;
    private AudioStreamPlayer3D keypadSolvedAudio;
    private AudioStreamPlayer3D timerTickAudio;
    private AudioStreamPlayer3D minute4Audio;
    private AudioStreamPlayer3D minute3Audio;
    private AudioStreamPlayer3D minute2Audio;
    private AudioStreamPlayer3D minute1Audio;
    private AudioStreamPlayer3D timeOutAudio;
    private AudioStreamPlayer3D winAudio;

    public override void _Ready()
    {        
        leverUpAudio = CreateAudioPlayer("res://Audio/SFX/sfx_lever_up.wav");
        leverDownAudio = CreateAudioPlayer("res://Audio/SFX/sfx_lever_down.wav");
        hatchAudio = CreateAudioPlayer("res://Audio/SFX/sfx_hatch_open.wav");
        lockedAudio = CreateAudioPlayer("res://Audio/SFX/sfx_interactable_locked.wav");
        keypadAudio = CreateAudioPlayer("res://Audio/SFX/sfx_keypad_button.wav");
        keypadSolvedAudio = CreateAudioPlayer("res://Audio/SFX/sfx_keypad_solved.wav");
        timerTickAudio = CreateAudioPlayer("res://Audio/SFX/sfx_timer_tick.wav");
        minute4Audio = CreateAudioPlayer("res://Audio/SFX/sfx_minute_mark_4.wav");
        minute3Audio = CreateAudioPlayer("res://Audio/SFX/sfx_minute_mark_3.wav");
        minute2Audio = CreateAudioPlayer("res://Audio/SFX/sfx_minute_mark_2.wav");
        minute1Audio = CreateAudioPlayer("res://Audio/SFX/sfx_minute_mark_1.wav");
        timeOutAudio = CreateAudioPlayer("res://Audio/SFX/sfx_time_out.wav");
        winAudio = CreateAudioPlayer("res://Audio/SFX/sfx_win.wav");
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

    private void PlaySoundAtPosition(AudioStreamPlayer3D player, Vector3 position)
    {
        player.Position = position;
        player.Play();
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

    public void PlayKeypadSolvedSFX(Vector3 position)
    {
        PlaySoundAtPosition(keypadSolvedAudio, position);
    }

    public void PlayTimerTickSFX(Vector3 position)
    {
        PlaySoundAtPosition(timerTickAudio, position);
    }

    public void PlayMinute4SFX(Vector3 position)
    {
        PlaySoundAtPosition(minute4Audio, position);
    }

    public void PlayMinute3SFX(Vector3 position)
    {
        PlaySoundAtPosition(minute3Audio, position);
    }

    public void PlayMinute2SFX(Vector3 position)
    {
        PlaySoundAtPosition(minute2Audio, position);
    }

    public void PlayMinute1SFX(Vector3 position)
    {
        PlaySoundAtPosition(minute1Audio, position);
    }

    public void PlayTimeOutSFX(Vector3 position)
    {
        PlaySoundAtPosition(timeOutAudio, position);
    }

    public void PlayWinSFX(Vector3 position)
    {
        PlaySoundAtPosition(winAudio, position);
    }
}