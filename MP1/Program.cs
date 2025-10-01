using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MP1
{
    internal class Program
    {
        /// <summary>
        /// The main entry point of the program. Manages the continuous loop 
        /// for key generation, encryption/decryption, and the restart prompt.
        /// </summary>
        static void Main(string[] args)

            
        {
            bool keepRunning = false;

            do 
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

                ProcessEncryptionDecryption(keyList); // Encryption and Decryption based on word

                
                Console.Write(" \nDo you want to restart? Y for Yes and Any Key for No: ");
                string userChoice = Console.ReadLine();

                if (userChoice == "Y")
                {
                    Console.Clear();
                
                    Console.Write("\nRestarting the Program");

                    for ( int i = 0; i <= 3; i++)
                    {
                        Thread.Sleep(750);
                        Console.Write(".");
                    }
                    Console,WriteLine();
                    Console.WriteLine("\nPress Any Key to Continue...");
                    Console.ReadKey();
                 
                    Console.Clear();
                }
               else
                {
                    keepRunning = true;
                }

                            
                Console.ResetColor();
                Console.WriteLine();

            } while (!keepRunning); // end 
      
        }

        /// <summary>
        /// Prompts the user to enter a Pass Phrase and validates that the input 
        /// is not empty and contains letters (non-numeric).
        /// </summary>
        /// <returns>The validated Pass Phrase as a string.</returns>
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
                    Console.WriteLine("\nPlease enter letters only.");
                }
                else
                {
                    valid = true;
                }
            } while (!valid);

            return phraseInput;
        }

        /// <summary>
        /// Generates the substitution cipher key list (26 characters) by 
        /// placing unique letters from the passphrase first, followed by 
        /// the remaining letters of the alphabet (A-Z).
        /// </summary>
        /// <param name="passphrase">The user-provided pass phrase used for key generation.</param>
        /// <returns>A List of characters representing the 26-letter substitution key.</returns>
        /// 
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

        /// <summary>
        /// Prompts the user to choose between Encrypt (E) or Decrypt (D), 
        /// gets the word/key from the user, and performs the selected operation.
        /// </summary>
        /// <param name="keyList">The substitution key list generated by the passphrase.</param>
        static void ProcessEncryptionDecryption(List<char> keyList)
        {
            string userChoice;
            bool validChoice = false;

            do
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("\nYou want to Encrypt It or Decrypt It? (E/D): ");
                userChoice = Console.ReadLine().ToUpper();

                if (userChoice == "E" || userChoice == "D")
                {
                    validChoice = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\nInvalid input. Please enter only 'E' for Encrypt or 'D' for Decrypt.");
                }
                Console.ResetColor();

            } while (!validChoice);

          
            string key = GetKey(); 

            // 3. Process the word based on the choice
            if (userChoice == "E")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                string encrypted = Encrypt(key, keyList);
                Console.WriteLine($"\nEncrypted: {encrypted}");
            }
            else // userChoice must be "D"
            {
           
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                // Decrypt the word the user entered (assuming it's ciphertext)
                string decrypted = Decrypt(key, keyList);
                Console.WriteLine($"\nDecrypted: {decrypted}");
            }
            Console.ResetColor();
        }
        /// <summary>
        /// Encrypts the given plaintext word using the generated substitution key list.
        /// Non-alphabetic characters are preserved.
        /// </summary>
        /// <param name="word">The word to be encrypted (plaintext).</param>
        /// <param name="keyList">The substitution key list.</param>
        /// <returns>The encrypted ciphertext string.</returns>
        static string Encrypt(string word, List<char> keyList) 
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

        /// <summary>
        /// Decrypts the given ciphertext word by reversing the substitution using the key list.
        /// Non-alphabetic characters are preserved.
        /// </summary>
        /// <param name="word">The word to be decrypted (ciphertext).</param>
        /// <param name="keyList">The substitution key list.</param>
        /// <returns>The decrypted plaintext string.</returns>
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
        /// <summary>
        /// Prompts the user to enter the word or cipher key to be processed 
        /// and validates that the input is not empty and contains letters 
        /// (non-numeric).
        /// </summary>
        /// <returns>The validated input word/key as a string.</returns>
        static string GetKey()
        {
            string wordInput;
            bool valid = false;

            do
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\nEnter Key: ");
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


    }
    
}
