using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSlicePerformanceTests.Markdown
{
    class Markdown : IMarkdown
    {
        private readonly string _markdown;

        public Markdown(string markdown)
        {
            _markdown = markdown;
        }

        #region IMarkdown
        public string Render()
        {
            return _markdown;
        }
        #endregion //IMarkdown

        public override string ToString()
        {
            return Render();
        }

        public static IMarkdown Empty
        {
            get
            {
                return new Markdown(string.Empty);
            }
        }

        public static IMarkdown Create(string markdown)
        {
            return new Markdown(markdown);
        }

        public static IMarkdown Create(string markdown, string formatString)
        {
            return new Markdown(string.Format(formatString, markdown));
        }

        public static IMarkdown Create(IMarkdown markdown, string formatString)
        {
            return new LazyMarkdown(() => new Markdown(string.Format(formatString, markdown.Render())));
        }

        public static IMarkdown Create(Func<string> getMarkdownString)
        {
            return new LazyMarkdown(() => new Markdown(getMarkdownString()));
        }

        public static IMarkdown Create(Func<IMarkdown> getMarkdown)
        {
            return new LazyMarkdown(getMarkdown);
        }

        public static IMarkdown Create<T>(Func<T, string> getItemRenderer, Func<T> getItem)
        {
            return Create(() => getItemRenderer(getItem()));
        }

        public static IMarkdown Create<T>(Func<T, IMarkdown> getItemRenderer, Func<T> getItem)
        {
            return new LazyMarkdown(() => getItemRenderer(getItem()));
        }
    }
}
