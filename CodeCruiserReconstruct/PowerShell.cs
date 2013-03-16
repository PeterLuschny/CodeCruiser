using System.IO;

namespace Luschny.Utils.CodeCruiser
{
    public class PowerShellFormat : CodeFormat
    {
        protected override string CommentRegEx
        {
            get
            {
                return @"#.*?(?=\r|\n)";
            }
        }

        protected override string Keywords
        {
            get
            {
                return "do function filter global script local private if else elseif for foreach in while switch continue break return default param begin process end throw trap until true false";
            }
        }

        protected override string Preprocessors
        {
            get
            {
                return "-as -contains -band -bor -bnot -match -notmatch -gt -ge -lt -le -is -imatch -inotmatch -ilike -like -notlike -eq -ne -replace -not -and -or -inotlike -ieq -ine -igt -ige -ilt -ile -xor -bxor";
            }
        }

        protected override string StringRegEx
        {
            get
            {
                return "@?\"\"|@?\".*?(?!\\\\).\"|''|'.*?(?!\\\\).'";
            }
        }
    }

    public class PowerShellConverter : PowerShellFormat, IConverter
    {
        public PowerShellConverter(bool Alternate, bool LineNumbers, bool EmbedStyleSheet)
        {
            base.Alternate = Alternate;
            base.LineNumbers = LineNumbers;
            base.EmbedStyleSheet = EmbedStyleSheet;
        }

        public string FormatCode(FileStream reader)
        {
            return base.FormatCode((Stream)reader);
        }
    }
} 
