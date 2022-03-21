using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELANFilesParseLibrary
{
    public class SpeakerWord
    {
        private string speakerWordText;
        private int startTime;
        private int endTime;
        private string speakerWordsEnTranslation;
        private string speakerWordsRuTranslation;
        private string speakerWordsComments;
        private string speakerSpeechLanguage;
        private string speakerPartOfSpeech;

        public SpeakerWord() { }

        public string SpeakerWordText { get => speakerWordText; set => speakerWordText = value; }
        public int StartTime { get => startTime; set => startTime = value; }
        public int EndTime { get => endTime; set => endTime = value; }
        public string SpeakerWordsEnTranslation { get => speakerWordsEnTranslation; set => speakerWordsEnTranslation = value; }
        public string SpeakerWordsRuTranslation { get => speakerWordsRuTranslation; set => speakerWordsRuTranslation = value; }
        public string SpeakerWordsComments { get => speakerWordsComments; set => speakerWordsComments = value; }
        public string SpeakerSpeechLanguage { get => speakerSpeechLanguage; set => speakerSpeechLanguage = value; }
        public string SpeakerPartOfSpeech { get => speakerPartOfSpeech; set => speakerPartOfSpeech = value; }
    }
}
