using Core;
using Mono.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CLI
{
    internal class Program
    {
        private const int GUESS_HOW_BIG = 1024;

        static void Main(string[] args)
        {
            bool outputsNumber = false,
                outputsMostWords = false,
                differentInitials = false,
                outputsMostLetters = false,
                allowingRings = false;
            var otherArgs = new List<string>();
            char? headLetter = null, tailLetter = null;
            string fileName;

            var p = new OptionSet()
            {
                "Usage: Wordlist.exe [OPTIONS] <WordList>.txt",
                "Options:",
                {
                    "n", "Outputs the number of word chains, including nested chains to stdout.",
                    v => outputsNumber = v != null
                },
                {
                    "w", "Outputs the word chain containing the most number of words to solution.txt.",
                    v => outputsMostWords = v != null
                },
                {
                    "m", "Outputs the word chain containing the most number of words with different initials to solution.txt.",
                    v => differentInitials = v != null
                },
                {
                    "c", "Outputs the word chain containing the most number of letters to solution.txt.",
                    v => outputsMostLetters = v != null
                },
                {
                    "h=", "Outputs the word chain with head letter {HEAD}.",
                    (char v) =>
                    {
                        if (char.IsLetter(v))
                        {
                            headLetter = char.ToLower(v);
                        }
                        else
                        {
                            throw new OptionException($"Option `-h' requires a letter, not `{v}'.", "h");
                        }
                    }
                },
                {
                    "t=", "Outputs the word chain with tail letter {TAIL}.",
                    (char v) =>
                    {
                        if (char.IsLetter(v))
                        {
                            tailLetter = char.ToLower(v);
                        }
                        else
                        {
                            throw new OptionException($"Option `-t' requires a letter, not `{v}'.", "t");
                        }
                    }
                },
                {
                    "r", "Outputs the word chain allowing words to form rings.",
                    v => allowingRings = v != null
                },
            };
            try
            {
                otherArgs = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Error.WriteLine(e.Message);
                p.WriteOptionDescriptions(Console.Error);
                return;
            }

            if (!otherArgs.Any())
            {
                Console.Error.WriteLine("Wordlist.exe receives exactly ONE wordlist file with extension .txt.");
                p.WriteOptionDescriptions(Console.Error);
                return;
            }

            var invalidOptions = from arg in otherArgs where (arg.StartsWith("-") || arg.StartsWith("/")) && !arg.EndsWith(".txt") select arg;
            if (invalidOptions.Any())
            {
                Console.Error.WriteLine("Invalid option(s): {0}", string.Join(" ", invalidOptions));
                p.WriteOptionDescriptions(Console.Error);
                return;
            }

            var fileNames = (from arg in otherArgs where !(arg.StartsWith("-") || arg.StartsWith("/")) || arg.EndsWith(".txt") select arg).ToArray();
            if (fileNames.Length != 1)
            {
                Console.Error.WriteLine("Wordlist.exe receives exactly ONE wordlist file with extension .txt.");
                p.WriteOptionDescriptions(Console.Error);
                return;
            }

            if (!(outputsNumber || outputsMostWords || differentInitials || outputsMostLetters))
            {
                Console.Error.WriteLine("At least one option [-n|-w|-m|-c] should be given.");
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

            if (outputsNumber && (headLetter != null || tailLetter != null || allowingRings))
            {
                Console.Error.WriteLine("Option `-n' cannot be used with option `-h', `-t' or `-r'.");
                p.WriteOptionDescriptions(Console.Error);
                return;
            }

            if (differentInitials && (headLetter != null || tailLetter != null || allowingRings))
            {
                Console.Error.WriteLine("Option `-m' cannot be used with option `-h', `-t' or `-r'.");
                p.WriteOptionDescriptions(Console.Error);
                return;
            }

            fileName = fileNames.First();

            OperationType type;

            if (outputsNumber)
            {
                type = OperationType.All;
            }
            else if (differentInitials)
            {
                type = OperationType.Unique;
            }
            else if (outputsMostLetters)
            {
                type = OperationType.Longest;
            }
            else
            {
                type = OperationType.Most;
            }

            try
            {
                List<string> toWrite =
                    CallWrapper.CallCoreByOptions(fileName, type, headLetter ?? '\0', tailLetter ?? '\0', allowingRings);
                // Use wrapper to handle calling in order to avoid unsafe main function.
                if (toWrite.Count > 0)
                {
                    if (outputsNumber)
                    {
                        toWrite.ForEach(Console.WriteLine);
                    }
                    else
                    {
                        File.WriteAllText("solution.txt", string.Join("\n", toWrite));
                    // Gather and output results.
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return;
            }
        }
    }
}