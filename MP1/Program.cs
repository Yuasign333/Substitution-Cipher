using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1
{
    internal class Program
    {
        static void Main(string[] args)

            
        {
         

            // Pass Phrasing
            string PassPhrased = GetPassPhrase();

            // List of Alphabets with Pass Phrase
            List<char> keyList = GenerateKeyList(PassPhrased);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nSubstitution Key:");
            Console.WriteLine();
            Console.ResetColor();

            // Print all letters A-Z on one line

            for (int i = 0; i < 26; i++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{(char)('A' + i)} ");
            }

            Console.WriteLine();
            Console.WriteLine();

            for (int i = 0; i < 26; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{keyList[i]} ");
            }

            Console.WriteLine(); 
            Console.ResetColor();



            string word = GetWord();  // Get user input

            Console.ForegroundColor= ConsoleColor.Blue;
            string encrypted = Encrypt(word, keyList);  // Use Encrypt here
            Console.WriteLine($"\nEncrypted: {encrypted}");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string decrypted = Decrypt(encrypted, keyList);  // Use Decrypt here
            Console.WriteLine($"\nDecrypted: {decrypted}");

            Console.ResetColor();
            Console.WriteLine();
        }


        static string GetPassPhrase()
        {
            string phraseInput;
            bool valid = false;

            do
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\nEnter Pass Phrase: ");
                phraseInput = Console.ReadLine();
                Console.ResetColor();

                if (string.IsNullOrWhiteSpace(phraseInput) || int.TryParse(phraseInput, out _))
                {
                    Console.WriteLine("Please enter letters only.");
                }
                else
                {
                    valid = true;
                }
            } while (!valid);

            return phraseInput;
        }

        static string GetWord()
        {
            string wordInput;
            bool valid = false;

            do
            {
                Console.ForegroundColor= ConsoleColor.Cyan;
                Console.Write("\nEnter Word: ");
                wordInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(wordInput) || int.TryParse(wordInput, out _))
                {
                    Console.WriteLine("Please enter letters only.");
                }
                else
                {
                    valid = true;
                }
               
            } while (!valid);

            return wordInput;
        }


        static List<char> GenerateKeyList(string passphrase) // list of alphabets with pass phrase
        {
            if (string.IsNullOrEmpty(passphrase))
            {
                Console.WriteLine("\nPass Phrase should be not empty");
            }
            else
            {
                passphrase = passphrase.Trim().ToUpper();
            }
            List<char> keyList = new List<char>();

            // Add unique letters from passphrase
            foreach (char c in passphrase)
            {
                if (Char.IsLetter(c) && !keyList.Contains(c))
                    keyList.Add(c);
            }

            // Add remaining letters of alphabet
            for (char c = 'A'; c <= 'Z'; c++)
            {
                if (!keyList.Contains(c))
                    keyList.Add(c);
            }

            return keyList;
        }
       
     
           static string Encrypt(string word, List<char> keyList) // encrypt method
        {
            word = word.ToUpper();

            string result = "";

            foreach (char c in word)
            {
                if (Char.IsLetter(c))
                {
                    int index = c - 'A';
                    result += keyList[index];
                }
                else
                {
                    result += c;
                }
            }
            return result;
        }

        // Method to decrypt word
        static string Decrypt(string word, List<char> keyList)
        {
            word = word.ToUpper();
            string result = "";
            foreach (char c in word)
            {
                if (Char.IsLetter(c))
                {
                    int index = keyList.IndexOf(c);
                    result += (char)('A' + index);
                }
                else
                {
                    result += c;
                }
            }
            return result;


        }

      
    }
    
}
