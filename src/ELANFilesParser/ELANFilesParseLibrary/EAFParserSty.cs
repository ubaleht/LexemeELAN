using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ELANFilesParseLibrary
{
    public class EAFParserSty
    {
        private string filePath;
        private string fullXmlText;
        private List<string> timeSlotsTierIdList = new List<string>() { "//TIER[@TIER_ID='General']", "//TIER[@TIER_ID='Speaker-Speech']", "//TIER[@TIER_ID='Speaker-Word']",
            "//TIER[@TIER_ID='Interviewer-Speech']" };
        private List<string> associationTierIdList = new List<string>() { "//TIER[@TIER_ID='Speaker-WordEnTranslation']", "//TIER[@TIER_ID='Speaker-WordRuTranslation']",
            "//TIER[@TIER_ID='Speaker-WordTrTranslation']", "//TIER[@TIER_ID='Speaker-WordLatin']", "//TIER[@TIER_ID='Speaker-PartOfSpeech']", "//TIER[@TIER_ID='Speaker-Morphology']",
            "//TIER[@TIER_ID='Speaker-SpeechEnTranslation']", "//TIER[@TIER_ID='Speaker-SpeechRuTranslation']", "//TIER[@TIER_ID='Speaker-SpeechTrTranslation']", "//TIER[@TIER_ID='Speaker-SpeechLatin']",
            "//TIER[@TIER_ID='Interviewer-SpeechEnTranslation']", "//TIER[@TIER_ID='Interviewer-SpeechTrTranslation']",
            "//TIER[@TIER_ID='Speaker-WordComments']", "//TIER[@TIER_ID='Speaker-SpeechComments']",
            "//TIER[@TIER_ID='Speaker-SpeechLanguage']", "//TIER[@TIER_ID='Speaker-WordLanguage']" };

        private Dictionary<string, List<TimeRangeNode>> timeRangeTiersDict = new Dictionary<string, List<TimeRangeNode>>();
        private Dictionary<string, List<AssociationNode>> associationTiersDict = new Dictionary<string, List<AssociationNode>>();

        Dictionary<string, int> timeOrderSectionDict = new Dictionary<string, int>();

        private List<AnnotationsFragmentSty> annotationsFragments = new List<AnnotationsFragmentSty>();

        public List<AnnotationsFragmentSty> AnnotationsFragments { get => annotationsFragments; set => annotationsFragments = value; }

        public EAFParserSty(string filePath)
        {
            this.filePath = filePath;
        }

        public void Parse()
        {
            fullXmlText = System.IO.File.ReadAllText(filePath);
            XmlDocument fullXmlDocument = new XmlDocument();
            fullXmlDocument.LoadXml(fullXmlText);
            TimeRangeNodesParse(fullXmlDocument);
            AssociationNodesParse(fullXmlDocument);
            TimeOrderSectionParse(fullXmlDocument);
            CreateFragmentsList();
        }

        private void TimeRangeNodesParse(XmlDocument fullXmlDocument)
        {
            foreach (string tierId in timeSlotsTierIdList)
            {
                List<TimeRangeNode> timeRangeNodes = new List<TimeRangeNode>();

                XmlNode tierNode = fullXmlDocument.SelectSingleNode(tierId);

                foreach (XmlNode node in tierNode.ChildNodes)
                {
                    string innerXml = node.InnerXml;
                    XmlDocument innerXmlDocument = new XmlDocument();
                    innerXmlDocument.LoadXml(innerXml);
                    timeRangeNodes.Add(new TimeRangeNode()
                    {
                        AnnotationId = innerXmlDocument.SelectSingleNode("//ALIGNABLE_ANNOTATION").Attributes["ANNOTATION_ID"].Value,
                        TimeSlotRef1 = innerXmlDocument.SelectSingleNode("//ALIGNABLE_ANNOTATION").Attributes["TIME_SLOT_REF1"].Value,
                        TimeSlotRef2 = innerXmlDocument.SelectSingleNode("//ALIGNABLE_ANNOTATION").Attributes["TIME_SLOT_REF2"].Value,
                        AnnotationValue = innerXmlDocument.SelectSingleNode("//ANNOTATION_VALUE").InnerText
                    });
                }
                string[] tierName = tierId.Split('\'');
                timeRangeTiersDict.Add(tierName[1], timeRangeNodes);
            }
        }

        private void AssociationNodesParse(XmlDocument fullXmlDocument)
        {
            foreach (string tierId in associationTierIdList)
            {
                List<AssociationNode> associationNodes = new List<AssociationNode>();

                XmlNode tierNode = fullXmlDocument.SelectSingleNode(tierId);

                foreach (XmlNode node in tierNode.ChildNodes)
                {
                    string innerXml = node.InnerXml;
                    XmlDocument innerXmlDocument = new XmlDocument();
                    innerXmlDocument.LoadXml(innerXml);
                    associationNodes.Add(new AssociationNode()
                    {
                        AnnotationId = innerXmlDocument.SelectSingleNode("//REF_ANNOTATION").Attributes["ANNOTATION_ID"].Value,
                        AnnotationRef = innerXmlDocument.SelectSingleNode("//REF_ANNOTATION").Attributes["ANNOTATION_REF"].Value,
                        AnnotationValue = innerXmlDocument.SelectSingleNode("//ANNOTATION_VALUE").InnerText
                    });
                }
                string[] tierName = tierId.Split('\'');
                associationTiersDict.Add(tierName[1], associationNodes);
            }
        }

        private void TimeOrderSectionParse(XmlDocument fullXmlDocument)
        {
            XmlNode timeOrderSection = fullXmlDocument.SelectSingleNode("//TIME_ORDER");

            foreach (XmlNode node in timeOrderSection.SelectNodes("//TIME_SLOT"))
            {
                timeOrderSectionDict.Add(node.Attributes["TIME_SLOT_ID"].Value, Convert.ToInt32(node.Attributes["TIME_VALUE"].Value));
            }
        }

        private void CreateFragmentsList()
        {
            foreach (TimeRangeNode generalTierNode in timeRangeTiersDict["General"])
            {
                List<string> generalTireKeys = timeOrderSectionDict.Where(x => x.Value >= timeOrderSectionDict[generalTierNode.TimeSlotRef1] && x.Value <= timeOrderSectionDict[generalTierNode.TimeSlotRef2]).Select(x => x.Key).ToList();
                List<string> interviewerSpeechTierNodeIdList = new List<string>();

                foreach (TimeRangeNode interviewerSpeechTierNode in timeRangeTiersDict["Interviewer-Speech"])
                {
                    if (generalTireKeys.Contains(interviewerSpeechTierNode.TimeSlotRef1) && generalTireKeys.Contains(interviewerSpeechTierNode.TimeSlotRef2))
                    {
                        interviewerSpeechTierNodeIdList.Add(interviewerSpeechTierNode.AnnotationId);
                    }
                }

                List<InterviewerSpeechSty> interviewerSpeechList = new List<InterviewerSpeechSty>();

                foreach (string interviewerSpeechTierNodeId in interviewerSpeechTierNodeIdList)
                {
                    TimeRangeNode interviewerSpeechTierCurrentNode = timeRangeTiersDict["Interviewer-Speech"].Where(x => x.AnnotationId == interviewerSpeechTierNodeId).FirstOrDefault();

                    if (interviewerSpeechTierNodeId != String.Empty)
                    {
                        AssociationNode interviewerSpeechEnTranslationTierNode = associationTiersDict["Interviewer-SpeechEnTranslation"].Where(x => x.AnnotationRef == interviewerSpeechTierCurrentNode.AnnotationId).FirstOrDefault();
                        AssociationNode interviewerSpeechTrTranslationTierNode = associationTiersDict["Interviewer-SpeechTrTranslation"].Where(x => x.AnnotationRef == interviewerSpeechTierCurrentNode.AnnotationId).FirstOrDefault();
                        interviewerSpeechList.Add(
                            new InterviewerSpeechSty()
                            {
                                InterviewerSpeechText = interviewerSpeechTierCurrentNode.AnnotationValue,
                                StartTime = timeOrderSectionDict[interviewerSpeechTierCurrentNode.TimeSlotRef1],
                                EndTime = timeOrderSectionDict[interviewerSpeechTierCurrentNode.TimeSlotRef2],
                                InterviewerSpeechEnTranslation = (interviewerSpeechEnTranslationTierNode != null) ? interviewerSpeechEnTranslationTierNode.AnnotationValue : String.Empty,
                                InterviewerSpeechTrTranslation = (interviewerSpeechTrTranslationTierNode != null) ? interviewerSpeechTrTranslationTierNode.AnnotationValue : String.Empty
                            }
                        );
                    }
                }

                List<string> speakerSpeechTierNodeIdList = new List<string>();

                foreach (TimeRangeNode speakerSpeechTierNode in timeRangeTiersDict["Speaker-Speech"])
                {
                    if (generalTireKeys.Contains(speakerSpeechTierNode.TimeSlotRef1) && generalTireKeys.Contains(speakerSpeechTierNode.TimeSlotRef2))
                    {
                        speakerSpeechTierNodeIdList.Add(speakerSpeechTierNode.AnnotationId);
                    }
                }

                List<SpeakerSpeechSty> speakerSpeechList = new List<SpeakerSpeechSty>();

                foreach (string speakerSpeechTierNodeId in speakerSpeechTierNodeIdList)
                {
                    TimeRangeNode speakerSpeechTierCurrentNode = timeRangeTiersDict["Speaker-Speech"].Where(x => x.AnnotationId == speakerSpeechTierNodeId).FirstOrDefault();

                    if (speakerSpeechTierNodeId != String.Empty)
                    {
                        AssociationNode speakerSpeechEnTranslationTierNode = associationTiersDict["Speaker-SpeechEnTranslation"].Where(x => x.AnnotationRef == speakerSpeechTierCurrentNode.AnnotationId).FirstOrDefault();
                        AssociationNode speakerSpeechRuTranslationTierNode = associationTiersDict["Speaker-SpeechRuTranslation"].Where(x => x.AnnotationRef == speakerSpeechTierCurrentNode.AnnotationId).FirstOrDefault();
                        AssociationNode speakerSpeechTrTranslationTierNode = associationTiersDict["Speaker-SpeechTrTranslation"].Where(x => x.AnnotationRef == speakerSpeechTierCurrentNode.AnnotationId).FirstOrDefault();
                        AssociationNode speakerSpeechLatinTierNode = associationTiersDict["Speaker-SpeechLatin"].Where(x => x.AnnotationRef == speakerSpeechTierCurrentNode.AnnotationId).FirstOrDefault();
                        speakerSpeechList.Add(
                            new SpeakerSpeechSty()
                            {
                                SpeakerSpeechText = speakerSpeechTierCurrentNode.AnnotationValue,
                                StartTime = timeOrderSectionDict[speakerSpeechTierCurrentNode.TimeSlotRef1],
                                EndTime = timeOrderSectionDict[speakerSpeechTierCurrentNode.TimeSlotRef2],
                                SpeakerSpeechEnTranslation = (speakerSpeechEnTranslationTierNode != null) ? speakerSpeechEnTranslationTierNode.AnnotationValue : String.Empty,
                                SpeakerSpeechRuTranslation = (speakerSpeechRuTranslationTierNode != null) ? speakerSpeechRuTranslationTierNode.AnnotationValue : String.Empty,
                                SpeakerSpeechTrTranslation = (speakerSpeechTrTranslationTierNode != null) ? speakerSpeechTrTranslationTierNode.AnnotationValue : String.Empty,
                                SpeakerSpeechLatin = (speakerSpeechLatinTierNode != null) ? speakerSpeechLatinTierNode.AnnotationValue : String.Empty
                            }
                        );
                    }
                }

                foreach (SpeakerSpeechSty speakerSpeechTierNode in speakerSpeechList)
                {
                    List<string> speakerSpeechTireKeys = timeOrderSectionDict.Where(x => x.Value >= speakerSpeechTierNode.StartTime && x.Value <= speakerSpeechTierNode.EndTime).Select(x => x.Key).ToList();

                    List<string> speakerWordsTierNodeIdList = new List<string>();

                    foreach (TimeRangeNode speakerWordsTierNode in timeRangeTiersDict["Speaker-Word"])
                    {
                        if (speakerSpeechTireKeys.Contains(speakerWordsTierNode.TimeSlotRef1) && speakerSpeechTireKeys.Contains(speakerWordsTierNode.TimeSlotRef2))
                        {
                            speakerWordsTierNodeIdList.Add(speakerWordsTierNode.AnnotationId);
                        }
                    }

                    List<SpeakerWordSty> speakerWordsList = new List<SpeakerWordSty>();

                    foreach (string speakerWordsTierNodeId in speakerWordsTierNodeIdList)
                    {
                        TimeRangeNode speakerWordsTierCurrentNode = timeRangeTiersDict["Speaker-Word"].Where(x => x.AnnotationId == speakerWordsTierNodeId).FirstOrDefault();

                        if (speakerWordsTierNodeId != String.Empty)
                        {
                            AssociationNode speakerWordsEnTranslationTierNode = associationTiersDict["Speaker-WordEnTranslation"].Where(x => x.AnnotationRef == speakerWordsTierCurrentNode.AnnotationId).FirstOrDefault();
                            AssociationNode speakerWordsRuTranslationTierNode = associationTiersDict["Speaker-WordRuTranslation"].Where(x => x.AnnotationRef == speakerWordsTierCurrentNode.AnnotationId).FirstOrDefault();
                            AssociationNode speakerWordsTrTranslationTierNode = associationTiersDict["Speaker-WordTrTranslation"].Where(x => x.AnnotationRef == speakerWordsTierCurrentNode.AnnotationId).FirstOrDefault();
                            AssociationNode speakerWordLatinTierNode = associationTiersDict["Speaker-WordLatin"].Where(x => x.AnnotationRef == speakerWordsTierCurrentNode.AnnotationId).FirstOrDefault();
                            AssociationNode speakerWordsComments = associationTiersDict["Speaker-WordComments"].Where(x => x.AnnotationRef == speakerWordsTierCurrentNode.AnnotationId).FirstOrDefault();
                            AssociationNode speakerSpeechLanguage = associationTiersDict["Speaker-SpeechLanguage"].Where(x => x.AnnotationRef == speakerWordsTierCurrentNode.AnnotationId).FirstOrDefault();
                            AssociationNode speakerPartOfSpeech = associationTiersDict["Speaker-PartOfSpeech"].Where(x => x.AnnotationRef == speakerWordsTierCurrentNode.AnnotationId).FirstOrDefault();
                            speakerWordsList.Add(
                                new SpeakerWordSty()
                                {
                                    SpeakerWordText = speakerWordsTierCurrentNode.AnnotationValue,
                                    StartTime = timeOrderSectionDict[speakerWordsTierCurrentNode.TimeSlotRef1],
                                    EndTime = timeOrderSectionDict[speakerWordsTierCurrentNode.TimeSlotRef2],
                                    SpeakerWordsEnTranslation = (speakerWordsEnTranslationTierNode != null) ? speakerWordsEnTranslationTierNode.AnnotationValue : String.Empty,
                                    SpeakerWordsRuTranslation = (speakerWordsRuTranslationTierNode != null) ? speakerWordsRuTranslationTierNode.AnnotationValue : String.Empty,
                                    SpeakerWordTrTranslation = (speakerWordsTrTranslationTierNode != null) ? speakerWordsTrTranslationTierNode.AnnotationValue : String.Empty,
                                    SpeakerWordLatin = (speakerWordLatinTierNode != null) ? speakerWordLatinTierNode.AnnotationValue : String.Empty,
                                    SpeakerWordsComments = (speakerWordsComments != null) ? speakerWordsComments.AnnotationValue : String.Empty,
                                    SpeakerWordsLanguage = (speakerSpeechLanguage != null) ? speakerSpeechLanguage.AnnotationValue : String.Empty,
                                    SpeakerPartOfSpeech = (speakerPartOfSpeech != null) ? speakerPartOfSpeech.AnnotationValue : String.Empty
                                }
                            );
                        }
                    }
                    speakerSpeechTierNode.SpeakerWordsSty = speakerWordsList;
                }

                annotationsFragments.Add(new AnnotationsFragmentSty()
                {
                    FragmentText = generalTierNode.AnnotationValue,
                    StartTime = timeOrderSectionDict[generalTierNode.TimeSlotRef1],
                    EndTime = timeOrderSectionDict[generalTierNode.TimeSlotRef2],
                    InterviewerSpeeches = interviewerSpeechList,
                    SpeakerSpeeches = speakerSpeechList
                });
            }
        }
    }
}
