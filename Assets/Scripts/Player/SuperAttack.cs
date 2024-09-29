using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SuperAttack : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float cooldownTime = 2f;
    [SerializeField] private Button superAttackButton; 
    [SerializeField] private Image cooldownImage; 

    private bool isCooldown = false;

    private void Start()
    {
        cooldownImage.fillAmount = 1;
        superAttackButton.onClick.AddListener(PerformSuperAttack);
    }

    private void PerformSuperAttack()
    {
        if (isCooldown == false)
        {
            player.SuperAttack();
            StartCoroutine(CooldownRoutine());
        }
    }
    private void Update()
    {
        superAttackButton.interactable = player.CheckEnemiesInRange();
    }
    private IEnumerator CooldownRoutine()
    {
        isCooldown = true;
        cooldownImage.fillAmount = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < cooldownTime)
        {
            elapsedTime += Time.deltaTime;
            cooldownImage.fillAmount = elapsedTime / cooldownTime; 
            yield return null;
        }

        cooldownImage.fillAmount = 1f;
        isCooldown = false;
    }
}
