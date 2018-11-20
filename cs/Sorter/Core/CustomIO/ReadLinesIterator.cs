using System.Diagnostics;
using System.IO;
using System.Text;
using static Sorter.Core.CustomIO.Enumerable;

namespace Sorter.Core.CustomIO
{
    internal class ReadLinesIterator : Iterator<string>
    {
        private const int DefaultBufferSize = 4096;
        private readonly Encoding _encoding;
        private StreamReader _reader;

        private ReadLinesIterator(Encoding encoding, StreamReader reader)
        {
            Debug.Assert(encoding != null);
            Debug.Assert(reader != null);

            _encoding = encoding;
            _reader = reader;
        }

        public override bool MoveNext()
        {
            if (this._reader != null)
            {
                this.current = _reader.ReadLine();
                if (this.current != null)
                    return true;

                // To maintain 4.0 behavior we Dispose 
                // after reading to the end of the reader.
                Dispose();
            }

            return false;
        }

        protected override Iterator<string> Clone()
        {
            // NOTE: To maintain the same behavior with the previous yield-based
            // iterator in 4.0, we have all the IEnumerator<T> instances share the same 
            // underlying reader. If we have already been disposed, _reader will be null, 
            // which will cause CreateIterator to simply new up a new instance to start up
            // a new iteration. We cannot change this behavior due to compatibility 
            // concerns.
            return CreateIterator(_reader, _encoding);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_reader != null)
                    {
                        _reader.Dispose();
                    }
                }
            }
            finally
            {
                _reader = null;
                base.Dispose(disposing);
            }
        }

        internal static ReadLinesIterator CreateIterator(
            StreamReader reader,
            Encoding encoding)
        {
            return new ReadLinesIterator(encoding, reader);
        }
    }
}
