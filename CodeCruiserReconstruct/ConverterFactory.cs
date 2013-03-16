namespace Luschny.Utils.CodeCruiser
{
    internal sealed class ConverterFactory
    {
        private ConverterFactory()
        {
        }

        public static IConverter CreateConverter(string codetype, bool paper, bool line, bool embed)
        {
            if (codetype == "java")
            {
                return new JavaConverter(paper, line, embed);
            }
            if (codetype == "vb")
            {
                return new VisualBasicConverter(paper, line, embed);
            }
            if (codetype == "groovy")
            {
                return new GroovyConverter(paper, line, embed);
            }
            if (codetype == "js")
            {
                return new JavaScriptConverter(paper, line, embed);
            }
            if (codetype == "asy")
            {
                return new AsymptoteConverter(paper, line, embed);
            }
            if (codetype == "ps1")
            {
                return new PowerShellConverter(paper, line, embed);
            }
            return new CSharpConverter(paper, line, embed);
        }
    }
} 
