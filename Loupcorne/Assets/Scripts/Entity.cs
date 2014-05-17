using UnityEngine;
using System.Collections;

public class Entity : SimObjectWrapper 
{
    [SerializeField] protected float _healthBarUpOffset;
    [SerializeField] protected float _health;
    [SerializeField] private Texture _healthBarTexture;
    [SerializeField] private Texture _healthBarBgTexture;
    
    protected float _maxHealth;
    protected bool isAlive;
    
    public float Health
    {
        get
        {
            return _health;
        }
    }

    public float MaxHealth
    {
        get
        {
            return _maxHealth;
        }
    }

    public LoupCorne.Framework.SimDescriptor DifficultyDescriptor
    {
        get;
        set;
    }

	protected virtual void Start () 
    {
        _health = (float)this.GetPropertyValue(SimProperties.MaxHealth);
        this._maxHealth = this._health;
        //Debug.Log(this._health + "/" + this._maxHealth);

        // Apply difficulty.
        if (this.DifficultyDescriptor != null)
        {
            this.AddDescriptor(this.DifficultyDescriptor);
            this.Refresh();
        }

        if (this is Player)
        {
            Debug.LogWarning("Player.base.Start");
        }

        isAlive = true;
	}
	
	void Update () 
    {
	    //TODO inputs ?
	}

	public virtual void Hit(float hitStrength)
	{
        float damageReductionRation = 1f;
        if (this is Player)
        {
            damageReductionRation = (float)this.GetPropertyValue(SimProperties.DamageReductionRatio);
        }

        if (isAlive)
        {
            _health -= hitStrength * damageReductionRation;
            if (_health <= 0)
                Kill();
        }
    }

    public virtual void Kill()
    {
        if (!(this is Player))
        {
            Player player = UnitsManager.Instance.player;
            double alignmentReward = this.GetPropertyValue(SimProperties.AlignmentRewardOnDeath);
            double currentPlayerAlignment = player.GetPropertyValue(SimProperties.Alignement);
            player.SetPropertyBaseValue(SimProperties.Alignement, currentPlayerAlignment + alignmentReward);
            player.Refresh();
        }

        GameObject.Instantiate(Resources.Load(@"Sfx/Wilhlem"), this.transform.position, this.transform.rotation);
        isAlive = false;
    }

    protected virtual void OnGUI()
    {
        Vector3 point = Camera.main.WorldToScreenPoint(this.transform.position + this.transform.up * _healthBarUpOffset);
        //GUI.Box(new Rect(point.x - 20f, Screen.height - point.y, 40f * Mathf.Clamp01(this._health / this._maxHealth), 10f), string.Empty);
        GUI.DrawTexture(new Rect(point.x - 21f, Screen.height - point.y, 42f, 10f), this._healthBarBgTexture);
        GUI.DrawTexture(new Rect(point.x - 20f, Screen.height - point.y + 1, 40f * Mathf.Clamp01(this._health / this._maxHealth), 8f), this._healthBarTexture);
    }
}
