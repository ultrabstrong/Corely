using Corely.DevTools.Attributes;

namespace Corely.DevTools.Commands
{
    internal class Add : CommandBase
    {
        private int Num1 { get; init; }
        private int Num2 { get; init; }

        [Option("-m", "-mult", Description = "Multiply numbers by this before adding")]
        private int Mult { get; init; } = 1;

        public Add() : base(nameof(Add), "Add two numbers together")
        {
        }

        public override void Execute()
        {
            var final = Num1 * Mult + Num2 * Mult;
            Console.WriteLine($"{Num1}*{Mult} + {Num2}*{Mult} = {final}");
        }
    }
}
