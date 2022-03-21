using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELANFilesParseLibrary
{
    public class SpeakerSpeech
    {
        private string speakerSpeechText;
        private int startTime;
        private int endTime;
        private List<SpeakerWord> speakerWords;
        private string speakerSpeechEnTranslation;
        private string speakerSpeechRuTranslation;

        public SpeakerSpeech()
        {
            this.speakerWords = new List<SpeakerWord>();
        }

        public string SpeakerSpeechText { get => speakerSpeechText; set => speakerSpeechText = value; }
        public List<SpeakerWord> SpeakerWords { get => speakerWords; set => speakerWords = value; }
        public int StartTime { get => startTime; set => startTime = value; }
        public int EndTime { get => endTime; set => endTime = value; }
        public string SpeakerSpeechEnTranslation { get => speakerSpeechEnTranslation; set => speakerSpeechEnTranslation = value; }
        public string SpeakerSpeechRuTranslation { get => speakerSpeechRuTranslation; set => speakerSpeechRuTranslation = value; }
    }
}
