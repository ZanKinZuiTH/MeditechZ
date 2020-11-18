using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class CheckupBookModel : PatientResultLabModel
    {
        public string RadiologyResultText { get; set; }
        public string RadiologyResultStatus { get; set; }
        public string WellnessResult { get; set; }
        public string Eyes { get; set; }
        public string Ears { get; set; }
        public string Throat { get; set; }
        public string Nose { get; set; }
        public string Teeth { get; set; }
        public string LymphNode { get; set; }
        public string Thyroid { get; set; }
        public string Lung { get; set; }
        public string Heart { get; set; }
        public string Skin { get; set; }
        public string Smoke { get; set; }
        public string Alcohol { get; set; }
        public string DrugAllergy { get; set; }
        public string PersonalHistory { get; set; }
        public string ExamConclusion { get; set; }
        public string EKGInfo { get; set; }
        public string EKGTranslate { get; set; }
        public string EkgConclusion { get; set; }
        public string EKGResult { get; set; }
        public double? FVC { get; set; }
        public double? FVCPred { get; set; }
        public double? FVCPer { get; set; }
        public double? FEV1 { get; set; }
        public double? FEV1Pred { get; set; }
        public double? FEV1Per { get; set; }
        public double? FEV1FVC { get; set; }
        public double? FEV1FVCPred { get; set; }
        public double? FEV1FVCPer { get; set; }
        public string SpiroRecommend { get; set; }
        public string SpiroResult { get; set; }
        public string FarPoint { get; set; }
        public string NearPoint { get; set; }
        public string Depth { get; set; }
        public string Color { get; set; }
        public string Muscle { get; set; }
        public string Visualfield { get; set; }
        public string VARt { get; set; }
        public string VALt { get; set; }
        public string Disease { get; set; }
        public string TitmusConclusion { get; set; }
        public string TitmusRecommend { get; set; }
        public string TitmusResult { get; set; }
        public string AudioRightResult { get; set; }
        public string AudioLeftResult { get; set; }
        public string L500Hz { get; set; }
        public string R500Hz { get; set; }
        public string L1000Hz { get; set; }
        public string R1000Hz { get; set; }
        public string L2000Hz { get; set; }
        public string R2000Hz { get; set; }
        public string L3000Hz { get; set; }
        public string R3000Hz { get; set; }
        public string L4000Hz { get; set; }
        public string R4000Hz { get; set; }
        public string L6000Hz { get; set; }
        public string R6000Hz { get; set; }
        public string L8000Hz { get; set; }
        public string R8000Hz { get; set; }
        public string AudioRecommend { get; set; }
        public string AudioResult { get; set; }
        public string MyopiaRight { get; set; }
        public string MyopiaLeft { get; set; }
        public string AstigmaticRight { get; set; }
        public string AstigmaticLeft { get; set; }
        public string ViewRight { get; set; }
        public string ViewLeft { get; set; }
        public string HyperopiaRight { get; set; }
        public string HyperopiaLeft { get; set; }
        public string VARight { get; set; }
        public string VALeft { get; set; }
        public string EyeDiseas { get; set; }
        public string BlindColor { get; set; }
        public string ViewResult { get; set; }
        public string ViewRecommend { get; set; }
    }
}
