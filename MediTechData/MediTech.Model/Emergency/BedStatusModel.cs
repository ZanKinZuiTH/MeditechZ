using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class BedStatusModel : PatientInformationModel
    {
    public int No { get; set; }
    public string Status { get; set; } 
    public string Severe { get; set; }
    public int SevereID { get; set; }
    public string SevereCode { get; set; }
    public bool IsUsed { get; set; }
    public bool IsLevel0 { get; set; }
    public bool IsLevel1 { get; set; }
    public bool IsLevel2 { get; set; }
    public bool IsLevel3 { get; set; }
    }
}
