using System.Collections.Generic;

namespace Rogue.Stats
{
    public interface IModifierProvider
    {
       IEnumerable<float> GetAdditiveModifiers(Stats stat);
       IEnumerable<float> GetPercentageModifiers(Stats stat);
    }
}

/*
 public IEnumerable<float> GetAdditiveModifiers(Stats stat){
    if(stat==stats.damage)
        yield return 1st weapon;
        yield return 2nd weapon;
    }
     */
