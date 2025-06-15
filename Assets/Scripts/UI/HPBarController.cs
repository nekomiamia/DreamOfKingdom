using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;


public class HPBarController : MonoBehaviour
{
    private CharacterBase curCharacter;
    private VisualElement defenseElement;
    private Label defenseLabel; 
    [Header("Elements")] public Transform HpBarTransform;
    private UIDocument hpBarDocument;
    private ProgressBar healthBar;
    private VisualElement intentElement;
    private Label intenLabel;

    private void Awake()
    {
        curCharacter = GetComponent<CharacterBase>();
    }

    private void Start()
    {
        InitHealthBar();
    }

    private void MoveToWorldPosition(VisualElement element, Vector3 worldPosition, Vector2 size)
    {
        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPosition, size, Camera.main);
        element.transform.position = rect.position;
    }
    
    [ContextMenu("Test Update Health Bar")]
    public void InitHealthBar()
    {
        hpBarDocument = GetComponent<UIDocument>();
        healthBar = hpBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");
        healthBar.highValue = curCharacter.MaxHP;
        MoveToWorldPosition(healthBar, HpBarTransform.position, Vector2.zero);
        defenseElement = healthBar.Q<VisualElement>("DefenseElement");
        defenseLabel = defenseElement.Q<Label>("DefenseLabel");
        
        defenseElement.style.display = DisplayStyle.None;
        
        intentElement = hpBarDocument.rootVisualElement.Q<VisualElement>("IntentElement");
        intenLabel = intentElement.Q<Label>("IntentLabel");
        intentElement.style.display = DisplayStyle.None;
        
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (curCharacter.isDead)
        {
            healthBar.style.display = DisplayStyle.None;
            return;
        }

        if (healthBar != null)
        {
            healthBar.title = $"{curCharacter.CurHP}/{curCharacter.MaxHP}";
            healthBar.value = curCharacter.CurHP;
            
            healthBar.RemoveFromClassList("highHealth");
            healthBar.RemoveFromClassList("mediumHealth");
            healthBar.RemoveFromClassList("lowHealth");

            var percentage = (float)curCharacter.CurHP / (float)curCharacter.MaxHP;
            if (percentage < 0.3f)
            {
                healthBar.AddToClassList("lowHealth");
            }else if (percentage < 0.6f)
            {
                healthBar.AddToClassList("mediumHealth");
            }
            else
            {
                healthBar.AddToClassList("highHealth");
            }
            
            // Defense UI Update
            defenseElement.style.display = curCharacter.defense.curValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            defenseLabel.text = curCharacter.defense.curValue.ToString();
            
        }
    }

    public void SetIntentElement()
    {
        var enemy = curCharacter as Enemy;
        intentElement.style.display = DisplayStyle.Flex;
        intentElement.style.backgroundImage = new StyleBackground(enemy.curAction.intentSprite);

        var value = enemy.curAction.effect.value;
        if ( enemy.curAction.effect.GetType() == typeof(DamageEffect))
        {
            value = (int) math.round(enemy.curAction.effect.value * enemy.damageMultiplier);
        }

        intenLabel.text = value.ToString();
    }
    
}
