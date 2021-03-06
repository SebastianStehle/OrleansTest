﻿using Orleans.Serialization;
using System;
using System.IO;

namespace OrleansTest.Grains
{
    internal sealed class StreamReaderWrapper : Stream
    {
        private readonly IBinaryTokenStreamReader reader;

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return reader.Length; }
        }

        public override long Position
        {
            get
            {
                return reader.CurrentPosition;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public StreamReaderWrapper(IBinaryTokenStreamReader reader)
        {
            this.reader = reader;
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var bytesLeft = reader.Length - reader.CurrentPosition;

            if (bytesLeft < count)
            {
                count = bytesLeft;
            }

            reader.ReadByteArray(buffer, offset, count);

            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}
