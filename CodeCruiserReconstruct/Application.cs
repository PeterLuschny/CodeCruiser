using System;
using System.Windows.Forms;

namespace Luschny.Utils.CodeCruiser
{
    internal static class Application
    {
        public static string CopyrightMsg = "    Code Cruiser.                              Version: 2012-12-31 NET4.5 \n                                                                     \n    (C) Copyright Peter Luschny (peter.luschny@gmail.com)            \n    Portions: (C) Copyright Jean-Claude Manoli (jc@manoli.net)       \n                                                                     \n This software is provided 'as-is', without any express or implied   \n warranty. In no event will the author(s) be held liable for any     \n damages arising from the use of this software.                      \n                                                                     \n Permission is granted to anyone to use this software for any        \n purpose, and to redistribute it freely, subject to the following    \n restriction:                                                        \n                                                                     \n This notice may not be removed or altered in any distribution.      \n                                                                     \n                                    E N J O Y                        \n";
        public static string dstOptions = "Specifies the output directory to store the       \ngenerated files in. By default the generated files\nare stored in the directory of the source files.    ";
        public static string formOptions = "Enables or disables:           \n-  line numbers in output,     \n-  alternating line background,\n-  embedded css style sheet.     ";
        public static string srcOptions = "An input parameter can be either a file, a directory or a   \ndirectory tree. When a directory is specified, all the files\nof the selected type inside that directory are converted.   \nWhen a tree is specified this conversion is recursivly.      ";

        [STAThread]
        private static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new CruiserForm());
        }
    }
}
