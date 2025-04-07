using Chevron.Core.PackageModels;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Chevron.Core.Extensions
{
    public static class Gen
    {

      public static string GenerateHtmlView(this IndexModel item, string baseFolder) {
        if (item == null || string.IsNullOrEmpty(item.Location))
          return "<html><body><p>No file specified</p></body></html>";

        string filePath = item.Location;
        if (!File.Exists(filePath))
          return $"<html><body><p>File not found: {HttpUtility.HtmlEncode(filePath)}</p></body></html>";

        string extension = Path.GetExtension(filePath)?.ToLowerInvariant() ?? "";
        if (extension == null || extension == "") {
          return $"<p>Unsupported file type: {filePath}</p>";
        }
        string fileContent = extension switch {
          ".png" or ".jpg" or ".jpeg" or ".gif" => null,
          _ => File.ReadAllText(filePath)
        };
      
        string relativePath = Path.GetRelativePath(baseFolder, filePath).Replace("\\", "/");
        string virtualUrl = $"http://{Exts.LocalHostName}/{relativePath}";

        var sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("    <meta charset='UTF-8'>");
        sb.AppendLine("    <style>");
        sb.AppendLine("        body { font-family: Arial, sans-serif; margin: 0; padding: 20px; background: #f5f5f5; display: flex; justify-content: center; align-items: center; min-height: 100vh; }");
        sb.AppendLine("        pre { background: #1e1e1e; color: #d4d4d4; padding: 15px; border-radius: 5px; overflow-x: auto; max-width: 100%; }");
        sb.AppendLine("        img { max-width: 100%; max-height: 100vh; width: auto; height: auto; object-fit: contain; }");
        sb.AppendLine("        .unsupported { color: #888; }");
        sb.AppendLine("    </style>");
        sb.AppendLine("    <script src='https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/highlight.min.js'></script>");
        sb.AppendLine("    <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/styles/vs2015.min.css'>");
        sb.AppendLine("    <script>document.addEventListener('DOMContentLoaded', () => hljs.highlightAll());</script>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");

        switch (extension) {
          case ".txt":
            sb.AppendLine($"    <pre>{HttpUtility.HtmlEncode(fileContent)}</pre>");
            break;

          case ".cs":
          case ".js":
          case ".py":
          case ".html":
            string language = extension.TrimStart('.') switch {
              "cs" => "csharp",
              "js" => "javascript",
              "py" => "python",
              "html" => "html",
              _ => "text"
            };
            sb.AppendLine($"    <pre><code class='language-{language}'>{HttpUtility.HtmlEncode(fileContent)}</code></pre>");
            break;

          case ".png":
          case ".jpg":
          case ".jpeg":
          case ".gif":
            sb.AppendLine($"    <img src='{HttpUtility.HtmlEncode(virtualUrl)}' alt='Image'>");
            break;

          default:
            sb.AppendLine($"    <pre class='unsupported'>{HttpUtility.HtmlEncode(fileContent ?? "No content available")}</pre>");
            sb.AppendLine("    <p class='unsupported'>Preview not optimized for this file type.</p>");
            break;
        }

        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        return sb.ToString();
      }

    }
}
