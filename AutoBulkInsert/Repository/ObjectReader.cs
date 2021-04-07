using System;
using System.Collections.Generic;

using System.Data;


namespace Repository
{
    class ObjectReader<T> : IDataReader
    {
        private IEnumerator<T> _enumerator;
        private TypePropertyLoader<T> _typePropertyLoader;
   
        /// <summary>
        /// Convert a source defined with the interface <see cref="IEnumerable{T}" to implementation of the 
        /// interface <see cref="IDataReader"/>/>
        /// </summary>
        /// <param name="dataSourceAsync">Input collection to be converted.</param>
        public ObjectReader(IEnumerable<T> dataSource)
        {
            CreateDataReader(dataSource);
            _typePropertyLoader = TypePropertyLoader<T>.Create<T>();
        }


        private IDataReader CreateDataReader(IEnumerable<T> dataSource)
        {
            if (dataSource == null)
            {
                throw new ArgumentNullException("dataSource", "The data source is Null. An IEnumerable instance is required.");
            }

            _enumerator = dataSource.GetEnumerator();


            return null;

        }

        public object this[int i] 
        {
            get 
            { 
                return _typePropertyLoader
                    .GetPropertyValue(i, _enumerator.Current) ?? DBNull.Value; 
            }
        }



        public object this[string name]
        {
            get 
            { 
                return _typePropertyLoader
                    .GetPropertyValue(name, _enumerator.Current) ?? DBNull.Value; 
            }
        }

        public int Depth =>  default(int);

        public bool IsClosed => _enumerator == null;

        public int RecordsAffected => default(int);

        public int FieldCount => _typePropertyLoader.GetFieldCount;

        public void Close()
        {
            _enumerator.Dispose();
        }

        public void Dispose()
        {
            _enumerator.Dispose();
        }

        public bool GetBoolean(int i)
        {
            return (Boolean)this[i];
        }

        public byte GetByte(int i)
        {
            return (Byte)this[i];
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            byte[] s = (byte[])this[i];
            int available = s.Length - (int)fieldOffset;
            if (available <= 0) return 0;

            int count = Math.Min(length, available);
            Buffer.BlockCopy(s, (int)fieldOffset, buffer, bufferoffset, count);
            return count;
        }

        public char GetChar(int i)
        {
            return (Char)this[i];
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            string s = (string)this[i];
            int available = s.Length - (int)fieldoffset;
            if (available <= 0) return 0;

            int count = Math.Min(length, available);
            s.CopyTo((int)fieldoffset, buffer, bufferoffset, count);
            return count;
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            return _typePropertyLoader
                .GetPropertyInfos[i]
                .PropertyType
                .Name;
        }

        public DateTime GetDateTime(int i)
        {
            return (DateTime)this[i];
        }

        public decimal GetDecimal(int i)
        {
            return (decimal)this[i];
        }

        public double GetDouble(int i)
        {
            return (double)this[i];
        }

        public Type GetFieldType(int i)
        {
            return _typePropertyLoader.GetPropertyInfos[i].PropertyType;
        }

        public float GetFloat(int i)
        {
            return (float)this[i];
        }

        public Guid GetGuid(int i)
        {
            return (Guid)this[i];
        }

        public short GetInt16(int i)
        {
            return (short)this[i];
        }

        public int GetInt32(int i)
        {
            return (int)this[i];
        }

        public long GetInt64(int i)
        {
            return (long)this[i];
        }

        public string GetName(int i)
        {
            return (string)this[i];
        }

        public int GetOrdinal(string name)
        {
            return _typePropertyLoader.GetFieldIndex(name);
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            return (string)this[i];
        }

        public object GetValue(int i)
        {
            return this[i];
        }

        public int GetValues(object[] values)
        {
            int count = values.Length;
            var propertyInfos = _typePropertyLoader.GetPropertyInfos;
            for (int i = 0; i < count; i++)
            { 
                values[i] = propertyInfos[i]; 
            }
            return count;
        }

        public bool IsDBNull(int i)
        {
            return this[i] is DBNull;
        }

        public bool NextResult()
        {
            return false;
        }

        public bool Read()
        {
            if (IsClosed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }                
            return _enumerator.MoveNext();
        }
    }
}
