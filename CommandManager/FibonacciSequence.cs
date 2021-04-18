namespace CommandManager
{
    public static class FibonacciSequence
    {
        private static ulong Fib(ulong n)
        {
            var a = 0UL;
            var b = 1UL;

            for (var i = 0UL; i < n; i++)
            {
                var t = a + b;
                a = b;
                b = t;
            }

            return a;
        }

        public static ulong FirstFibonacciSumFor(ulong number)
        {
            var sum = 1UL;

            for (var i = 0UL; i < number; i++) 
                sum += Fib(sum);

            return sum;
        }
    }
}