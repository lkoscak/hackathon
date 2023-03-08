using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApi.Core.Context;
using WebApi.Core.Repository;

namespace FleetWebApi.BusinessLogic.CommonUtilities
{
    public class TranslationManager : BaseRepository
    {
        string language;
        Dictionary<string, string> translatedWords;
        List<string> wordList;

        public TranslationManager(IContextManager contextManager, List<string> wordList, string lang) : base(contextManager)
        {
            translatedWords = new Dictionary<string, string>();
            language = lang;
            this.wordList = wordList;
            if(language != "hr")  fillTranslatedWords();
        }


        private void fillTranslatedWords()
        {
            using (SqlDataReader data = DynamicQuery.GetData(Connection, ContextManager.sessionKey, 916, language))
            {
                while (data.Read())
                {
                    if (data["dic_croatian"] != DBNull.Value && data["dic_word"] != DBNull.Value)
                    {
                        if (!translatedWords.ContainsKey(data["dic_croatian"].ToString()))
                        {
                            translatedWords.Add(data["dic_croatian"].ToString(), data["dic_word"].ToString());
                        }
                    }
                }
            }
        }
        public string getTranslatedWord(string word)
        {
            if (language == "hr") return word;
            if (translatedWords.ContainsKey(word))
            {
                return translatedWords[word];
            }
            else
            {
                ContextManager.loggerManager.info($"Word {word} for lang {language} not found in dictionary");
                return word;
            }

        }

    }
}