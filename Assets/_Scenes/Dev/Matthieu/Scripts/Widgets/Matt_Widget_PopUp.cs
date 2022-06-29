using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Matt_Widget_PopUp : Matt_Widgets
{
    [Header("OBJECT")]
    [SerializeField] protected GameObject _popUp;

    [Header("TEXT")]
    [SerializeField] protected TextMeshProUGUI _textTitle;
    [SerializeField] protected TextMeshProUGUI _textDesc;

    /// <summary>
    /// Met a jour les informations du popUp
    /// </summary>
    /// <param name="title"></param>
    /// <param name="desc"></param>
    public void SetPopUp(string title, string desc)
    {
        _textTitle.text = title;
        _textDesc.text = desc;
    }

    /// <summary>
    /// Affiche une pop-up sur la souris avec les information entrer
    /// </summary>
    public void DisplayPopUp(Vector3 position, string description = " ", string title = "Information")
    {

        // Si le popUp est initialiser, l'affiche et change son texte
        if (_popUp != null)
        {
            // Deplace le popUp a l'endroit indiquer
            _popUp.transform.position = position;

            // Recupere la transform du popUp
            Transform parent = _popUp.transform;
            // Change le titre du PopUp
            ModifyTextBox(parent, "Title", title);
            // Change la description du PopUp
            ModifyTextBox(parent, "Description", description);

            if (_popUp.activeSelf == false) { _popUp.SetActive(true); }
        }

    }

    /// <summary>
    /// Affiche une pop-up sur la souris avec les information entrer
    /// </summary>
    private void DisplayPopUp(DataCharacter.Capacity data, Vector3 position, string description = " ", string title = "Information")
    {
        if (hudManager.Character.IsMoving) return; // [UI] [GRID] Empêcher la prévisualisation quand notre perso se déplace

        PlayerController playerController = hudManager.ActualPlayer;

        GridManager.ResetCasesPreview(playerController.GetCurrentCharactedSelected.CurrentCase.GridParent);

        if (data.typeA == ActionTypeMode.Overwatch)
        {
            playerController.GetCurrentCharactedSelected.PreviewOverwatch();
            playerController.IsPreviewing = true;
        }
        else
        {
            DataWeapon weapon = playerController.GetWeaponFromActionMode(data.typeA);
            if (data.typeA != ActionTypeMode.Reload)
                playerController.GetCurrentCharactedSelected.AttackRange(weapon, weapon.Range.casePreviewRange);

            playerController.IsPreviewing = true;
        }


        // Si le popUp est initialiser, l'affiche et change son texte
        if (_popUp != null)
        {
            // Deplace le popUp a l'endroit indiquer
            _popUp.transform.position = position;
            DataWeapon weapon = playerController.GetWeaponFromActionMode(data.typeA);
            PopupActionBar _popup = _popUp.GetComponent<PopupActionBar>();
            _popup.SetWidget(title, description, weapon.CostPoint.ToString(), weapon.Cooldown);

            if (_popUp.activeSelf == false) { _popUp.SetActive(true); }
        }

    }

    /// <summary>
    /// Met a jour la boite de texte enfant d'un GameObject
    /// </summary>
    /// <param name="nameChild">Enfant a chercher du GameObject</param>
    /// <param name="valueString">Chaine de charactere a inserer</param>
    public void ModifyTextBox(Transform parent, string nameChild, string valueString)
    {
        TextMeshProUGUI textMesh;
        Text text;
        Transform textBox;

        // -- Initialise le texte de la boite de texte de "Description" -- //
        textBox = parent.Find(nameChild);
        // Si le composant text est present alors change le texte
        if (textBox.TryGetComponent<TextMeshProUGUI>(out textMesh))
        {
            textMesh.text = valueString;
        }
        else if (textBox.TryGetComponent<Text>(out text))
        {
            text.text = valueString;
        }
    }

    /// <summary>
    /// Si l'objet PopUp est assigne, le desactive si il est actif
    /// </summary>
    public void HidePopUp()
    {
        // Si le popUp est initialiser, le cache
        if (_popUp != null)
        {
            if (_popUp.activeSelf == true) { _popUp.SetActive(false); }
        }

        if (hudManager.ActualPlayer.GetCurrentCharactedSelected != null && !hudManager.ActualPlayer.GetCurrentCharactedSelected.IsMoving)
        {
            hudManager.ActualPlayer.IsPreviewing = false;
            GridManager.ResetCasesPreview(hudManager.ActualPlayer.GetCurrentCharactedSelected.CurrentCase.GridParent);
        }
    }
}
