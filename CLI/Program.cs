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
    enum OperationType
    {
        All,
        Unique,
        Longest,
        Most
    }

    internal class Parser
    {
        private static bool isAlphabet(char c)
        {
            if ((c <= 'z' && c >= 'a') || (c <= 'Z' && c >= 'A'))
            {
                return true;
            }

            return false;
        }

        public static unsafe int parse(string inputFile, char*[] words)
        {
            Console.WriteLine("Now parsing.");
            int wordNum = 0;
            string text = System.IO.File.ReadAllText(inputFile);
            string tempString = "";
            bool inWord = false;

            foreach (var c in text)
            {
                if (Parser.isAlphabet(c))
                {
                    tempString = tempString + c;
                    inWord = true;
                }
                else
                {
                    if (inWord)
                    {
                        tempString = tempString + '\0';
                        Console.WriteLine($"{tempString}");
                        words[wordNum] = (char*)Marshal.StringToHGlobalUni(tempString);
                        wordNum++;
                        tempString = "";
                    }

                    inWord = false;
                }
            }

            if (inWord)
            {
                tempString = tempString + '\0';
                Console.WriteLine($"{tempString}");
                words[wordNum] = (char*)Marshal.StringToHGlobalUni(tempString);
                wordNum++;
                tempString = "";
            }


            return wordNum;
        }
    }

    internal class CallWrapper
        // Wrapper module for calling Core, should be rewritten to call by loading lib(dll) at stage 2.
    {
        public static unsafe List<String> CallCoreByOptions(string inputFileName, OperationType type, char head, char tail, bool loop)
        {
            char*[] forWords = new char*[10501]; // Allocate space for ptr.
            int wordNum = 0;

            wordNum = Parser.parse(inputFileName, forWords);

            char*[] forResults = new char*[20501]; // Allocate space for ptr.
            int reNum = 0;

            List<string> re = new List<string>();

            
            switch (type)
            {
                case OperationType.All:
                    reNum = Core.Launcher.gen_chains_all(forWords, wordNum, forResults);
                    re.Add(reNum.ToString());
                    break;
                case OperationType.Unique:
                    reNum = Core.Launcher.gen_chain_word_unique(forWords, wordNum, forResults);
                    break;
                case OperationType.Longest:
                    reNum = Core.Launcher.gen_chain_char(forWords, wordNum, forResults, head, tail, loop);
                    break;
                case OperationType.Most:
                default:
                    reNum = Core.Launcher.gen_chain_word(forWords, wordNum, forResults, head, tail, loop);
                    break;
            }
            
            re.Add(reNum.ToString());
            for (int i = 0; i < reNum; i++)
            {
                re.Add(new string(forResults[i]));
            }

            /*
             // To test parser.
            re.Add(wordNum.ToString());
            for (int i = 0; i < wordNum; i++)
            {
                re.Add(new string(forWords[i]));
            }
            */

            return re;
            
        }
    }

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
                    "n", "Outputs the number of word chains, including nested chains.",
                    v => outputsNumber = v != null
                },
                {
                    "w", "Outputs the word chain containing the most number of words to solution.txt.",
                    v => outputsMostWords = v != null
                },
                {
                    "m",
                    "Outputs the word chain containing the most number of words with different initials to solution.txt.",
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
                            throw new OptionException(string.Format("Option `-h' requires a letter, not `{0}\'.", v),
                                "h");
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
                            throw new OptionException(string.Format("Option `-t' requires a letter, not `{0}\'.", v),
                                "t");
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
                Console.WriteLine(e.Message);
                p.WriteOptionDescriptions(Console.Error);
                return;
            }

            if (!otherArgs.Any())
            {
                p.WriteOptionDescriptions(Console.Error);
                return;
            }

            var invalidOptions =
                from junk in otherArgs where junk.StartsWith("-") && !junk.EndsWith(".txt") select junk;
            if (invalidOptions.Any())
            {
                Console.Error.WriteLine("Invalid option(s): {0}", string.Join(" ", invalidOptions));
                p.WriteOptionDescriptions(Console.Error);
                return;
            }

            var fileNames = (from junk in otherArgs where !junk.StartsWith("-") || junk.EndsWith(".txt") select junk)
                .ToArray();
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

            List<string> toWrite =
                CallWrapper.CallCoreByOptions(fileName, type, headLetter ?? '\0', tailLetter ?? '\0', allowingRings);
            // Use wrapper to handle calling in order to avoid unsafe main function.

            if (toWrite.Count > 0)
            {
                File.WriteAllText("solution.txt", string.Join("\n", toWrite));
                // Gather and output results.
            }
        }
    }
}