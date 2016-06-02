using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.git
{
    public interface IDestructible
    {
        /// <summary>
        /// Causes this IDestructrible the specified amount of damage
        /// </summary>
        /// <param name="damage">The damage to cause</param>
        /// <param name="author">The author of the damage</param>
        /// <returns>This IDestructible's life after the taken damage</returns>
        float damage(float damage, IPlayer author);

        /// <summary>
        /// The Life level of this IDestructible
        /// </summary>
        float life { get; set; }
    }
}
