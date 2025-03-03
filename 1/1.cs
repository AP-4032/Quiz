using System;
class Program
{
	public static int rec(int[] coins, int N)
	{
		if (N < 0) return 100000;
		if (N == 0) return 0;
		int mi = 100000;
		for (int i = 0; i < coins.Length; i++)
		{
			int ss = 1 + rec(coins, N - coins[i]);
			if (ss < mi) mi = ss;
		}
		return mi;
	}
	public static void Main()
	{
		int num = int.Parse(Console.ReadLine());
		int[] input = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		int N = int.Parse(Console.ReadLine());
		Console.WriteLine(rec(input, N));
	}
}