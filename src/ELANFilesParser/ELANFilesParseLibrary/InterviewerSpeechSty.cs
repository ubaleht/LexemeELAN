using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELANFilesParseLibrary
{
    public class InterviewerSpeechSty : InterviewerSpeech
    {
        private string interviewerSpeechTrTranslation;

        public InterviewerSpeechSty() : base() { }

        public string InterviewerSpeechTrTranslation { get => interviewerSpeechTrTranslation; set => interviewerSpeechTrTranslation = value; }
    }
}
