using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NSlicePerformanceTests.Markdown
{
    static class CommonRenderers
    {
        public static IMarkdown ToMarkdown(this string source)
        {
            return Markdown.Create(source);
        }

        public static IEnumerable<IMarkdown> ToMarkdown(this IEnumerable<string> source)
        {
            return source.Select(x => x.ToMarkdown());
        }

        public static IMarkdown Add(this IMarkdown markdown, string toAdd)
        {
            return Markdown.Create(() => markdown.Render() + toAdd);
        }

        public static IMarkdown Add(this IMarkdown markdown, CultureInfo culture, string toAdd, params object[] arguments)
        {
            return Markdown.Create(() => markdown.Render() + string.Format(culture, toAdd, arguments));
        }

        public static IMarkdown Add(this IMarkdown markdown, IMarkdown toAdd)
        {
            return Markdown.Create(() => markdown.Render() + toAdd.Render());
        }

        public static IMarkdown NewLine(this IMarkdown markdown)
        {
            return Markdown.Create(() => markdown.Render() + "  " + Environment.NewLine);
        }

        public static IMarkdown H1(this IMarkdown markdown)
        {
            return Markdown.Create(markdown, "#{0}");
        }

        public static IMarkdown H2(this IMarkdown markdown)
        {
            return Markdown.Create(markdown, "##{0}");
        }

        public static IMarkdown H3(this IMarkdown markdown)
        {
            return Markdown.Create(markdown, "###{0}");
        }

        public static IMarkdown Bold(this IMarkdown markdown)
        {
            return Markdown.Create(markdown, "**{0}**");
        }

        public static IMarkdown Italic(this IMarkdown markdown)
        {
            return Markdown.Create(markdown, "*{0}*");
        }

        public static IMarkdown Code(this IMarkdown markdown)
        {
            return Markdown.Create(() =>
                string.Join(
                    Environment.NewLine,
                    markdown
                        .Render()
                        .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                        .Select(x => "\t" + x)));
        }

        public static IMarkdown InlineCode(this IMarkdown markdown)
        {
            return Markdown.Create(markdown, "`{0}`");
        }

        public static IMarkdown AsLines(this IEnumerable<IMarkdown> markdowns)
        {
            return Markdown.Create(() => string.Join(Environment.NewLine, markdowns.Select(m => m.Render() + "  ")));
        }

        public static IMarkdown AsUnorderedList(this IEnumerable<IMarkdown> markdowns)
        {
            return Markdown.Create(() => markdowns.Select(m => Markdown.Create("- " + m.Render())).AsLines());
        }

        public static IMarkdown AsOrderedList(this IEnumerable<IMarkdown> markdowns)
        {
            return Markdown.Create(() => markdowns.Select((m, i) => Markdown.Create((i+1).ToString() + ". " + m.Render())).AsLines());
        }

        public static IMarkdown Concat(this IEnumerable<IMarkdown> markdowns)
        {
            return Markdown.Create(() => string.Join("", markdowns.Select(m => m.Render())));
        }
    }
}
