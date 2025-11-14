using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public class Skill
{
    [Tooltip("ID único da habilidade")]
    public string skillID;

    [Tooltip("Nome exibido da habilidade.")]
    public string skillName;

    [Tooltip("Descrição da habilidade.")]
    [TextArea(3, 5)]
    public string skillDescription;

    [Tooltip("Sprite de ícone para a habilidade.")]
    public Sprite skillIcon;

    [Tooltip("Indica se a habilidade já foi adquirida/ativada.")]
    public bool isLearned = false;

    [Header("Efeitos da Habilidade")]
    [Tooltip("Eventos disparados quando a habilidade é ativada.")]
    public UnityEvent OnSkillActivated;

    [Tooltip("Eventos disparados quando a habilidade é desativada.")]
    public UnityEvent OnSkillDeactivated;

    [Tooltip("Objetos a serem ativados quando a habilidade é aprendida/ativada.")]
    public List<GameObject> objectsToActivate;

    [Tooltip("Objetos a serem desativados quando a habilidade é aprendida/ativada.")]
    public List<GameObject> objectsToDeactivate;

    public Skill(string id, string name, string desc, Sprite icon)
    {
        skillID = id;
        skillName = name;
        skillDescription = desc;
        skillIcon = icon;
        isLearned = false; // Por padrão, não aprendida
    }

    /// <summary>
    /// Ativa os efeitos visuais e lógicos da habilidade.
    /// </summary>
    public void ActivateEffects()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null) obj.SetActive(true);
        }
        foreach (GameObject obj in objectsToDeactivate)
        {
            if (obj != null) obj.SetActive(false);
        }
        OnSkillActivated?.Invoke();
        Debug.Log($"Habilidade '{skillName}' ativada.");
    }

    /// <summary>
    /// Desativa os efeitos visuais e lógicos da habilidade.
    /// (Útil se a habilidade puder ser desativada ou removida)
    /// </summary>
    public void DeactivateEffects()
    {
        foreach (GameObject obj in objectsToDeactivate) 
        {
            if (obj != null) obj.SetActive(true);
        }
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null) obj.SetActive(false);
        }
        OnSkillDeactivated?.Invoke();
        Debug.Log($"Habilidade '{skillName}' desativada.");
    }
}