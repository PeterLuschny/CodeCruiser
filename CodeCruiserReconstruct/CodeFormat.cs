using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Luschny.Utils.CodeCruiser
{
    public abstract class CodeFormat : SourceFormat
    {
        protected CodeFormat()
        {
            Regex regex = new Regex(@"\w+|-\w+|#\w+|@@\w+|#(?:\\(?:s|w)(?:\*|\+)?\w+)+|@\\w\*+");
            string input = regex.Replace(this.Keywords, @"(?<=^|\W)$0(?=\W)");
            string str2 = regex.Replace(this.Preprocessors, @"(?<=^|\s)$0(?=\s|$)");
            regex = new Regex(" +");
            input = regex.Replace(input, "|");
            str2 = regex.Replace(str2, "|");
            if (str2.Length == 0)
            {
                str2 = "(?!.*)_{37}(?<!.*)";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("(");
            builder.Append(this.CommentRegEx);
            builder.Append(")|(");
            builder.Append(this.StringRegEx);
            if (str2.Length > 0)
            {
                builder.Append(")|(");
                builder.Append(str2);
            }
            builder.Append(")|(");
            builder.Append(input);
            builder.Append(")");
            RegexOptions options = this.CaseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;
            base.CodeRegex = new Regex(builder.ToString(), RegexOptions.Singleline | options);
        }

        protected override string MatchEval(Match match)
        {
            if (match.Groups[1].Success)
            {
                string str;
                StringReader reader = new StringReader(match.ToString());
                StringBuilder builder = new StringBuilder();
                while ((str = reader.ReadLine()) != null)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append("\n");
                    }
                    builder.Append("<span class=\"rem\">");
                    builder.Append(str);
                    builder.Append("</span>");
                }
                return builder.ToString();
            }
            if (match.Groups[2].Success)
            {
                return ("<span class=\"str\">" + match.ToString() + "</span>");
            }
            if (match.Groups[3].Success)
            {
                return ("<span class=\"preproc\">" + match.ToString() + "</span>");
            }
            if (match.Groups[4].Success)
            {
                return ("<span class=\"kwd\">" + match.ToString() + "</span>");
            }
            return "";
        }

        public virtual bool CaseSensitive
        {
            get { return true; }
        }

        protected abstract string CommentRegEx { get; }

        protected abstract string Keywords { get; }

        protected virtual string Preprocessors
        {
            get{ return ""; }
        }

        protected abstract string StringRegEx { get; }
    }
}
