using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELANFilesParseLibrary;
using System.Data;
using System.Data.SqlClient;

namespace ELANToDatabase
{
    public class ELANToMSSQLServer
    {
        private string connectionString = System.Configuration.ConfigurationSettings.AppSettings["DBConnection"].Replace("\\\\", "\\");

        private int batchSize = 10;

        public ELANToMSSQLServer() { }

        private void ConnectToDatabase()
        {

        }

        public void SaveELANFileToDatabase(List<AnnotationsFragment> annotationsFragments)
        {
            int startRange = 0;
            int endRange = 0;
            bool flag = true;
            while (flag == true)
            {
                if (startRange + batchSize >= annotationsFragments.Count)
                {
                    endRange = annotationsFragments.Count;
                    flag = false;
                }
                else
                {
                    endRange = startRange + batchSize;
                }

                using (var conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    { 
                        conn.Open();
                        for (int fragmentIndex = startRange; fragmentIndex < endRange; fragmentIndex++)
                        {
                            cmd.CommandText = "INSERT INTO GeneralFragment (FragmentName, Start, Finish, FileNameFieldWorkFile) VALUES (@FragmentName" + fragmentIndex.ToString() + ", @Start" + fragmentIndex.ToString()
                                + ", @Finish" + fragmentIndex.ToString() + ", @FileNameFieldWorkFile" + fragmentIndex.ToString() + ")";
                            cmd.Parameters.AddWithValue("@FragmentName" + fragmentIndex.ToString(), annotationsFragments[fragmentIndex].FragmentText);
                            cmd.Parameters.AddWithValue("@Start" + fragmentIndex.ToString(), annotationsFragments[fragmentIndex].StartTime);
                            cmd.Parameters.AddWithValue("@Finish" + fragmentIndex.ToString(), annotationsFragments[fragmentIndex].EndTime);
                            cmd.Parameters.AddWithValue("@FileNameFieldWorkFile" + fragmentIndex.ToString(), "KKM-34-003");
                            cmd.ExecuteNonQuery();

                            /*using (var reader = cmd.ExecuteReader()){}*/
                            cmd.CommandText = "SELECT Id FROM GeneralFragment WHERE Id = @@Identity";
                            var generalFragmentId = cmd.ExecuteScalar();

                            for (int interviewerIndex = 0; interviewerIndex < annotationsFragments[fragmentIndex].InterviewerSpeeches.Count; interviewerIndex++)
                            {
                                string interviewerVarName = fragmentIndex.ToString() + interviewerIndex.ToString();
                                cmd.CommandText = "INSERT INTO InterviewerSpeech (IdGeneralFragment, Start, Finish, InterviewerSpeech, InterviewerSpeechEnTranslation, InterviewerPersonalCode) VALUES (@IdGeneralFragment"
                                    + interviewerVarName + ", @InterviewerStart" + interviewerVarName + ", @InterviewerFinish" + interviewerVarName +
                                    ", @InterviewerSpeech" + interviewerVarName + ", @InterviewerSpeechEnTranslation" + interviewerVarName + ", @InterviewerPersonalCode" + interviewerVarName + ")";
                                cmd.Parameters.AddWithValue("@IdGeneralFragment" + interviewerVarName, generalFragmentId);
                                cmd.Parameters.AddWithValue("@InterviewerStart" + interviewerVarName, annotationsFragments[fragmentIndex].InterviewerSpeeches[interviewerIndex].StartTime);
                                cmd.Parameters.AddWithValue("@InterviewerFinish" + interviewerVarName, annotationsFragments[fragmentIndex].InterviewerSpeeches[interviewerIndex].EndTime);
                                cmd.Parameters.AddWithValue("@InterviewerSpeech" + interviewerVarName, (annotationsFragments[fragmentIndex].InterviewerSpeeches[interviewerIndex].InterviewerSpeechText != null) ? annotationsFragments[fragmentIndex].InterviewerSpeeches[interviewerIndex].InterviewerSpeechText : String.Empty);
                                cmd.Parameters.AddWithValue("@InterviewerSpeechEnTranslation" + interviewerVarName, (annotationsFragments[fragmentIndex].InterviewerSpeeches[interviewerIndex].InterviewerSpeechEnTranslation != null) ? annotationsFragments[fragmentIndex].InterviewerSpeeches[interviewerIndex].InterviewerSpeechEnTranslation : String.Empty);
                                cmd.Parameters.AddWithValue("@InterviewerPersonalCode" + interviewerVarName, "UIP-80");
                                cmd.ExecuteNonQuery();
                            }

                            for (int speakerIndex = 0; speakerIndex < annotationsFragments[fragmentIndex].SpeakerSpeeches.Count; speakerIndex++)
                            {
                                string speakerVarName = fragmentIndex.ToString() + speakerIndex.ToString();
                                cmd.CommandText = "INSERT INTO SpeakerSpeech(IdGeneralFragment, Start, Finish, Speech, SpeechEnTranslation, SpeechRuTranslation, SpeakerPersonalCode) VALUES(@IdGeneralFragmentForSpeech" + speakerVarName + ", @SpeechStart" + speakerVarName
                                    + ", @SpeechFinish" + speakerVarName + ", @SpeakerSpeech" + speakerVarName + ", @SpeakerSpeechEnTranslation" + speakerVarName + ", @SpeakerSpeechRuTranslation" + speakerVarName
                                    + ", @SpeakerPersonalCode" + speakerVarName + ")";
                                cmd.Parameters.AddWithValue("@IdGeneralFragmentForSpeech" + speakerVarName, generalFragmentId);
                                cmd.Parameters.AddWithValue("@SpeechStart" + speakerVarName, annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].StartTime);
                                cmd.Parameters.AddWithValue("@SpeechFinish" + speakerVarName, annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].EndTime);
                                cmd.Parameters.AddWithValue("@SpeakerSpeech" + speakerVarName, (annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerSpeechText != null) ? annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerSpeechText : String.Empty);
                                cmd.Parameters.AddWithValue("@SpeakerSpeechEnTranslation" + speakerVarName, (annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerSpeechEnTranslation != null) ? annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerSpeechEnTranslation : String.Empty);
                                cmd.Parameters.AddWithValue("@SpeakerSpeechRuTranslation" + speakerVarName, (annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerSpeechRuTranslation != null) ? annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerSpeechRuTranslation : String.Empty);
                                cmd.Parameters.AddWithValue("@SpeakerPersonalCode" + speakerVarName, "KKM-34");
                                cmd.ExecuteNonQuery();

                                cmd.CommandText = "SELECT Id FROM SpeakerSpeech WHERE Id = @@Identity";
                                var speakerSpeechId = cmd.ExecuteScalar();

                                for (int wordIndex = 0; wordIndex < annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerWords.Count; wordIndex++)
                                {
                                    string wordVarName = fragmentIndex.ToString() + speakerIndex.ToString() + wordIndex.ToString();
                                    cmd.CommandText = "INSERT INTO Word(IdSpeakerSpeech, Start, Finish, Word, WordEnTranslation, WordRuTranslation, PartOfSpeech, WordComments) VALUES(@IdSpeakerSpeech" + wordVarName + ", @WordStart" + wordVarName
                                        + ", @WordFinish" + wordVarName + ", @WordText" + wordVarName + ", @WordEnTranslation" + wordVarName + ", @WordRuTranslation" + wordVarName
                                        + ", @PartOfSpeech" + wordVarName + ", @WordComments" + wordVarName + ")";
                                    cmd.Parameters.AddWithValue("@IdSpeakerSpeech" + wordVarName, speakerSpeechId);
                                    cmd.Parameters.AddWithValue("@WordStart" + wordVarName, annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerWords[wordIndex].StartTime);
                                    cmd.Parameters.AddWithValue("@WordFinish" + wordVarName, annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerWords[wordIndex].EndTime);
                                    cmd.Parameters.AddWithValue("@WordText" + wordVarName, (annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerWords[wordIndex].SpeakerWordText != null) ? annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerWords[wordIndex].SpeakerWordText : String.Empty);
                                    cmd.Parameters.AddWithValue("@WordEnTranslation" + wordVarName, (annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerWords[wordIndex].SpeakerWordsEnTranslation != null) ? annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerWords[wordIndex].SpeakerWordsEnTranslation : String.Empty);
                                    cmd.Parameters.AddWithValue("@WordRuTranslation" + wordVarName, (annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerWords[wordIndex].SpeakerWordsRuTranslation != null) ? annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerWords[wordIndex].SpeakerWordsRuTranslation : String.Empty);
                                    cmd.Parameters.AddWithValue("@PartOfSpeech" + wordVarName, (annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerWords[wordIndex].SpeakerPartOfSpeech != null) ? annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerWords[wordIndex].SpeakerPartOfSpeech : String.Empty);
                                    cmd.Parameters.AddWithValue("@WordComments" + wordVarName, (annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerWords[wordIndex].SpeakerWordsComments != null) ? annotationsFragments[fragmentIndex].SpeakerSpeeches[speakerIndex].SpeakerWords[wordIndex].SpeakerWordsComments : String.Empty);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }    
                    }
                }
                startRange = endRange;
            }
        }
    }
}
