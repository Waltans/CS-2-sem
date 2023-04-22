using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        private readonly byte[] standardArray;
        public int Length => standardArray.Length;
        private readonly int _hashCode;

        public ReadonlyBytes(params byte[] arr)
        {
            standardArray = arr ?? throw new ArgumentNullException();
            unchecked
            {
                var hash = 15151;
                for (var index = 0; index < standardArray.Length; index++)
                {
                    var standard = standardArray[index];
                    hash *= 166515;
                    hash += standard;
                }
                _hashCode = hash;
            }
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= Length)
                    throw new IndexOutOfRangeException();
                return standardArray[index];
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj?.GetType() == typeof(ReadonlyBytes))) return false;
            var bytesObj = (ReadonlyBytes)obj;
            if (Length.Equals(bytesObj.Length))
            {
                for (var i = 0; i < Length; i++)
                    if (!standardArray[i].Equals(bytesObj[i]))
                        return false;
                return true;
            }
            return false;
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<byte> GetEnumerator()
        {
            return ((IEnumerable<byte>)standardArray)?.GetEnumerator();
        }

        public override int GetHashCode() => _hashCode;

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append('[');
            for (var i = 0; i < Length; i++)
            {
                if (i > 0)
                    stringBuilder.Append(", ");
                stringBuilder.Append(standardArray[i]);
            }
            return stringBuilder.Append(']').ToString();
        }
    }
}