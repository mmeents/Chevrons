using System.Globalization;
using System.Text;
using System.Security.Cryptography;

namespace Chevron.Core.Extensions {
  public static class DataConvertExt {
    /// <summary>
    /// async read file from file system into string
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static async Task<string> ReadAllTextAsync(this string filePath) {
      using (var streamReader = new StreamReader(filePath)) {
        return await streamReader.ReadToEndAsync();
      }
    }

    /// <summary>
    /// async write content to fileName location on file system. 
    /// </summary>
    /// <param name="Content"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static async Task<int> WriteAllTextAsync(this string Content, string fileName) {
      using (var streamWriter = new StreamWriter(fileName)) {
        await streamWriter.WriteAsync(Content);
      }
      return 1;
    }

    public static string ComputeFileHash(this string filePath) {
      using var sha256 = SHA256.Create();
      using var stream = File.OpenRead(filePath);
      return BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", "").ToLower();
    }

    public static string ReplaceInvalidFileNameChars(this string input, string replacement) {
      var invalidChars = Path.GetInvalidFileNameChars();
      return string.Join(replacement, input.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
    }
    public static int AsInt32(this string value) {
      return int.Parse(value);
    }
    public static long AsInt64(this string value) {
      return long.Parse(value);
    }
    public static byte[] AsByte(this string value) {
      return Convert.FromBase64String(value);
    }
    public static bool AsBoolean(this string value) {
      return Convert.ToBoolean(value);
    }
    public static DateTime AsDateTime(this string value) {
      return DateTime.Parse(value);
    }
    public static Decimal AsDecimal(this string value) {
      return Decimal.Parse(value);
    }
    public static byte[] AsBytes(this string text) {
      return Encoding.UTF8.GetBytes(text);
    }
    public static string AsString(this byte[] bytes) {
      return Encoding.UTF8.GetString(bytes);
    }
    public static string AsString(this int value) {
      return value.ToString();
    }

    public static string AsString(this Guid value) {
      return value.ToString().RemoveChar('-');
    }

    public static int AsInt32(this Object value) {
      return Convert.ToInt32(value);
    }
    public static long AsInt64(this Object value) {
      return Convert.ToInt64(value);
    }
    public static byte[] AsBytes(this Object value) {
      return (byte[])value;
    }
    public static bool AsBoolean(this Object value) {
      return Convert.ToBoolean(value);
    }
    public static DateTime AsDateTime(this Object value) {
      return Convert.ToDateTime(value);
    }
    public static Decimal AsDecimal(this Object value) {
      return Convert.ToDecimal(value);
    }
    public static string AsString(this Object value) {
      return value.ToString();
    }

    public static Guid AsGuid(this Object value) {
      return Guid.Parse(value.ToString());
    }


    #region Date to string 
    /// <summary> Day to string Sortable yyyy-MM-dd</summary>
    /// <returns> string </returns>
    public static string AsStrDate(this DateTime x) {
      return String.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-dd}", x);
    }
    /// <summary> DateTime to string yyyy-MM-dd hh:mm:ss.FFF tt </summary>
    /// <returns> string </returns>
    public static string AsStrDateTime12H(this DateTime x) {
      return String.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-dd hh:mm:ss.FFF tt}", x);
    }
    /// <summary> DateTime to string yyyy-MM-dd HH:mm:ss.FFF</summary>
    /// <returns> string </returns>
    public static string AsStrDateTime24H(this DateTime x) {
      return String.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-dd HH:mm:ss.FFF}", x);
    }
    /// <summary> DateTime to string time h:mm:ss tt</summary>
    /// <returns> string </returns>
    public static string AsStrTime(this DateTime x) {
      return String.Format(CultureInfo.InvariantCulture, "{0:h:mm:ss tt}", x);
    }
    /// <summary> DateTime to string Day Time MM/dd/yyyy hh:mm</summary>
    /// <returns> string </returns>
    public static string AsStrDayHHMM(this DateTime x) {
      return String.Format(CultureInfo.InvariantCulture, "{0:MM/dd/yyyy hh:mm}", x);
    }
    /// <summary> DateTime to string Day MM/dd/yyyy</summary>
    /// <returns> string </returns>
    public static string AsStrDay(this DateTime x) {
      return String.Format(CultureInfo.InvariantCulture, "{0:MM/dd/yyyy}", x);
    }

    #endregion


    public static string[] Parse(this string content, string delims) {
      return content.Split(delims.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    }
    public static string ParseFirst(this string content, string delims) {
      string[] sr = content.Parse(delims);
      if (sr.Length > 0) {
        return sr[0];
      }
      return "";
    }
    public static string ParseLast(this string content, string delims) {
      string[] sr = content.Parse(delims);
      if (sr.Length > 0) {
        return sr[^1];
      }
      return "";
    }
    /// <summary> Remove all instances of CToRemove from content</summary>
    /// <returns> string </returns>
    public static string RemoveChar(this string content, char CToRemove) {
      string r = content;
      while (r.Contains(CToRemove)) {
        r = r.Remove(r.IndexOf(CToRemove), 1);
      }
      return r;
    }

  }
}