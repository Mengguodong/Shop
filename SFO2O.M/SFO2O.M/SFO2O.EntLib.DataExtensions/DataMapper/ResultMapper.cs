using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFO2O.EntLib.DataExtensions.DataMapper;
using Microsoft.Practices.EnterpriseLibrary.Data;
namespace SFO2O.EntLib.DataExtensions.DataMapper
{
    public class ResultMapper<T> : IResultSetMapper<T>, IRowMapper<T>
    {
        public IEnumerable<T> MapSet(System.Data.IDataReader reader)
        {
            var mapper = Mapping.GetIMapper<T>();
            return mapper.ToList<T>(reader);
        }
        public T MapRow(System.Data.IDataRecord row)
        {
            var mapper = Mapping.GetIMapper<T>();
            T target = mapper.Create();
            mapper.Load<T>(target, row);
            return target;
        }
    }
}
