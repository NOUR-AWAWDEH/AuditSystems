using System.Text.RegularExpressions;

namespace AuditSystem.BusinessLogic.Utilities;
public static class FileUtilities
{
    public static string GetCleanFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return "Unknown";

        // Regex to match: filename_32characterID.extension
        var regex = new Regex(@"^(.+)_([a-f0-9]{32})(\.[^.]+)$", RegexOptions.IgnoreCase);
        var match = regex.Match(fileName);

        if (match.Success)
        {
            return match.Groups[1].Value + match.Groups[3].Value; // original name + extension
        }

        // If pattern doesn't match, return original filename
        return fileName;
    }


    public static string GetContentType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension switch
        {
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".ppt" => "application/vnd.ms-powerpoint",
            ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            ".txt" => "text/plain",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".zip" => "application/zip",
            ".rar" => "application/x-rar-compressed",
            _ => "application/octet-stream"
        };
    }
}