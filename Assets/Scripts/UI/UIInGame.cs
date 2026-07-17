using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;
    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordThrowImage;
    [SerializeField] private Image blackHoleImage;
    [SerializeField] private Image flaskImage;

    [SerializeField] private TextMeshProUGUI currentSouls;

    //[SerializeField] private float dashCooldown;
    private SkillManager skillManager;

    private void Start()
    {
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;

        //dashCooldown = SkillManager.instance.dash.cooldown;
        skillManager = SkillManager.instance;
    }

    //后续可以进行优化，过于冗余了;同时关于flask部分可以基于自己的思路进行优化,同时对于冷却时机的判定需要重新设置
    private void Update()
    {
        currentSouls.text = PlayerManager.instance.GetCurrency().ToString("#,#");

        if (Input.GetKeyDown(KeyCode.LeftShift) && skillManager.dash.dashUnlocked)
            SetCooldwonOf(dashImage);
        
        if (Input.GetKeyDown(KeyCode.Q) && skillManager.parry.parryUnlocked)
            SetCooldwonOf(parryImage);

        if (Input.GetKeyDown(KeyCode.F) && skillManager.crystal.crystalUnlocked)
            SetCooldwonOf(crystalImage);
        
        if (Input.GetKeyDown(KeyCode.Mouse1) && skillManager.sword.swordUnlocked)
            SetCooldwonOf(swordThrowImage);

        if (Input.GetKeyDown(KeyCode.R) && skillManager.blackHole.blackHoleUnlocked)
            SetCooldwonOf(blackHoleImage);

        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldwonOf(flaskImage);

        CheckCooldownOf(dashImage, skillManager.dash.cooldown);
        CheckCooldownOf(parryImage, skillManager.parry.cooldown);
        CheckCooldownOf(crystalImage, skillManager.crystal.cooldown);
        CheckCooldownOf(swordThrowImage, skillManager.sword.cooldown);
        CheckCooldownOf(blackHoleImage, skillManager.blackHole.cooldown);

        CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    private void SetCooldwonOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }
}
