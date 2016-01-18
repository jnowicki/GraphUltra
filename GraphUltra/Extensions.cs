using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace GraphUltra
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the radius of this circle (ellipse is assumed to be round).
        /// </summary>
        /// <param name="circle">The circle</param>
        /// <returns>Radius</returns>
        public static double GetRadius(this Ellipse circle)
        {
            return circle.Width / 2;
        }
    }
}
