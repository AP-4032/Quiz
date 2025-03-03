using System;


class Program
{
	public static void Main()
	{
		int[] stack = new int[1000];
		int cnt = 0;
		while (true)
		{
			int n = int.Parse(Console.ReadLine());
			if (n == 0) break;
			if (n == 1)
			{
				int nn = int.Parse(Console.ReadLine());
				stack[cnt++] = nn;
			}
			else if (n == 2) {
				Console.WriteLine(stack[--cnt]);
			}
		}
	}
}
