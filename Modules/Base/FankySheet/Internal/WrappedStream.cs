using System;
using System.IO;

namespace FankySheet.Internal
{
    public class WrappedStream : Stream
    {
        private Stream _inner;

        public WrappedStream(Stream inner)
        {
            _inner = inner;
        }

        protected override void Dispose(bool disposing)
        {
            _inner = null;
            base.Dispose(disposing);
        }

        public override void Flush()
        {
            _inner.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        private int _position;
        public override void Write(byte[] buffer, int offset, int count)
        {
            _inner.Write(buffer, offset, count);
            _position += count;
        }

        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => _inner.CanWrite;
        public override long Length
        {
            get
            {
                throw new NotSupportedException();
            }
        }


        public override long Position
        {
            get { return _position; }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}