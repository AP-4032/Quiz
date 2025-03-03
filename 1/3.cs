using System;

class Program
{
	public static void Bubble_sort(int[] ll, int n)
	{
		for (int i = 0; i < n - 1; i++)
		{
			for (int j = 0; j < n - i - 1; j++)
			{
				if (ll[j] > ll[j + 1])
				{
					int t = ll[j];
					ll[j] = ll[j + 1];
					ll[j + 1] = t;
				}
			}
		}
	}
	public static int Abs(int n)
	{
		if (n < 0)
		{
			n *= -1;
		}
		return n;
	}
	public static void Main()
	{
		int n = int.Parse(Console.ReadLine());
		int[] l = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		int[] ll = new int[n];
		Array.Copy(l, ll, n);
		Bubble_sort(ll, n);
		int[] ekh_soo = new int[n];
		int[] ekh_noo = new int[n];
		int soo = 0, noo = 0;
		for (int i = 0; i < n; i++)
		{
			soo += Abs(ekh_soo[i] = ll[i] - l[i]);
			noo += Abs(ekh_noo[i] = ll[n - i - 1] - l[i]);
		}
		if (soo <= noo)
		{
			foreach (var x in ekh_soo)
			{
				Console.Write(x + " ");
			}
		}
		else
		{
			foreach (var x in ekh_noo)
			{
				Console.Write(x + " ");
			}
		}
	}
}
