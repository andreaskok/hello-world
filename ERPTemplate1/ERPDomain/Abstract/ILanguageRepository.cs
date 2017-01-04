using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ILanguageRepository
    {
        IEnumerable<Language> Language { get; }
        IEnumerable<Language> GetLanguagePaging(Int64 startID, Int64 endID);
        IEnumerable<Language> LanguageWildSearch(string fieldName, string fieldValue);
        IEnumerable<Language> GetLanguageByID(Int64 id);
        void SaveLanguage(Language language);
        void DeleteLanguage(Language language);
        Int64 LanguageCount();
        Int64 GetMaxID();

    }
}
