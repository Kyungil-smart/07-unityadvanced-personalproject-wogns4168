public abstract class MonsterBase : Health, ITargetable
{
    public abstract string Name { get; }
    public abstract void Act();
    public abstract void Reward();

    // ITargetable 기본 구현 (추후 하위클래스에서 변경가능하도록 virtual)
    public virtual void OnSelect() {  }
    public virtual void OnHoverEnter() {  }
    public virtual void OnHoverExit() {  }

    public override void Die()
    {
        base.Die();
        Reward();
    }
}