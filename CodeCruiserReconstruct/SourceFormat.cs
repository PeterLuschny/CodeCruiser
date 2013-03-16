using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Luschny.Utils.CodeCruiser
{
    public abstract class SourceFormat
    {
        private bool _alternate;
        private bool _embedStyleSheet;
        private bool _lineNumbers;
        private byte _tabSpaces = 4;
        private Regex codeRegex;

        protected SourceFormat()
        {
        }

        public string FormatCode(Stream source)
        {
            StreamReader reader = new StreamReader(source);
            string str = reader.ReadToEnd();
            reader.Close();
            return this.FormatCode(str, this._lineNumbers, this._alternate, false);
        }

        public string FormatCode(string source)
        {
            return this.FormatCode(source, this._lineNumbers, this._alternate, false);
        }

        private string FormatCode(string source, bool lineNumbers, bool alternate, bool subCode)
        {
            StringBuilder builder = new StringBuilder(source);
            if (!subCode)
            {
                builder.Replace("&", "&amp;");
                builder.Replace("<", "&lt;");
                builder.Replace(">", "&gt;");
                builder.Replace("\t", string.Empty.PadRight(this._tabSpaces));
            }
            source = this.codeRegex.Replace(builder.ToString(), new MatchEvaluator(this.MatchEval));
            builder = new StringBuilder();
            if (lineNumbers || alternate)
            {
                string str;
                if (!subCode)
                {
                    builder.Append("<div class=\"ccl\">\n");
                }
                StringReader reader = new StringReader(source);
                int num = 0;
                while ((str = reader.ReadLine()) != null)
                {
                    num++;
                    if (alternate && ((num % 2) == 1))
                    {
                        builder.Append("<pre class=\"alt\">");
                    }
                    else
                    {
                        builder.Append("<pre>");
                    }
                    if (lineNumbers)
                    {
                        int num2 = (int)Math.Log10((double)num);
                        builder.Append("<span class=\"lnum\">" + "    ".Substring(0, Math.Max(0, 3 - num2)) + num.ToString(CultureInfo.CurrentCulture) + ":  </span>");
                    }
                    if (str.Length == 0)
                    {
                        builder.Append("&nbsp;");
                    }
                    else
                    {
                        builder.Append(str);
                    }
                    builder.Append("</pre>\n");
                }
                reader.Close();
                if (!subCode)
                {
                    builder.Append("</div>");
                }
            }
            else
            {
                if (!subCode)
                {
                    builder.Append("<pre class=\"ccl\">\n");
                }
                builder.Append(source);
                if (!subCode)
                {
                    builder.Append("</pre>");
                }
            }
            return builder.ToString();
        }

        public string FormatSubCode(string source)
        {
            return this.FormatCode(source, false, false, true);
        }

        protected abstract string MatchEval(Match match);

        public bool Alternate
        {
            get { return this._alternate; }
            set { this._alternate = value; }
        }

        protected Regex CodeRegex
        {
            get { return this.codeRegex; }
            set { this.codeRegex = value; }
        }

        public bool EmbedStyleSheet
        {
            get { return this._embedStyleSheet; }
            set { this._embedStyleSheet = value; }
        }

        public bool LineNumbers
        {
            get { return this._lineNumbers; }
            set { this._lineNumbers = value; }
        }

        public byte TabSpaces
        {
            get { return this._tabSpaces; }
            set { this._tabSpaces = value; }
        }
    }
} 
