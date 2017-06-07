

namespace ONP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Program
    {
        private static IDictionary<char, int> OperatorPriority =
            new Dictionary<char, int>()
            {
                { '+', 1 },
                { '-', 2 },
                { '*', 3 },
                { '/', 4 },
                { '^', 5 }
            };
        static void Main(string[] args)
        {
            int numberOfTests = Convert.ToInt32(Console.ReadLine());
            for(int testCase = 0; testCase < numberOfTests; testCase++)
            {
                string expressionString = Console.ReadLine();
                Stack<char> operators = new Stack<char>();
                char[] expression = expressionString.ToCharArray();
                foreach(char currentCharacter in expression)
                {
                    if (Char.IsWhiteSpace(currentCharacter))
                    {
                        continue;
                    }

                    if (Program.IsOperand(currentCharacter))
                    {
                        Console.Write(currentCharacter);
                    }
                    else if (Program.IsOperator(currentCharacter))
                    {
                        char topOfStack;
                        while (operators.Any())
                        {
                            topOfStack = operators.Pop();
                            if(topOfStack == '(')
                            {
                                operators.Push(topOfStack);
                                break;
                            }
                            else if(OperatorPriority[topOfStack] < OperatorPriority[currentCharacter])
                            {
                                operators.Push(topOfStack);
                                break;
                            }
                            else
                            {
                                Console.Write(topOfStack);
                            }
                        }

                        operators.Push(currentCharacter);
                    }
                    else if(currentCharacter == '(')
                    {
                        operators.Push(currentCharacter);
                    }
                    else if(currentCharacter == ')')
                    {
                        char topOfStack;
                        while (operators.Any())
                        {
                            topOfStack = operators.Pop();
                            if(topOfStack == '(')
                            {
                                break;
                            }
                            else
                            {
                                Console.Write(topOfStack);
                            }
                        }
                    }
                }

                while (operators.Any())
                {
                    char remainingOperator = operators.Pop();
                    Console.Write(remainingOperator);
                }

                Console.WriteLine();
            }
        }

        private static bool IsOperator(char character)
        {
            bool result = Program.OperatorPriority.ContainsKey(character);
            return result;
        }

        private static bool IsOperand(char character)
        {
            bool result = Char.IsLetter(character);
            return result;
        }
    }
}
