public abstract class MonsterBase : Health, ITargetable
{
    public abstract string Name { get; }
    public abstract void Act();
    public abstract void Reward();

    // ITargetable 기본 구현
    public virtual void OnSelect() {  }
    public virtual void OnHoverEnter() {  }
    public virtual void OnHoverExit() {  }

    protected override void Die()
    {
        base.Die();
        Reward();
    }
}