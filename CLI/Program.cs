using Mono.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool outputsNumber = false, outputsMostWords = false, differentInitials = false, outputsMostLetters = false, allowingRings = false;
            var otherArgs = new List<string>();
            char? headLetter = null, tailLetter = null;
            string fileName;

            var p = new OptionSet()
            {
                "Usage: Wordlist.exe [OPTIONS] <WordList>.txt",
                "Options:",
                { "n", "Outputs the number of word chains, including nested chains.",
                  v => outputsNumber = v != null },
                { "w", "Outputs the word chain containing the most number of words to solution.txt.",
                  v => outputsMostWords = v != null },
                { "m", "Outputs the word chain containing the most number of words with different initials to solution.txt.",
                  v => differentInitials = v != null },
                { "c", "Outputs the word chain containing the most number of letters to solution.txt.",
                  v => outputsMostLetters = v != null },
                { "h=", "Outputs the word chain with head letter {HEAD}.",
                  (char v) => {
                      if (char.IsLetter(v))
                      {
                          headLetter = char.ToLower(v);
                      }
                      else
                      {
                          throw new OptionException(string.Format("Option `-h' requires a letter, not `{0}\'.", v), "h");
                      }
                  }
                },
                { "t=", "Outputs the word chain with tail letter {TAIL}.",
                  (char v) => {
                      if (char.IsLetter(v))
                      {
                          tailLetter = char.ToLower(v);
                      }
                      else
                      {
                          throw new OptionException(string.Format("Option `-t' requires a letter, not `{0}\'.", v), "t");
                      }
                  }
                },
                { "r", "Outputs the word chain allowing words to form rings.",
                  v => allowingRings = v != null },
            };
            try
            {
                otherArgs = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                p.WriteOptionDescriptions(Console.Error);
                return;
            }
            if (!otherArgs.Any())
            {
                p.WriteOptionDescriptions(Console.Error);
                return;
            }
            var invalidOptions = from junk in otherArgs where junk.StartsWith("-") && !junk.EndsWith(".txt") select junk;
            if (invalidOptions.Any())
            {
                Console.Error.WriteLine("Invalid option(s): {0}", string.Join(" ", invalidOptions));
                p.WriteOptionDescriptions(Console.Error);
                return;
            }
            var fileNames = (from junk in otherArgs where !junk.StartsWith("-") || junk.EndsWith(".txt") select junk).ToArray();
            if (fileNames.Length != 1)
            {
                Console.Error.WriteLine("Wordlist.exe receives exactly ONE wordlist file with extension .txt.");
                p.WriteOptionDescriptions(Console.Error);
                return;
            }
            if (!(outputsNumber || outputsMostWords || differentInitials || outputsMostLetters || allowingRings))
            {
                Console.Error.WriteLine("At least one option should be given.");
                p.WriteOptionDescriptions(Console.Error);
                return;
            }
            if (outputsNumber && outputsMostWords || differentInitials && outputsMostLetters ||
                outputsNumber && differentInitials || outputsMostWords && outputsMostLetters ||
                outputsNumber && outputsMostLetters || outputsMostWords && differentInitials)
            {
                Console.Error.WriteLine("These options cannot be used together: `-n' `-w' `-m' `-c'.");
                p.WriteOptionDescriptions(Console.Error);
                return;
            }
            if (differentInitials && (headLetter != null || tailLetter != null || allowingRings))
            {
                Console.Error.WriteLine("Option `-m' cannot be used with option `-h', `-t' or `-r'.");
                p.WriteOptionDescriptions(Console.Error);
                return;
            }
            fileName = fileNames[0];
        }
    }
}
