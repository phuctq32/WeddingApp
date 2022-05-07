using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeddingApp.Models
{
    public class Data
    {
        private static Data _ins;
        public static Data Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new Data();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        public WeddingDBEntities DB { get; set; }
        private Data()
        {
            DB = new WeddingDBEntities();
        }
    }
}