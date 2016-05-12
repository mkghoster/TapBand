using System;

public class HashUtils
{
	public static byte[] CalculateHash(byte[] input, bool hasHash)
	{
		long num = 693779765864729976L;
		int num2 = input.Length;
		if (hasHash)
		{
			num2 -= 8;
		}
		for (int i = 0; i < num2 / 8; i++)
		{
			long num3 = 0L;
			for (int j = 0; j < 8; j++)
			{
				num3 = num3 * 256L + (long)input[i * 8 + (7 - j)];
			}
			num = num * 13L + Convert.ToInt64(num3);
		}
		for (int k = 0; k < (num2 & 7); k++)
		{
			num = num * 13L + (long)input[num2 / 8 * 8 + k];
		}
		return BitConverter.GetBytes(num);
	}

	public static byte[] AddHash(byte[] data)
	{
		if (data == null || data.Length == 0)
		{
			return null;
		}
		byte[] array = CalculateHash(data, false);
		byte[] array2 = new byte[data.Length + array.Length];
		Buffer.BlockCopy(data, 0, array2, 0, data.Length);
		Buffer.BlockCopy(array, 0, array2, data.Length, array.Length);
		return array2;
	}

	public static bool IsHashValid(byte[] data)
	{
		if (data == null)
		{
			return false;
		}
		int num = 8;
		if (data.Length <= num)
		{
			return false;
		}
		long num2 = BitConverter.ToInt64(data, data.Length - num);
		long num3 = BitConverter.ToInt64(CalculateHash(data, true), 0);
		return num3 == num2;
	}

	public static byte[] RemoveHash(byte[] data)
	{
		if (data == null)
		{
			return null;
		}
		int num = 8;
		if (data.Length <= num)
		{
			return null;
		}
		byte[] array = new byte[data.Length - num];
		Buffer.BlockCopy(data, 0, array, 0, array.Length);
		return array;
	}
}
