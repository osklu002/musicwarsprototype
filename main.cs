using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class main : Node2D
{

    Button c, d, e, f, g, a;
    TextureProgressBar timerBar;
    bool ActiveInput = false;
    StringBuilder stringBuilder;
    AudioStreamPlayer audioStreamPlayer;
    AudioStream aSound, dSound, cSound, gSound, fSound, eSound;
    Label inputLabel, feedback, spellList;

    List<(string, string)> spells = new List<(string, string)>() {
        ("Three Blind Mice", "EDCEDC"),
        ("Twinkle Twinkle Little Star", "CCGGAAGFFEEDDC")
    };
	public override void _Ready()
    {
        audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        aSound = GD.Load<AudioStream>("res://sounds/a.wav");
        dSound = GD.Load<AudioStream>("res://sounds/d.wav");
        cSound = GD.Load<AudioStream>("res://sounds/c.wav");
        eSound = GD.Load<AudioStream>("res://sounds/e.wav");
        gSound = GD.Load<AudioStream>("res://sounds/g.wav");
        fSound = GD.Load<AudioStream>("res://sounds/f.wav");

        inputLabel = GetNode<Label>("InputLabel");
        feedback = GetNode<Label>("Feedback");
        spellList = GetNode<Label>("SpellList");

        foreach(var spell in spells) {
            spellList.Text += $"{spell.Item2} - {spell.Item1}\r\n";
        }

        // Get the button nodes
        c = GetNode<Button>("C");
        d = GetNode<Button>("D");
        e = GetNode<Button>("E");
        f = GetNode<Button>("F");
        g = GetNode<Button>("G");
        a = GetNode<Button>("A");
        timerBar = GetNode<TextureProgressBar>("Timer");
        timerBar.Value = 100;

        c.Pressed += () => ButtonPressed("C");
		d.Pressed += () => ButtonPressed("D");
        e.Pressed += () => ButtonPressed("E");
        f.Pressed += () => ButtonPressed("F");
        g.Pressed += () => ButtonPressed("G");
        a.Pressed += () => ButtonPressed("A");

        stringBuilder = new StringBuilder();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (ActiveInput) {
            timerBar.Value -= 1;
        }
        if (timerBar.Value <= 0) {
            ActiveInput = false;
            timerBar.Value = 100;
            CastSpell(stringBuilder.ToString());
            stringBuilder.Clear();
        }
    }

    private void ButtonPressed(string button) {
        feedback.Text = "";
        switch(button) {
            case "A":
                audioStreamPlayer.Stream = aSound;
                break;
            case "C":
                audioStreamPlayer.Stream = cSound;
                break;
            case "D":
                audioStreamPlayer.Stream = dSound;
                break;
            case "E":
                audioStreamPlayer.Stream = eSound;
                break;
            case "F":
                audioStreamPlayer.Stream = fSound;
                break;
        }
        audioStreamPlayer.Play();
        if (!ActiveInput) {
            ActiveInput = true;
        } else {
            timerBar.Value = 100;
        }
		stringBuilder.Append(button);
        inputLabel.Text = stringBuilder.ToString();
	}

    private void CastSpell(string input) {
        var spell = spells.FirstOrDefault(spell => spell.Item2 == input);
        if (spell != default) {
            GD.Print($"{spell.Item1} was a valid spell!");
            feedback.Text = spell.Item1;
        } else {
            feedback.Text = "Unknown spell";
        }
    }
}
