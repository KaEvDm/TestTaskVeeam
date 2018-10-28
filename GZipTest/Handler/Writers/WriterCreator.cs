using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GZipTest
{
    public class WriterCreator
    {
        public IWriter CreateWriter(string result, TypeInfo resultType)
        {
            IWriter writer;
            switch (resultType)
            {
                case TypeInfo.File:
                {
                    writer = new FileWriter(result);
                    break;
                }
                case TypeInfo.DataBase:
                {
                    throw new NotImplementedException();
                }
                case TypeInfo.Web:
                {
                    throw new NotImplementedException();
                }
                default:
                {
                    throw new Exception("Неверный тип итоговых данных!");
                }
            }
            return writer;
        }
    }
}
