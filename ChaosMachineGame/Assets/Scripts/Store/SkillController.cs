using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SkillController : MonoBehaviour
{
    public static SkillController Instance { get; private set; }

    [Header("Habilidades Disponíveis")]
    [Tooltip("Lista de todas as habilidades gerenciadas por este controlador.")]
    public List<Skill> availableSkills = new List<Skill>();

    private const string SKILL_PREFIX = "SkillLearned_";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        LoadSkillStates(); 

        foreach (Skill skill in availableSkills)
        {
            if (skill.isLearned)
            {
                skill.ActivateEffects();
            }
        }
    }

    /// <summary>
    /// Carrega o estado 'isLearned' de todas as habilidades salvas em PlayerPrefs.
    /// </summary>
    private void LoadSkillStates()
    {
        foreach (Skill skill in availableSkills)
        {
            string prefKey = SKILL_PREFIX + skill.skillID;
            skill.isLearned = PlayerPrefs.GetInt(prefKey, 0) == 1;

            if (skill.isLearned)
            {
                Debug.Log($"Estado carregado: Habilidade '{skill.skillName}' é aprendida.");
            }
        }
    }

    /// <summary>
    /// Salva o estado 'isLearned' de uma habilidade específica em PlayerPrefs.
    /// </summary>
    /// <param name="skillID">O ID da habilidade cujo estado será salvo.</param>
    private void SaveSkillState(string skillID)
    {
        Skill skill = GetSkillByID(skillID);
        if (skill != null)
        {
            string prefKey = SKILL_PREFIX + skill.skillID;
            PlayerPrefs.SetInt(prefKey, skill.isLearned ? 1 : 0);
            PlayerPrefs.Save(); 
            Debug.Log($"Estado salvo: Habilidade '{skill.skillName}' é {(skill.isLearned ? "aprendida" : "não aprendida")}.");
        }
    }

    /// <summary>
    /// Tenta aprender uma habilidade específica.
    /// </summary>
    public void TryToLearn(string skillID) { LearnSkill(skillID); }

    /// <summary>
    /// Tenta aprender uma habilidade específica.
    /// Se a habilidade já foi aprendida, nada acontece.
    /// </summary>
    /// <param name="skillID">O ID da habilidade a ser aprendida.</param>
    /// <returns>True se a habilidade foi aprendida (ou já estava aprendida), false se não encontrada.</returns>
    public bool LearnSkill(string skillID)
    {
        Skill skillToLearn = GetSkillByID(skillID);

        if (skillToLearn == null)
        {
            Debug.LogWarning($"SkillController: Habilidade com ID '{skillID}' não encontrada.");
            return false;
        }

        if (skillToLearn.isLearned)
        {
            Debug.Log($"SkillController: Habilidade '{skillToLearn.skillName}' já foi aprendida.");
            return true;
        }

        skillToLearn.isLearned = true;
        skillToLearn.ActivateEffects();
        SaveSkillState(skillID); 
        Debug.Log($"SkillController: Habilidade '{skillToLearn.skillName}' aprendida com sucesso!");
        return true;
    }

    /// <summary>
    /// Tenta desaprender uma habilidade específica.
    /// </summary>
    /// <param name="skillID">O ID da habilidade a ser desaprendida.</param>
    /// <returns>True se a habilidade foi desaprendida (ou já não estava aprendida), false se não encontrada.</returns>
    public bool UnlearnSkill(string skillID)
    {
        Skill skillToUnlearn = GetSkillByID(skillID);

        if (skillToUnlearn == null)
        {
            Debug.LogWarning($"SkillController: Habilidade com ID '{skillID}' não encontrada.");
            return false;
        }

        if (!skillToUnlearn.isLearned)
        {
            Debug.Log($"SkillController: Habilidade '{skillToUnlearn.skillName}' já não está aprendida.");
            return true;
        }

        skillToUnlearn.isLearned = false;
        skillToUnlearn.DeactivateEffects();
        SaveSkillState(skillID); 
        Debug.Log($"SkillController: Habilidade '{skillToUnlearn.skillName}' desaprendida.");
        return true;
    }

    /// <summary>
    /// Verifica se uma habilidade está aprendida.
    /// </summary>
    /// <param name="skillID">O ID da habilidade a ser verificada.</param>
    /// <returns>True se a habilidade foi aprendida, false caso contrário ou se não encontrada.</returns>
    public bool IsSkillLearned(string skillID)
    {
        Skill skill = GetSkillByID(skillID);
        return skill != null && skill.isLearned;
    }

    /// <summary>
    /// Retorna uma habilidade pelo seu ID.
    /// </summary>
    /// <param name="skillID">O ID da habilidade.</param>
    /// <returns>A Skill encontrada, ou null se não existir.</returns>
    public Skill GetSkillByID(string skillID)
    {
        return availableSkills.FirstOrDefault(skill => skill.skillID == skillID);
    }

    /// <summary>
    /// Executa o UnityEvent de uma habilidade APENAS se ela estiver aprendida.
    /// </summary>
    /// <param name="skillID">O ID da habilidade cujo evento deve ser invocado.</param>
    public void InvokeSkillEventIfLearned(string skillID)
    {
        Skill skill = GetSkillByID(skillID);
        if (skill != null && skill.isLearned)
        {
            skill.OnSkillActivated?.Invoke();
            Debug.Log($"Evento da habilidade '{skill.skillName}' invocado.");
        }
        else if (skill == null)
        {
            Debug.LogWarning($"SkillController: Tentativa de invocar evento de habilidade com ID '{skillID}' que não existe.");
        }
        else
        {
            Debug.Log($"SkillController: Habilidade '{skill.skillName}' não está aprendida, evento não invocado.");
        }
    }

    /// <summary>
    /// Limpa todos os dados de habilidades salvas no PlayerPrefs.
    /// CUIDADO: Isso irá "desaprender" todas as habilidades permanentemente até que sejam compradas novamente.
    /// </summary>
    public void ResetAllSkillStates()
    {
        foreach (Skill skill in availableSkills)
        {
            string prefKey = SKILL_PREFIX + skill.skillID;
            if (PlayerPrefs.HasKey(prefKey))
            {
                PlayerPrefs.DeleteKey(prefKey);
            }
            skill.isLearned = false; 
            skill.DeactivateEffects(); 
        }
        PlayerPrefs.Save();
        Debug.Log("Todos os estados de habilidades foram resetados no PlayerPrefs.");
    }
}