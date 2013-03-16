using System;
using System.IO;

namespace Luschny.Utils.CodeCruiser
{
    internal sealed class CodeFrame
    {
        public static string[] CodeCruiserCss = new string[] { 
        "/* --- CodeCruiser --- */", ".ccl, .ccl pre {", "\tfont-size: small;", "\tcolor: black;", "\tfont-family: Consolas, 'Courier New', Courier, Monospace;", "\tbackground-color: #ffffff;", "}", "p.banner {", "\tpadding: 4px;", "\tletter-spacing: 4px;", "\ttext-transform: capitalize;", "\tfont-weight: bold;", "\tcolor: yellow;", "\tbackground-color: #ccccff;", "\ttext-align: center;", "\tfont-family: Verdana, 'Courier New', Monospace;", 
        "}", ".ccl pre {", "\tmargin: 0em;", "}", ".ccl .rem {", "\tcolor: #008000;", "}", ".ccl .kwd {", "\tcolor: #0000ff;", "}", ".ccl .str {", "\tcolor: #a31515;", "}", ".ccl .op {", "\tcolor: #0000c0;", "}", 
        ".ccl .preproc {", "\tcolor: #cc6633;", "}", ".ccl .asp {", "\tbackground-color: #ffff00;", "}", ".ccl .html {", "\tcolor: #800000;", "}", ".ccl .attr {", "\tcolor: #ff0000;", "}", ".ccl .alt {", "\tbackground-color: #f4f4f4;", "\twidth: 100%;", "\tmargin: 0em;", 
        "}", ".ccl .lnum {", "\tcolor: #606060;", "}"
     };
        private static string frame = "<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Frameset//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">\n<html><head><title>Cool Code Listings For The Web</title>\n<meta http-equiv=\"Content-Type\" content=\"text/html;charset=iso-8859-1\" >\n<meta http-equiv=\"Content-Language\" content=\"en-us\" ></head>\n<frameset cols=\"24%,76%\"> <frame src=\"__Index.html\" scrolling=\"auto\" target=\"code\" >\n<frame src=\"REPLACEME.HTML\"  scrolling=\"auto\" name=\"code\" >\n<noframes><p>This document is designed to be viewed using the frames feature.</p></noframes>\n</frameset></html>";

        private CodeFrame()
        {
        }

        public static void WriteCss(string destPath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(destPath))
                {
                    foreach (string str in CodeCruiserCss)
                    {
                        writer.WriteLine(str);
                    }
                }
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public static void WriteFooter(StreamWriter writer)
        {
            writer.Write("<p class=\"banner\">~bye</p>\n</body></html>\n");
        }

        public static void WriteFrame(string destPath, string src)
        {
            string str = frame.Replace("REPLACEME.HTML", src);
            try
            {
                using (StreamWriter writer = new StreamWriter(destPath))
                {
                    writer.Write(str);
                }
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public static void WriteHeader(StreamWriter writer, string title, bool embedStyleSheet)
        {
            writer.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\"\"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n<head>\n<link rel=\"stylesheet\" type=\"text/css\" href=\"CodeCruiser.css\" title=\"Style\" />\n<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml; charset=utf-8\" />\n<meta http-equiv=\"Content-Language\" content=\"en-us\" />\n<meta name=\"Generator\" content=\"CodeCruiser\" />\n");
            writer.Write("<title>" + title + "</title>\n");
            if (embedStyleSheet)
            {
                writer.Write("<style type=\"text/css\">\n");
                foreach (string str in CodeCruiserCss)
                {
                    writer.WriteLine(str);
                }
                writer.Write("</style>\n");
            }
            writer.Write("</head>\n<body><p class=\"banner\">" + title + "</p>\n");
        }
    }
}
 
