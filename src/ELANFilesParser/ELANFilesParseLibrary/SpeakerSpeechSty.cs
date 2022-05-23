using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELANFilesParseLibrary
{
    public class SpeakerSpeechSty : SpeakerSpeech
    {
        private List<SpeakerWordSty> speakerWordsSty;
        private string speakerSpeechTrTranslation;
        private string speakerSpeechLatin;

        public SpeakerSpeechSty() : base() { }

        public List<SpeakerWordSty> SpeakerWordsSty { get => speakerWordsSty; set => speakerWordsSty = value; }
        public string SpeakerSpeechTrTranslation { get => speakerSpeechTrTranslation; set => speakerSpeechTrTranslation = value; }
        public string SpeakerSpeechLatin { get => speakerSpeechLatin; set => speakerSpeechLatin = value; }
    }
}
