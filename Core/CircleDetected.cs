using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class CircleDetected: Exception
    {
        public CircleDetected(): base("Circle is detected without `-r' option."){}
    }
}
