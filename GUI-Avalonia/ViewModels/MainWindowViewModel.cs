using GUI_Avalonia.Utilities;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GUI_Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string inputFileName = "";
        public string outputFileName = "";
        public bool canLoop = false;
        public int generateType = 0;

        public string headLetter = "";

        public string tailLetter = "";

        public string InputFileName
        { get => inputFileName; set => this.RaiseAndSetIfChanged(ref inputFileName, value); }
        public string OutputFileName
        { get => outputFileName; set => this.RaiseAndSetIfChanged(ref outputFileName, value); }

        public bool CanLoop
        { get => canLoop; set => this.RaiseAndSetIfChanged(ref canLoop, value); }

        public int GenerateType
        { get => generateType; set => this.RaiseAndSetIfChanged(ref generateType, value);}

        public string HeadLetter
        { get => headLetter; set => this.RaiseAndSetIfChanged(ref headLetter, value); }

        public string TailLetter
        { get => tailLetter; set => this.RaiseAndSetIfChanged(ref tailLetter, value); }

        public void Generate()
        {
            OperationType type = OperationType.All;
            char head = '\0';
            char tail = '\0';

            if (headLetter.Length>0&&char.IsLetter(headLetter[0]))
            {
                head = headLetter[0];
            }

            if (tailLetter.Length > 0 && char.IsLetter(tailLetter[0]))
            {
                tail = tailLetter[0];
            }

            switch (generateType)
            {
                case 0:
                    type = OperationType.All;
                    break;
                case 1:
                    type = OperationType.Unique;
                    break;
                case 2:
                    type = OperationType.Longest;
                    break;
                case 3:
                    type = OperationType.Most;
                    break;
                default:
                    type = OperationType.All;
                    break;
            }

            string inputFile = "wordlist.txt";
            string outputFile = "solution.txt";

            if (inputFileName.Length > 0)
            {
                inputFile = inputFileName;
            }

            if (outputFileName.Length > 0)
            {
                outputFile = outputFileName;
            }

            try
            {
                List<string> toWrite =
                    CallWrapper.CallCoreByOptions(inputFile, type, head, tail, canLoop);
                // Use wrapper to handle calling in order to avoid unsafe main function.
                if (toWrite.Count > 0)
                {
                    File.WriteAllText(outputFile, string.Join("\n", toWrite));
                    // Gather and output results.
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(outputFile, ex.Message);
                return;
            }
        }
    }
}
