
namespace Framework
{
    public static class Math
    {
        /// <summary>
        /// Modulo operator
        /// because % return a remainder
        /// </summary>
        public static int Mod(int k, int n) => (k %= n) < 0 ? k + n : k;
    }
}
