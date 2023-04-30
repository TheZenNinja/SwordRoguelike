using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using InventorySystem;

//https://odininspector.com/tutorials/how-to-create-custom-drawers-using-odin/how-to-create-a-custom-value-drawer

public class WeaponStatGUI : OdinValueDrawer<WeaponStat>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        Rect container = EditorGUILayout.GetControlRect();
        WeaponStat wep = ValueEntry.SmartValue;

        EditorGUI.LabelField(container.AlignLeft(container.width * .2f), "Stat Type:");
        wep.statType = (WeaponStat.StatType)EditorGUI.EnumPopup(
            container.AlignLeft(container.width * .3f).AddX(container.width * .15f), wep.statType);
        
        //ignore flat status chance
        if (wep.statType == WeaponStat.StatType.statusChance)
        {
            wep.isFlatValue = false;
        }
        else
        {
            EditorGUI.LabelField(container.AlignRight(container.width * .5f), "Flat Val:");
            wep.isFlatValue = EditorGUI.Toggle(container.AlignRight(container.width * .4f), wep.isFlatValue);
        }
        
        EditorGUI.LabelField(new Rect(container).SetX(container.width * .8f), "Value:");
        wep.value = EditorGUI.FloatField(container.AlignRight(container.width * .2f), wep.value);

        ValueEntry.SmartValue = wep;
    }
}
