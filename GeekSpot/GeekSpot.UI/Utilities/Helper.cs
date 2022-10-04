using HtmlAgilityPack;
using System.Text;
using System.Linq;
using GeekSpot.Domain.Entities;

namespace GeekSpot.UI.Utilities
{
    public class Helper
    {
        public static IEnumerable<Post> GetTruncatedTextFromHtml(IEnumerable<Post> posts,int length)
        {
            return posts.Select(p =>
            {
                var text = GetTextFromHtml(p.Content);
                p.Content = $"{(text.Length < length ? text : text.Substring(0, length))}...";
                return p;
            });
        }

        private static string GetTextFromHtml(string htmlString)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlString);
            var root = doc.DocumentNode;
            var sb = new StringBuilder();
            foreach (var text in from node in root.DescendantsAndSelf()
                                 where !node.HasChildNodes
                                 let text = node.InnerText
                                 where !string.IsNullOrEmpty(text)
                                 select text)
            {
                sb.AppendLine(text.Trim());
            }

            return sb.ToString();
        }
    }
}
