using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient
{
    public class Lead
    {
        public string id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string leads_owner_id { get; set; }
        public string users_id { get; set; }
        public string companies_id { get; set; }
        public string leads_status_id { get; set; }
        public string is_duplicated { get; set; }
        public bool sendToQueue { get; set; }
        public string[] customFields { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string is_deleted { get; set; }
        public string description { get; set; }
        public string SubID { get; set; }
        public string antispam { get; set; }
        public string amount_requested { get; set; }
        public string annual_revenue { get; set; }
        public string credit_score { get; set; }
        public string business_founded { get; set; }
        public string industry { get; set; }
        public string programs { get; set; }
    }
}
