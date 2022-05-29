using DevExpress.Xpo;
using System;

namespace MediTech.Model
{
    public  class VISIT_HC
    {
      
        public Nullable<System.DateTime> TRIGGER_DTTM { get; set; }
        public string REPLICA_DTTM { get; set; }
        public string PATIENT_ID { get; set; }
        public string PATIENT_NAME { get; set; }
        public string OTHER_PATIENT_ID { get; set; }
        public string PATIENT_BIRTH_DATE { get; set; }
        public string PATIENT_SEX { get; set; }
        public string VISIT_TYPE { get; set; }
        public string VISIT_NUMBER { get; set; }
        public string VISIT_DTTM { get; set; }
        public string ORDER_TYPE { get; set; }
        public string ADD_ITEM_ID { get; set; }
        public string ADD_ITEM_NAME { get; set; }
        public string ADD_ITEM_PRICE { get; set; }
        public string ICHECKUP_NO { get; set; }
        public string PROJECT_CODE { get; set; }
        public string PACKAGE_CODE { get; set; }
        public string PACKAGE_NAME { get; set; }
        public string SUB_COMPANY_CODE { get; set; }
        public string EMPLOYEE_ID { get; set; }
        public string POSITION { get; set; }
        public string DEPARTMENT { get; set; }
        public string VISIT_STATUS { get; set; }
    }

}