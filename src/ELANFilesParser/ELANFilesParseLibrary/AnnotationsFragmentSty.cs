using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELANFilesParseLibrary
{
    //TODO: Implementation inheritance (AnnotationsFragment) of interface
    public class AnnotationsFragmentSty
    {
        private string fragmentText;
        private int startTime;
        private int endTime;
        private List<InterviewerSpeechSty> interviewerSpeeches;
        private List<SpeakerSpeechSty> speakerSpeeches;

        public AnnotationsFragmentSty()
        {
            interviewerSpeeches = new List<InterviewerSpeechSty>();
            speakerSpeeches = new List<SpeakerSpeechSty>();
        }

        public string FragmentText { get => fragmentText; set => fragmentText = value; }
        public int StartTime { get => startTime; set => startTime = value; }
        public int EndTime { get => endTime; set => endTime = value; }
        public List<InterviewerSpeechSty> InterviewerSpeeches { get => interviewerSpeeches; set => interviewerSpeeches = value; }
        public List<SpeakerSpeechSty> SpeakerSpeeches { get => speakerSpeeches; set => speakerSpeeches = value; }

    }
}
