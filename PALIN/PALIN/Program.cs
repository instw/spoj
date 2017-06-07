using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PALIN
{
    class Program
    {
        private static char[] IncrementNumber(char[] characters)
        {
            bool incremented = false;
            for (int index = characters.Length - 1; index >= 0; index--)
            {
                if (characters[index] != '9')
                {
                    characters[index]++;
                    incremented = true;
                    break;
                }
                else
                {
                    characters[index] = '0';
                }
            }

            if (!incremented)
            {
                List<char> resultList = new List<char>();
                resultList.Add('1');
                for (int index = 0; index < characters.Length; index++)
                {
                    resultList.Add('0');
                }

                resultList.Add('0');
                return resultList.ToArray();
            }
            else
            {
                return characters;
            }
        }
        private static string IncrementNumber(string input)
        {
            char[] inputCharacters = input.ToCharArray();
            bool incremented = false;
            for(int index = inputCharacters.Length - 1; index >= 0; index--)
            {
                if(inputCharacters[index] != '9')
                {
                    inputCharacters[index]++;
                    incremented = true;
                    break;
                }
                else
                {
                    inputCharacters[index] = '0';
                }
            }

            if (!incremented)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append('1');
                for(int index = 0; index < inputCharacters.Length; index++)
                {
                    stringBuilder.Append('0');
                }

                return stringBuilder.ToString();
            }
            else
            {
                return new string(inputCharacters);
            }
        }

        static void Main(string[] args)
        {
            int numberOfTests = Convert.ToInt32(Console.ReadLine());
            for(int testCase = 0; testCase < numberOfTests; testCase++)
            {
                string input = Console.ReadLine().Trim();
                input = Program.IncrementNumber(input);
                char[] inputCharacters = input.ToCharArray();
                // Single characters simple increments
                if(inputCharacters.Length == 1)
                {
                    Console.WriteLine(inputCharacters[0]);
                    continue;
                }

                int firstHalfSize = (inputCharacters.Length / 2) + inputCharacters.Length % 2;
                char[] firstHalf = inputCharacters.Take(firstHalfSize).ToArray();
                int oddNumberOffset = inputCharacters.Length % 2;
                char[] secondHalf = inputCharacters.Skip(firstHalfSize).Reverse().ToArray();
                
                for(int index = firstHalf.Length-1; index>=0; index--)
                {
                    if(secondHalf.Length - index < 1)
                    {
                        continue;
                    }

                    if(firstHalf[index] > secondHalf[index])
                    {
                        break;
                    }
                    else if(firstHalf[index] < secondHalf[index])
                    {
                        firstHalf = Program.IncrementNumber(firstHalf);
                        break;
                    }
                }
                
                for(int index = 0; index < firstHalf.Length; index++)
                {
                    Console.Write(firstHalf[index]);
                }


                for(int index = firstHalf.Length - 1 - oddNumberOffset; index >= 0; index--)
                {
                    Console.Write(firstHalf[index]);
                }

                Console.WriteLine();
            }
        }
    }
}
