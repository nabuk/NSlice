using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSliceTests.Dto
{
    public class SliceTestCase
    {
        public int? From { get; set; }
        public int? To { get; set; }
        public int Step { get; set; }
        public int Length { get; set; }
    }
}
