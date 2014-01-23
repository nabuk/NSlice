using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NSliceTests.Helpers
{
    public class EnumerableMock<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> source;
        private readonly List<EnumeratorMock<T>> enumerators;

        public EnumerableMock(IEnumerable<T> source)
        {
            this.source = source;
            this.enumerators = new List<EnumeratorMock<T>>();
            this.Enumerators = new ReadOnlyCollection<EnumeratorMock<T>>(this.enumerators);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var enumerator = new EnumeratorMock<T>(this.source.GetEnumerator());
            this.enumerators.Add(enumerator);
            this.EnumeratorCreated(enumerator);
            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IList<EnumeratorMock<T>> Enumerators { get; private set; }

        public event Action<EnumeratorMock<T>> EnumeratorCreated = delegate { };
    }

    
}
