using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELANFilesParseLibrary
{
    public class AssociationNode
    {
        private string annotationId;
        private string annotationRef;
        private string annotationValue;

        public AssociationNode() { }

        public string AnnotationId { get => annotationId; set => annotationId = value; }
        public string AnnotationRef { get => annotationRef; set => annotationRef = value; }
        public string AnnotationValue { get => annotationValue; set => annotationValue = value; }
    }
}
