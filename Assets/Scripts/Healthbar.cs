using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public RectTransform HealthBarRect;

    public Image HealthBarImage;

    public Color Health100;
    public Color Health65;
    public Color Health35;

    private RectTransform _canvas;

    private ITargetable _user;
    private float _offset;


    public void SetHealthBar(ITargetable user,RectTransform healthBarPanel, float heightOffset)
    {
        _canvas = healthBarPanel;
        _user = user;
        _user.Damaged += OnDamaged;
        _offset = heightOffset;
        UpdateHealthBar();

    }

    private void Update()
    {
        UpdateHealthBar();
    }

    private void OnDamaged(float health)
    {
        float percentage = health / _user.MaxHealth;
        HealthBarImage.fillAmount = percentage;
        
        //Set health bar color based on health percentage
        if (percentage > 0.65f)
        {
            HealthBarImage.color = Health100;
        }
        if (percentage < 0.65f)
        {
            HealthBarImage.color = Health65;
        }
        if (percentage < 0.35f)
        {
            HealthBarImage.color = Health35;
        }


    }

    private void OnDestroyed()
    {
        //Cleanup
        _user.Damaged -= OnDamaged;
        _user.Destroyed -= OnDestroyed;

        //Could potentialy use a object pool and send this health bar back to the pool. Would 100% be more efficient, but would take longer.
        Destroy(gameObject);
    }


    private void UpdateHealthBar()
    {
        if (_user == null || _user.Equals(null))
        {
            OnDestroyed();
            return;
        }

        //Convert the position of the entity to a viewpoint
        Vector2 viewport = Camera.main.WorldToViewportPoint(_user.GetTransform().position);
        
        //Calculate the position of the health bar to be above the entity.
        Vector2 position = new Vector2(
        ((viewport.x * _canvas.sizeDelta.x) - (_canvas.sizeDelta.x * 0.5f)),
        ((viewport.y * _canvas.sizeDelta.y) - (_canvas.sizeDelta.y * 0.5f)) + _offset);


        HealthBarRect.anchoredPosition = position;
    }
}
