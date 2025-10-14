using System.IO;
using System.Text;

namespace AlgoX.Code
{
    public class AlgoXHelper
    {
        private static bool AreThereAssociations = false;
        private static IList<char> targetLetters;
        private static IList<int> targetNumbers;

        private string Path { get; set; }
        private char[] LettersToConsider { get; set; }
        private int[] NumbersToConsider { get; set; }
        private Association<char>[] LetterAssociations { get; set; }
        private Association<int>[] NumberAssociations { get; set; }

        public AlgoXHelper (char[] lettersToConsider, int[] numbersToConsider, string path)
        {
            LettersToConsider = lettersToConsider;
            NumbersToConsider = numbersToConsider;
            Path = path;
            ReadAssociationsFromTxt();
            if (!AreThereAssociations)
            {
                SetupLists();
                WriteAssociationsInTxt(GenerateLetterAssociations(), GenerateNumberAssociations());
            }
        }

        public string EncodeMessage(string msg)
        {
            StringBuilder sb = new StringBuilder();
            foreach(char c in msg)
            {
                char newLetter = TranformElementToAssociated(c);
                sb.Append(newLetter);
            }
            return sb.ToString();
        }
        public string DecodeMessage(string msg)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in msg)
            {
                char newLetter = InvertElementToAssociated(c);
                sb.Append(newLetter);
            }
            return sb.ToString();
        }
        private char TranformElementToAssociated(char c)
        {
            bool isCapital = Char.IsUpper(c);
           
            if (Char.IsLetter(c))
            {
                int numLetter = Convert.ToInt32(Char.ToUpper(c)) - Convert.ToInt32('A'); //Letters input in the constructor must be capital
                return isCapital ? LetterAssociations[numLetter].AssociatedElement : Char.ToLower(LetterAssociations[numLetter].AssociatedElement);
            }
            else if (Char.IsDigit(c))
            {
                return (char)NumberAssociations[(int)char.GetNumericValue(c)].AssociatedElement;
            }
            return c;
        }
        private char InvertElementToAssociated(char c)
        {
            bool isCapital = Char.IsUpper(c);

            if (Char.IsLetter(c))
            {
                int numAssociatedLetter = 0; //Mala pràctica
                for (int i = 0; i < LetterAssociations.Length; i++)
                {
                    if (LetterAssociations[i].AssociatedElement == c)
                    {
                        numAssociatedLetter = i;
                        i = LetterAssociations.Length;
                    }
                }
                return isCapital ? LetterAssociations[numAssociatedLetter].Element : Char.ToLower(LetterAssociations[numAssociatedLetter].Element);
            }
            else if (Char.IsDigit(c))
            {
                int numAssociatedNumber = 0; //Mala pràctica
                for (int i = 0; i < NumberAssociations.Length; i++)
                {
                    if (NumberAssociations[i].AssociatedElement == c)
                    {
                        numAssociatedNumber = i;
                        i = NumberAssociations.Length;
                    }
                }
                return (char)NumberAssociations[numAssociatedNumber].Element;
            }
            return c;
        }

        private void SetupLists()
        {
            foreach (char letter in LettersToConsider)
            {
                targetLetters.Append(letter);
            }
            foreach (int num in NumbersToConsider)
            {
                targetNumbers.Append(num);
            }
        }
        private Association<char>[] GenerateLetterAssociations()
        {
            Association<char>[] letterAssociations = new Association<char>[LettersToConsider.Length];

            for (int i = 0; i < LettersToConsider.Length; i++)
            {
                int assNumber = RandomNumberBetweenRange(0, targetLetters.Count);
                Association<char> a = new Association<char>(LettersToConsider[i], targetLetters[assNumber]);
                letterAssociations[i] = a;
                targetLetters.RemoveAt(assNumber);
            }
            LetterAssociations = letterAssociations;
            return letterAssociations;
        }
        private Association<int>[] GenerateNumberAssociations()
        {
            Association<int>[] numberAssociations = new Association<int>[NumbersToConsider.Length];

            for (int i = 0; i < NumbersToConsider.Length; i++)
            {
                int assNumber = RandomNumberBetweenRange(0, targetLetters.Count);
                Association<int> a = new Association<int>(NumbersToConsider[i], targetNumbers[assNumber]);
                numberAssociations[i] = a;
                targetNumbers.RemoveAt(assNumber);
            }
            NumberAssociations = numberAssociations;
            return numberAssociations;
        }
        private void WriteAssociationsInTxt(Association<char>[] letterAss, Association<int>[] numberAss)
        {
            using (StreamWriter sw = new StreamWriter(Path, true))
            {
                foreach (Association<char> a in letterAss)
                {
                    sw.WriteLine($"{a.Element}{a.AssociatedElement}");
                }
                foreach (Association<int> a in numberAss)
                {
                    sw.WriteLine($"{a.Element}{a.AssociatedElement}");
                }
            }
        }
        private bool FileIsEmpty()
        {
            return new FileInfo(Path).Length == 0;
        }
        private void ReadAssociationsFromTxt()
        {
            if (FileIsEmpty()) return;
            else
            {
                using (StreamReader sr = new StreamReader(Path))
                {
                    string line;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (i < LettersToConsider.Length)
                        {
                            LetterAssociations[i] = new Association<char>(line[0], line[1]);
                        }
                        else
                        {
                            (int, int) nums = (
                                (int)char.GetNumericValue(line[0]),
                                (int)char.GetNumericValue(line[1])
                            );
                            NumberAssociations[i] = new Association<int>(nums.Item1, nums.Item2);
                        }
                        i++;
                    }
                }
                AreThereAssociations = true;
            }
        }
        private static int RandomNumberBetweenRange(int min, int max)
        {
            Random r = new Random();
            return r.Next(min, max + 1);
        }
    }
}