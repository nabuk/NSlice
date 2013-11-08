using System;

namespace NSlicePerformanceTests.Markdown
{
    class LazyMarkdown : IMarkdown
    {
        private readonly Func<IMarkdown> _markdownFactory;
        private string _markdownString;
        private bool _wasRendered;

        public LazyMarkdown(Func<IMarkdown> markdownFactory)
        {
            _markdownFactory = markdownFactory;
        }

        public string Render()
        {
            if (!_wasRendered)
            {
                _markdownString = _markdownFactory().Render();
                _wasRendered = true;
            }
            return _markdownString;
        }

        public override string ToString()
        {
            return Render();
        }
    }
}
