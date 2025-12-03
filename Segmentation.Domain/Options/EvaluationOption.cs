using System;
using System.Collections.Generic;
using System.Text;

namespace Segmentation.Domain.Options
{
    public class EvaluationOption
    {
        //Disabled by default due to limit of supported operations
        public bool UseTypeMissmatchWapper { get; set; } = false;
    }
}
