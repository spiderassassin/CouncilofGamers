using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ParolePileofSkulls : FlammableEntity
{
    public GameObject waveManager;
    public GameObject prompt;
    public GameObject NPCSprite;
    private TextMeshProUGUI promptText;

    protected override void Start()
    {
        base.Start();
        promptText = prompt.GetComponent<TextMeshProUGUI>();

        // If the game stage is past the tutorial, destroy the pile of skulls.
        if (GameManager.Instance.gameStage > GameManager.GameStage.TutorialPunchWave)
        {
            Destroy(gameObject);
            // Hide the parole guard.
            NPCSprite.GetComponent<paroleAnimations>().hide();
        }
    }

    protected override void Update()
    {
        // base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If player gets close to the pile of skulls, show a prompt telling them to punch it.
        // Only displayed if we're in the first downtime stage.
        if (other.gameObject.GetComponent<Controller>() != null && GameManager.Instance.gameStage == GameManager.GameStage.Downtime1)
        {
            prompt.SetActive(true);
            promptText.text = "Punch the pile of skulls";
        }
    }

    public override void OnDamaged(IAttacker attacker, DamageInformation dmg)
    {

        if (dmg.type == DamageType.AdditiveDamage && attacker is Controller)
        {
            base.OnDamaged(attacker, dmg);
        }
    }

    public override void Death()
    {
        base.Death();
        SoundManager.Instance.PlayOneShot(FMODEvents.Instance.punchImpact, transform.position);

        //fill fuel to full
        while (GameManager.Instance.fuel != GameManager.Instance.GetMaxFuel())
        {
            GameManager.Instance.UpdateFuel(false, false, true);
        }
        waveManager.GetComponent<WaveManager>().tutorialSkullPilePunched = true;

        prompt.SetActive(false);

        WaveManager.Instance.triggerAreaForParoleDialogue.SetActive(true);

        Destroy(gameObject);
    }
}
