
using System.Collections;

public interface IStatusEffect
{
    public void ApplyStatusEffect(StatusEffectObj data);

    public void RemoveEffect();

    public IEnumerator BurnEffect();
}
