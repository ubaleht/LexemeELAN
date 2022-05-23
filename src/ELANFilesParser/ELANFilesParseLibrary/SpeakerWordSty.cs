using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELANFilesParseLibrary
{
    public class SpeakerWordSty : SpeakerWord
    {
        private string speakerWordTrTranslation;
        private string speakerWordLatin;

        public SpeakerWordSty() : base () {}

        public string SpeakerWordTrTranslation { get => speakerWordTrTranslation; set => speakerWordTrTranslation = value; }
        public string SpeakerWordLatin { get => speakerWordLatin; set => speakerWordLatin = value; }
    }
}
