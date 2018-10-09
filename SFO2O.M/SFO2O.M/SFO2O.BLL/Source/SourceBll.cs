using SFO2O.DAL.Source;
using SFO2O.Model.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.BLL.Source
{
    public class SourceBll
    {
        public readonly SourceDal sourceDal = new SourceDal();

        public SourceEntity GetSourcePercentById(int cid)
        {
            return sourceDal.GetSourcePercentById(cid);
        }
    }
}
