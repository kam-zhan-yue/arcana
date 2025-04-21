using Kuroneko.UtilityDelivery;
using UnityEngine;

public abstract class StatusEffect
{
    public Status status;
    public float statusTime;
    public bool completed = false;

    private float _timer = 0f;
    private ParticleSystem _particles;

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
            // At this point, status should be confirmed
            if (status != Status.None)
            {
                SpawnParticles(enemy);
            }
            return true;
        }

        return false;
    }

    private void SpawnParticles(Enemy enemy)
    {
        StatusParticle particles = ServiceLocator.Instance.Get<IGameManager>().GetGame().Database.settings
            .GetParticleByStatus(status); 
        _particles = Object.Instantiate(particles.particles, enemy.transform);
        _particles.transform.localPosition = particles.offset;
        _particles.Play();
    }

    public void Update(Enemy enemy, float deltaTime)
    {
        if (completed)
            return;
        _timer += deltaTime;
        OnUpdate(enemy,  deltaTime);
        if (_timer >= statusTime)
        {
            Complete(enemy);
        }
    }

    public void Complete(Enemy enemy)
    {
        if (completed) return;
        if (_particles)
            _particles.Stop();
        completed = true;
        OnComplete(enemy);
    }

    protected abstract void OnApply(Enemy enemy);
    protected abstract void OnUpdate(Enemy enemy, float deltaTime);
    protected abstract void OnComplete(Enemy enemy);
    
    public override string ToString()
    {
        return $"{status} ({statusTime:F2}s)";
    } 
}