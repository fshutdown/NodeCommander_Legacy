using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stratis.NodeCommander.Controls
{
    public class StackTraceRichTextBox : RichTextBox
    {
        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                Format(Text);
            }
        }

        private void Format(string text)
        {
            string formattedException = string.Empty;
            foreach (string line in text.Split(new char[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] lineParts = line.Split(new string[] {" in "}, StringSplitOptions.None);
                string executionLine = lineParts[0];
                string fileReference = string.Empty;


                if (!line.Trim().StartsWith("at"))
                {
                    formattedException += line + "\n";
                    continue;
                }

                executionLine = Regex.Replace(executionLine, "(\\s+)(at)(\\s+)", $"$1\\cf2 $2\\cf1 $3");
                executionLine = Regex.Replace(executionLine, "([a-zA-Z0-9`_-]+)\\.", $"\\cf3 $1\\cf1 .");
                executionLine = Regex.Replace(executionLine, "([a-zA-Z0-9`_-]+)\\(", $"\\cf5\\b1 $1\\cf1\\b0 (");
                executionLine = Regex.Replace(executionLine, "<([a-zA-Z0-9`_-]+)>", $"<\\cf4 $1\\cf1 >");
                executionLine = Regex.Replace(executionLine, "(\\([a-zA-Z0-9`_-]+|,\\s+[a-zA-Z0-9`_-]+)",
                    $"\\cf4 $0\\cf1");
                executionLine = Regex.Replace(executionLine, "(\\([a-zA-Z0-9`_-]+|,\\s+[a-zA-Z0-9`_-]+)",
                    $"\\cf4 $0\\cf1");
                executionLine = Regex.Replace(executionLine, "[,()]+", $"\\cf1 $0");

                if (lineParts.Length == 2)
                {
                    fileReference = lineParts[1];
                    fileReference = Regex.Replace(fileReference, ":(line [0-9]+)", $":\\cf7 $1");
                    fileReference = Regex.Replace(fileReference, "([A-Za-z0-9-_]+\\.cs):",
                        $" \\b1\\cf7 $1\\cf1\\b0:");
                    formattedException += $"{executionLine} \\cf2 in \\cf8 {fileReference}" + "\n";
                }
                else
                {
                    formattedException += $"{executionLine} \\cf2" + "\n";
                }
            }

            formattedException = formattedException.Replace("\n", "\\par");

            Rtf = "{\\rtf1\\ansi\\deff0{\\colortbl;" +
                  "\\red0\\green0\\blue0;" +
                  "\\red0\\green43\\blue180;" +
                  "\\red23\\green54\\blue93;" +
                  "\\red49\\green132\\blue155;" +
                  "\\red148\\green54\\blue52;" +
                  "\\red220\\green20\\blue60;" +
                  "\\red79\\green98\\blue40;" +
                  "\\red128\\green128\\blue128;" +
                  "}" + formattedException + "}";
        }
    }
}
