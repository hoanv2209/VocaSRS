using System.Runtime.CompilerServices;
using VocaSRS.Context.Entities;

namespace VocaSRS.Extensions
{
    public static class Extension
    {
        public static PracticeStatus Next(this PracticeStatus current)
        {
            switch (current)
            {
                case PracticeStatus.Unseen:
                    return PracticeStatus.First;
                case PracticeStatus.First:
                    return PracticeStatus.Second;
                case PracticeStatus.Second:
                    return PracticeStatus.Third;
                case PracticeStatus.Third:
                    return PracticeStatus.Fourth;
                case PracticeStatus.Fourth:
                    return PracticeStatus.Fifth;
                case PracticeStatus.Fifth:
                    return PracticeStatus.Memorized;
                case PracticeStatus.Memorized:
                    return PracticeStatus.Memorized;
                default:
                    return PracticeStatus.Unseen;
            }
        }

        public static string HtmlHighLight(this string s)
        {
            return $"<span class=\"text-danger\">{s}</span>";
        }
    }
}
