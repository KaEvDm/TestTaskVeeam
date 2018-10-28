using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GZipTest
{
    class ReaderCreator
    {
        public IReader CreateReader(string source, TypeInfo sourceType)
        {
            IReader reader;

            switch (sourceType)
            {
                case TypeInfo.File:
                {
                    reader = new FileReader(source);
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
                    throw new Exception("Неверный тип источника данных!");
                }
            }
            return reader;
        }
    }
}
