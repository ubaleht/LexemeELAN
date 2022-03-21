using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELANFilesParseLibrary
{
    public class AnnotationsFragment
    {
        private string fragmentText;
        private int startTime;
        private int endTime;
        private List<InterviewerSpeech> interviewerSpeeches;
        private List<SpeakerSpeech> speakerSpeeches;

        public AnnotationsFragment()
        {
            interviewerSpeeches = new List<InterviewerSpeech>();
            speakerSpeeches = new List<SpeakerSpeech>();
        }

        public string FragmentText { get => fragmentText; set => fragmentText = value; }
        public int StartTime { get => startTime; set => startTime = value; }
        public int EndTime { get => endTime; set => endTime = value; }
        public List<InterviewerSpeech> InterviewerSpeeches { get => interviewerSpeeches; set => interviewerSpeeches = value; }
        public List<SpeakerSpeech> SpeakerSpeeches { get => speakerSpeeches; set => speakerSpeeches = value; }

        public void InsertToDB() { }

        public void CreateFragment() { }
    }
}
