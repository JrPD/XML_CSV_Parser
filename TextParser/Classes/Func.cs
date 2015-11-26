using System.Configuration;
using System.IO;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace TextParser.Classes
{

	public static class Func
	{
		/// <summary>
		/// save string to disk
		/// </summary>
		/// <param name="res">text string</param>
		public static void SaveToFile(string res)
		{
			var path = GetFileName();
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			using (StreamWriter file = new System.IO.StreamWriter(GetFileName()))
			{
				file.Write(res);
			}
		}

		/// <summary>
		/// return output.xml location
		/// </summary>
		/// <returns>path to output file</returns>
		public static string GetFileName()
		{
			string path = HttpRuntime.AppDomainAppPath;

			// get file name from web.config
			string filename = ConfigurationManager.AppSettings["filePath"];
			if (filename.IsNullOrWhiteSpace())
			{
				filename = "App_Data\\output.txt";
			}
			return  path + filename;
		}
	}
}