using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModifierState
{
    public ModifierDefinition Definition { get; set; }
    public bool Enabled { get; set; }
}

public class ModifierManager : MBSingleton<ModifierManager>
{
    public List<ModifierDefinition> AllModifiers { get; private set; }
    public List<ModifierState> RuntimeStates { get; private set; }

    protected override void Awake()
    {
        AllModifiers = new(Resources.LoadAll<ModifierDefinition>("Modifiers/"));
        RuntimeStates = new();

        foreach(var definition in AllModifiers)
        {
            ModifierState state = new();
            state.Definition = definition;
            state.Enabled = false;

            RuntimeStates.Add(state);
        }
    }

    public void Dummy() { }

    public void EnableModifier(ModifierDefinition definition, bool enabled)
    {
        var state = RuntimeStates.Find(m => m.Definition == definition);

        if(state != null)
        {
            state.Enabled = enabled;
        }
    }

    public bool IsEnabled(ModifierDefinition definition)
    {
        return RuntimeStates.Find(m => m.Definition == definition).Enabled;
    }

    public Modifier[] GetActiveModifiers()
    {
        List<Modifier> active = new();
        var activeStates = RuntimeStates.FindAll(m => m.Enabled);

        foreach(var state in activeStates)
        {
            Modifier mod = (Modifier)Activator.CreateInstance(Type.GetType(state.Definition.cSharpModifierName));
            active.Add(mod);
        }

        return active.ToArray();
    }
}
