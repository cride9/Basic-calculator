using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator {

    /* Simpler calculation handling */
    public enum MathType {
        ADD,
        TAKE,
        MULTIPLY,
        DIVIDE,
        POW,
    }

    internal class Calculator {

        /* Line reading */
        public Calculator(string lineIn) =>
           GetCalculationOrder(lineIn);

        double finalNumber = 0;

        /* Already used check (kinda dumb) */
        double alreadyUsedCheck = (int.MaxValue) - 10;

        /* Lambda functions for the calculations */
        private double Multiply(double a, double b) =>
            (a == alreadyUsedCheck ? finalNumber : a) * (b == alreadyUsedCheck ? finalNumber : b);

        private double Divide(double a, double b) =>
            (a == alreadyUsedCheck ? a = finalNumber : a) / (b == alreadyUsedCheck ? finalNumber : b);

        private double Add(double a, double b) =>
            (a == alreadyUsedCheck ? a = finalNumber : a) + (b == alreadyUsedCheck ? finalNumber : b);

        private double Take(double a, double b) =>
            (a == alreadyUsedCheck ? a = finalNumber : a) - (b == alreadyUsedCheck ? finalNumber : b);

        private double Pow(double a, double b) =>
            Math.Pow((a == alreadyUsedCheck ? finalNumber : a), (b == alreadyUsedCheck ? finalNumber : b));

        /* This list will hold every math problem to solve in order */
        private List<(double, double, MathType)> calculationOrder = new List<(double, double, MathType)>();
        private void GetCalculationOrder(string lineIn) {

            /* Reset number for the new calculation */
            finalNumber = 0;

            /* 2 array in math order */
            string[] mathSymbolsChar = { "^", "/", "*", "-", "+" };
            MathType[] mathSymbols = { MathType.POW, MathType.DIVIDE, MathType.MULTIPLY, MathType.TAKE, MathType.ADD };

            /* Check every symbol if it's used or not */
            /* TODO: operator multiplying check (20+20+20 is not recognised only the first "+" will be seen)*/
            for (int i = 0; i < mathSymbols.Length; i++) 
                if (lineIn.Contains(mathSymbolsChar[i])) 
                    AddCalculation(lineIn, mathSymbolsChar[i], mathSymbols[i]);
        }

        public double GetResult() {

            /* Loop through every calculation */
            foreach (var item in calculationOrder) {
                switch (item.Item3) {
                    case MathType.ADD:
                        finalNumber = Add(item.Item1, item.Item2);
                        break;
                    case MathType.TAKE:
                        finalNumber = Take(item.Item1, item.Item2);
                        break;
                    case MathType.MULTIPLY:
                        finalNumber = Multiply(item.Item1, item.Item2);
                        break;
                    case MathType.DIVIDE:
                        finalNumber = Divide(item.Item1, item.Item2);
                        break;
                    case MathType.POW:
                        finalNumber = Pow(item.Item1, item.Item2);
                        break;
                }
            }
            return finalNumber;
        }

        private void AddCalculation(string lineIn, string symbol, MathType mathSymbol) {

            /* Check for the operator */
            int symbolIndex = lineIn.IndexOf(symbol);
            if (symbolIndex == -1)
                return;

            /* Check for used indexes 2+2*2 (the middle 2 is already used so use the 2*2 and then + 2) */
            List<int> usedIndexes = new();

            /* Variables used in this function */
            string numberAfter = "", numberBefore = "";
            double outAfterNumber = 0, outBeforeNumber = 0;

            /* Search number before the operator while its a valid number (keep in mind this will be reversed) */
            while (--symbolIndex >= 0 && (double.TryParse(numberBefore, out outBeforeNumber) || numberBefore == "")) {

                if (!usedIndexes.Contains(symbolIndex)) {
                    usedIndexes.Add(symbolIndex);
                    numberBefore += lineIn[symbolIndex];
                    continue;
                }
                outBeforeNumber = alreadyUsedCheck;
                break;
            }

            /* Eliminate invalid symbol caused by overrun */
            if (numberBefore.Length > 1 && !double.TryParse(numberBefore, out outAfterNumber))
                numberBefore = numberBefore.Remove(numberBefore.Length - 1);

            /* Reset index and search for number after the symbol */
            symbolIndex = lineIn.IndexOf(symbol);
            while (++symbolIndex < lineIn.Length && (double.TryParse(numberAfter, out outAfterNumber) || numberAfter == "")) {

                if (!usedIndexes.Contains(symbolIndex)) {
                    usedIndexes.Add(symbolIndex);
                    numberAfter += lineIn[symbolIndex];
                    continue;
                }
                outAfterNumber = alreadyUsedCheck;
                break;
            }

            /* Eliminate invalid symbol caused by overrun */
            if (numberAfter.Length > 1 && !double.TryParse(numberAfter, out outAfterNumber))
                numberAfter = numberAfter.Remove(numberAfter.Length - 1);

            /* Add this calculation to the list */
            calculationOrder.Add((outBeforeNumber == alreadyUsedCheck ? outBeforeNumber : (double.Parse(new string(numberBefore.Reverse().ToArray()))), outAfterNumber == alreadyUsedCheck ? outAfterNumber : double.Parse(numberAfter), mathSymbol));
        }
    }
}
