using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Handels the text of the ability selection pop up
/// </summary>
public class AbilitySelectionManager : MonoBehaviour {
    [SerializeField] private TMP_Text _topHeader;
    [SerializeField] private TMP_Text _topBody;
    [SerializeField] private TMP_Text _botHeader;
    [SerializeField] private TMP_Text _botBody;
    [SerializeField] private GameObject _selectionDisplay;

    private TowerBase _currentTarget;
    private List<Ability> _currentAbilitySelection;

    /// <summary>
    /// Shows and fills the pop-up with the correct text
    /// </summary>
    /// <param name="statType">What stat has passed the level up threshold</param>
    /// <param name="statAmount">How many points the stat is now</param>
    /// <param name="targetTower">The reference to the tower</param>
    public void HandleAbilitySelection(StatType statType, int statAmount, TowerBase targetTower) {
        _selectionDisplay.SetActive(true);
        var abilityDistribution = ResourceSystem.Instance.GetScriptableAbilityDistribution();
        _currentAbilitySelection = abilityDistribution.GetAbilityList(statType, statAmount);
        _topHeader.text = _currentAbilitySelection[0].Name;
        _topBody.text = _currentAbilitySelection[0].Description;
        _botHeader.text = _currentAbilitySelection[1].Name;
        _botBody.text = _currentAbilitySelection[1].Description;
        _currentTarget = targetTower;
    }

    /// <summary>
    /// Disables the pop-up and applies the effect ot the unit 
    /// </summary>
    /// <param name="selectionNumber"></param>
    public void AbilitySelected(int selectionNumber) {
        _currentTarget.DistributeEffects(_currentAbilitySelection[selectionNumber].Effects, _currentTarget);
        _selectionDisplay.SetActive(false);
    }
}
