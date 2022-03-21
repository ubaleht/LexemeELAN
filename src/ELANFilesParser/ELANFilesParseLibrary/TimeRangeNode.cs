using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELANFilesParseLibrary
{
    public class TimeRangeNode
    {
        private string annotationId;
        private string timeSlotRef1;
        private string timeSlotRef2;
        private string annotationValue;

        public TimeRangeNode() { }

        public string AnnotationId { get => annotationId; set => annotationId = value; }
        public string TimeSlotRef1 { get => timeSlotRef1; set => timeSlotRef1 = value; }
        public string TimeSlotRef2 { get => timeSlotRef2; set => timeSlotRef2 = value; }
        public string AnnotationValue { get => annotationValue; set => annotationValue = value; }
    }
}
