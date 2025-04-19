public abstract class StatusEffect
{
    public Status status;
    public float statusTime;
    public bool completed = false;

    private float _timer = 0f;

    protected StatusEffect(Status s, float t)
    {
        status = s;
        statusTime = t;
    }

    public virtual void Refresh()
    {
        _timer = 0f;
        completed = false;
    }

    public abstract StatusEffect Clone();

    protected abstract bool CanApply(Enemy enemy);

    public bool Apply(Enemy enemy)
    {
        if (CanApply(enemy))
        {
            _timer = 0f;
            completed = false;
            OnApply(enemy);
            return true;
        }

        return false;
    }

    public void Update(Enemy enemy, float deltaTime)
    {
        if (completed)
            return;
        _timer += deltaTime;
        OnUpdate(enemy,  deltaTime);
        if (_timer >= statusTime)
        {
            completed = true;
            OnComplete(enemy);
        }
    }

    protected abstract void OnApply(Enemy enemy);
    protected abstract void OnUpdate(Enemy enemy, float deltaTime);
    protected abstract void OnComplete(Enemy enemy);
    
    public override string ToString()
    {
        return $"{status} ({statusTime:F2}s)";
    } 
}