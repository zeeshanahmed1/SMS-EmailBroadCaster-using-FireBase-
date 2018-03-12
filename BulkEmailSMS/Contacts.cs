using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace BulkEmailSMS
{


    public class Friends
    {

        public List<Contacts> data { get; set; }
    }



    public class Contacts
    {




        public string Category { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }

    }
}