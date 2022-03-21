using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELANFilesParseLibrary
{
    public class InterviewerSpeech
    {
        private string interviewerSpeechText;
        private int startTime;
        private int endTime;
        private string interviewerSpeechEnTranslation;
        
        public InterviewerSpeech() {}

        public string InterviewerSpeechText { get => interviewerSpeechText; set => interviewerSpeechText = value; }
        public int StartTime { get => startTime; set => startTime = value; }
        public int EndTime { get => endTime; set => endTime = value; }
        public string InterviewerSpeechEnTranslation { get => interviewerSpeechEnTranslation; set => interviewerSpeechEnTranslation = value; }
    }
}
