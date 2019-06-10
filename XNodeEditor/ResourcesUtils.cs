using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace XNodeEditor
{
	class ResourcesUtils
	{
		// loads a png resources from the GamebookEditor dll
		public static Texture2D LoadDllPngResource(string resourceName, int width, int height)
		{
			// first try to load as local resource, in case not running dll version
			// also lets you override dll resources locally for rapid iteration

			Texture2D texture = (Texture2D)Resources.Load(resourceName);
			if (texture != null)
			{
				Debug.Log("Loaded local resource: " + resourceName);
				return texture;
			}

			// if unavailable, try assembly
			Assembly assembly = Assembly.GetExecutingAssembly();
			Stream resourceStream = assembly.GetManifestResourceStream("XNodeEditor.Resources." + resourceName + ".png");

			if (resourceStream != null)
			{
				texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
				texture.LoadImage(ReadToEnd(resourceStream));

				if (texture == null)
				{
					Debug.LogError("Unable to create texture for resource: " + resourceName);
				}

				return texture;
			}
			Debug.LogError("Missing Dll resource: " + resourceName);
			return null;
		}

		static byte[] ReadToEnd(Stream stream)
		{
			long originalPosition = stream.Position;
			stream.Position = 0;

			try
			{
				byte[] readBuffer = new byte[4096];

				int totalBytesRead = 0;
				int bytesRead;

				while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
				{
					totalBytesRead += bytesRead;

					if (totalBytesRead == readBuffer.Length)
					{
						int nextByte = stream.ReadByte();
						if (nextByte != -1)
						{
							byte[] temp = new byte[readBuffer.Length * 2];
							Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
							Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
							readBuffer = temp;
							totalBytesRead++;
						}
					}
				}

				byte[] buffer = readBuffer;
				if (readBuffer.Length != totalBytesRead)
				{
					buffer = new byte[totalBytesRead];
					Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
				}
				return buffer;
			}
			finally
			{
				stream.Position = originalPosition;
			}
		}
	}
}
