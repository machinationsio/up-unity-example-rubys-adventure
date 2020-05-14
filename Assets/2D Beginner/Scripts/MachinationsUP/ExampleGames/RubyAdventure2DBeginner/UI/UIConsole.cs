using System;
using System.Collections;
using System.Collections.Generic;
using MachinationsUP.Engines.Unity;
using MachinationsUP.ExampleGames.RubyAdventure2DBeginner;
using MachinationsUP.Integration.Elements;
using MachinationsUP.Integration.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class UIConsole : MonoBehaviour
{

    static public UIConsole Instance { get; private set; }

    public Text consoleText;

    /// <summary>
    /// List of items tracked by the console.
    /// </summary>
    private List<DiagramMapping> _trackedItems = new List<DiagramMapping>();

    // Use this for initialization
    void Awake ()
    {
        Instance = this;
        //Subscribe to update notifications from MGL.
        MachinationsGameLayer.OnMachinationsUpdate += OnMachinationsUpdate;
    }

    /// <summary>
    /// When <see cref="MachinationsUP.Engines.Unity.MachinationsGameLayer"/> gets some updated values,
    /// make sure that we update the console with any new info on <see cref="_trackedItems"/>.
    /// </summary>
    private void OnMachinationsUpdate (object sender, EventArgs e)
    {
        string text = "";
        foreach (DiagramMapping dm in _trackedItems)
        {
            ElementBase eb = MachinationsGameLayer.GetSourceElementBase(dm);
            text += dm.GameObjectName + "." + dm.GameObjectPropertyName + " = " + eb + "\r\n";
            //Update UI Health Bar of Ruby.
            if (dm.GameObjectName == "Ruby" && dm.GameObjectPropertyName == "Health")
            {
                UIHealthBar.Instance.SetNumericValue(eb.BaseValue);
                UIHealthBar.Instance.SetPercentValueOf1(eb.CurrentValue / (float) eb.BaseValue);
            }
        }
        consoleText.text = text;
    }

    /// <summary>
    /// Adds a new <see cref="MachinationsUP.Integration.Inventory.DiagramMapping"/> that the console should track and update on every update
    /// from <see cref="MachinationsUP.Engines.Unity.MachinationsGameLayer"/>.
    /// </summary>
    /// <param name="diagramMapping">Diagam Mapping to track. If it alraedy exists, it will be ignored.</param>
    public void AddTrackedItem (DiagramMapping diagramMapping)
    {
        if (!_trackedItems.Contains(diagramMapping))
            _trackedItems.Add(diagramMapping);
    }

}
