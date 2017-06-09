
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARITH
{
    public static class Program
    {
        private static char[] Operators = new[] { '+', '-', '*' };

        static void Main(string[] args)
        {
            int numberOfTests = Convert.ToInt32(Console.ReadLine());
            for(int testCase = 0; testCase < numberOfTests; testCase++)
            {
                string expression = Console.ReadLine();
                int operatorIndex = expression.IndexOfAny(Program.Operators);
                char[] leftHandSide = expression.Substring(0, operatorIndex).ToCharArray();
                char[] rightHandSide = expression.Substring(operatorIndex + 1).ToCharArray();
                char arithmeticOperator = expression[operatorIndex];

                char[][] intermediateSteps = null;
                char[] result = null;
                switch (arithmeticOperator)
                {
                    case '+':
                        result = Sum(leftHandSide, rightHandSide);
                        break;
                    case '-':
                        result = Difference(leftHandSide, rightHandSide);
                        break;
                    case '*':
                        result = Multiply(leftHandSide, rightHandSide, out intermediateSteps);
                        break;
                }

                Program.PrintResults(leftHandSide, arithmeticOperator, rightHandSide, intermediateSteps, result);
                Console.WriteLine();
            }
        }

        private static void PrintResults(char[] firstInput, char arithmeticOperator, char[] secondInput, char[][] intermediateSteps, char[] result)
        {
            string resultString = Program.ConvertWithoutZeroes(result);
            int maxColumnWidth = GetMaximum(firstInput.Length, secondInput.Length + 1, resultString.Length);

            string firstNumberAligned = new string(firstInput).PadLeft(maxColumnWidth);

            string secondNumber = new string(secondInput);
            string secondNumberWithOperator = arithmeticOperator + secondNumber;
            string secondNumberAligned = secondNumberWithOperator.PadLeft(maxColumnWidth);
            Console.WriteLine(firstNumberAligned);
            Console.WriteLine(secondNumberAligned);

            if(arithmeticOperator != '*' || intermediateSteps.Length == 1)
            {
                int lineWidth = GetMaximum(secondInput.Length + 1, resultString.Length);
                string horizontalLine = new string('-', lineWidth).PadLeft(maxColumnWidth);
                Console.WriteLine(horizontalLine);
                string resultAligned = resultString.PadLeft(maxColumnWidth);
                Console.WriteLine(resultAligned);
            }
            else
            {
                string firstIntermediateStepString = Program.ConvertWithoutZeroes(intermediateSteps[0]);
                int firstHorizontalLine = GetMaximum(secondInput.Length+1, firstIntermediateStepString.Length);
                string horizontalLine = new string('-', firstHorizontalLine).PadLeft(maxColumnWidth);
                Console.WriteLine(horizontalLine);
                for(int offset = 0; offset < intermediateSteps.Length; offset++)
                {
                    char[] printableNumber = intermediateSteps[offset].Take(intermediateSteps[offset].Length - offset).ToArray();
                    int padding = maxColumnWidth - offset;
                    string intermediateStepString = Program.ConvertWithoutZeroes(printableNumber);
                    string intermediateStepAligned = intermediateStepString.PadLeft(padding);
                    Console.WriteLine(intermediateStepAligned);
                }

                horizontalLine = new string('-', resultString.Length).PadLeft(maxColumnWidth);
                Console.WriteLine(horizontalLine);
                string resultAligned = resultString.PadLeft(maxColumnWidth);
                Console.WriteLine(resultAligned);
            }
        }

        private static string ConvertWithoutZeroes(char[] number)
        {
            bool zeroSkipping = true;
            StringBuilder stringBuilder = new StringBuilder();
            for(int index = 0; index < number.Length; index++)
            {
                if(zeroSkipping && number[index] == '0')
                {
                    continue;
                }

                zeroSkipping = false;
                stringBuilder.Append(number[index]);
            }

            if (zeroSkipping)
            {
                return "0";
            }

            string result = stringBuilder.ToString();
            return result;
        }

        private static int GetMaximum(params int[] list)
        {
            int result = list.Max();
            return result;
        }


        private static char[] Sum(char[] one, char[] two)
        {
            int carryOver = 0;
            int largerSize = one.Length > two.Length ? one.Length : two.Length;
            int diffIndexOne = largerSize - one.Length;
            int diffIndexTwo = largerSize - two.Length;
            List<char> reversedResult = new List<char>();
            for (int index = largerSize - 1; index >= 0; index--)
            {
                char charOne = '0', charTwo = '0';
                if(index - diffIndexOne >= 0)
                {
                    charOne = one[index - diffIndexOne];
                }
                if(index - diffIndexTwo >= 0)
                {
                    charTwo = two[index - diffIndexTwo];
                }

                int numberOne = charOne.ToInteger();
                int numberTwo = charTwo.ToInteger();
                int sumDigits = numberOne + numberTwo + carryOver;
                int sumUnitDigit = sumDigits % 10;
                carryOver = sumDigits / 10;

                char sumUnitDigitChar = sumUnitDigit.ToCharacter();
                reversedResult.Add(sumUnitDigitChar);
            }

            if(carryOver != 0)
            {
                reversedResult.Add(carryOver.ToCharacter());
            }

            reversedResult.Reverse();
            char[] result = reversedResult.ToArray();
            return result;
        }

        private static int ToInteger(this char character)
        {
            int result = character - '0';
            return result;
        }

        private static char ToCharacter(this int integer)
        {
            if(integer >= 10)
            {
                throw new InvalidOperationException("Must be a single integer");
            }

            char result = (char)(integer + '0');
            return result;
        }

        private static char[] Difference(char[] one, char[] two)
        {
            bool carryForward = false;
            List<char> reversedResult = new List<char>();
            int diffInLength = one.Length - two.Length;
            // The problem statement states that one will always be larger than two
            for(int index = one.Length - 1; index >= 0; index--)
            {
                char charOne = one[index];
                char charTwo = '0';
                if(index - diffInLength >= 0)
                {
                    charTwo = two[index - diffInLength];
                }

                int digitOne = charOne.ToInteger();
                int digitTwo = charTwo.ToInteger();
                if (carryForward)
                {
                    if(digitOne == 0)
                    {
                        digitOne = 9;
                        carryForward = true;
                    }
                    else
                    {
                        digitOne--;
                        carryForward = false;
                    }
                }

                if(digitOne < digitTwo)
                {
                    carryForward = true;
                    digitOne += 10;
                }

                int resultOfSubtraction = digitOne - digitTwo;
                char resultChar = resultOfSubtraction.ToCharacter();
                reversedResult.Add(resultChar);
            }

            if (carryForward)
            {
                throw new InvalidOperationException("CarryForward shouldn't be true at the end of the execution if the first number is larger than the second");
            }

            reversedResult.Reverse();
            char[] result = reversedResult.ToArray();
            return result;
        }

        private static char[] Multiply(char[] one, char[] two, out char[][] intermediateSteps)
        {
            intermediateSteps = null;
            List<char[]> intermediateStepsList = new List<char[]>(two.Length);
            char[] result = new char[] { '0' };
            for(int twoIndex = two.Length - 1; twoIndex >= 0; twoIndex--)
            {
                int carryOver = 0;
                int twoDigit = two[twoIndex].ToInteger();
                List<char> intermediateStepResult = new List<char>();
                for(int oneIndex = one.Length - 1; oneIndex >= 0; oneIndex--)
                {
                    int oneDigit = one[oneIndex].ToInteger();
                    int multiplyResult = (oneDigit * twoDigit) + carryOver;
                    int resultDigit = multiplyResult % 10;
                    carryOver = multiplyResult / 10;
                    intermediateStepResult.Add(resultDigit.ToCharacter());
                }

                if(carryOver != 0)
                {
                    intermediateStepResult.Add(carryOver.ToCharacter());
                }

                intermediateStepResult.Reverse();
                for(int extraZeroes = twoIndex; extraZeroes < two.Length - 1; extraZeroes++)
                {
                    intermediateStepResult.Add('0');
                }

                char[] intermediateStepResultArray = intermediateStepResult.ToArray();
                intermediateStepsList.Add(intermediateStepResultArray);
                result = Sum(intermediateStepResult.ToArray(), result);
            }

            intermediateSteps = intermediateStepsList.ToArray();
            return result;
        }
    }
}
