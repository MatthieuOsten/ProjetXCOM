using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Matt_Widget_PopUpAction : Matt_Widget_PopUp
{

    [Space]
    [Header("ACTION POINT")]
    [SerializeField] private TextMeshProUGUI _textCostAction;
    [Space]
    [Header("COOLDOWN")]
    [SerializeField] private Image _imageCooldown;
    [SerializeField] private TextMeshProUGUI _textCooldown;

    /// <summary>
    /// Met a jour les informations du popUp
    /// </summary>
    /// <param name="title"></param>
    /// <param name="desc"></param>
    /// <param name="costAction"></param>
    /// <param name="cooldownBetweenUse"></param>
    public void SetPopUp(string title, string desc, string costAction, int cooldownBetweenUse = 0)
    {
        if (cooldownBetweenUse == 0)
        {
            _textCooldown.color = Color.clear;
            _imageCooldown.color = Color.clear;
        }
        else
        {
            _textCooldown.text = cooldownBetweenUse.ToString();
            _textCooldown.color = Color.white;
            _imageCooldown.color = Color.white;
        }
        _textTitle.text = title;
        _textDesc.text = desc;
        _textCostAction.text = costAction;

    }

    /// <summary>
    /// Affiche une pop-up sur la souris avec les information entrer
    /// </summary>
    public void DisplayPopUp(DataCharacter.Capacity data, Vector3 position, string description = " ", string title = "Information")
    {
        if (hudManager.Character.IsMoving) return; // [UI] [GRID] Emp�cher la pr�visualisation quand notre perso se d�place

        GridManager.ResetCasesPreview(hudManager.ActualPlayer.GetCurrentCharactedSelected.CurrentCase.GridParent);

        if (data.typeA == ActionTypeMode.Overwatch)
        {
            hudManager.ActualPlayer.GetCurrentCharactedSelected.PreviewOverwatch();
            hudManager.ActualPlayer.IsPreviewing = true;
        }
        else
        {
            DataWeapon weapon = hudManager.ActualPlayer.GetWeaponFromActionMode(data.typeA);
            if (data.typeA != ActionTypeMode.Reload)
                hudManager.ActualPlayer.GetCurrentCharactedSelected.AttackRange(weapon, weapon.Range.casePreviewRange);

            hudManager.ActualPlayer.IsPreviewing = true;
        }


        // Si le popUp est initialiser, l'affiche et change son texte
        if (_popUp != null)
        {
            // Deplace le popUp a l'endroit indiquer
            _popUp.transform.position = position;
            DataWeapon weapon = hudManager.ActualPlayer.GetWeaponFromActionMode(data.typeA);
            PopupActionBar _popup = _popUp.GetComponent<PopupActionBar>();
            _popup.SetWidget(title, description, weapon.CostPoint.ToString(), Color.white ,weapon.Cooldown);

            if (_popUp.activeSelf == false) { _popUp.SetActive(true); }
        }

    }

}