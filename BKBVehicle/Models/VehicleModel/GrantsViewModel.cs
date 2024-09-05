using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BKBVehicle.Models.GrantsModel
{
    public class GrantsViewModel
    {
    }
    public partial class GrantsDeathViewModel
    {

        public int id { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string PFEN { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string Designation { get; set; }
        //[Required(ErrorMessage = "This Field is Required")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string NID { get; set; }
        //[Required(ErrorMessage = "This Field is Required")]
        //[StringLength(15, MinimumLength = 13, ErrorMessage = "13 Digit Account Number")]
        public string AccNum { get; set; }
        //[Required(ErrorMessage = "This Field is Required")]
        //[StringLength(15, MinimumLength = 13, ErrorMessage = "13 Digit Account Number")]
        public string NomineeAcc { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string FileNum { get; set; }
        
        public Nullable<System.DateTime> CreatedOn { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> AppDate { get; set; }
        //public string AppDate { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string LastWorkingPlace { get; set; }
       
        public string Status { get; set; }
        //[Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> PayDate { get; set; }
        //public string PayDate { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> DateOfDeath { get; set; }
        //public string DateOfDeath { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> DateOfJoining { get; set; }
        //public string DateOfJoining { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> DateOfRetirement { get; set; }
       // public string DateOfRetirement { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<decimal> PaymentAmount { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string SendFromProperMedium { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string ApplicationAttested { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string DeathCertificate { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string InterestBreakup { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string LiabilityCertificate { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string LiabilityDeductionLetter { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string LastSalaryCertificate { get; set; }
        //[Required(ErrorMessage = "This Field is Required")]
        public string NomineeMainCopy { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string SuccessionMainCopy { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string LastOfficeOrder { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string SecondMarriageCertificate { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string HalfYearlyDeathCertificate { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string SuccessionLetter { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string SuccessionNID { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string AuditReport { get; set; }
      
        public string AuditReportDetails { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string ComplianceReport { get; set; }
        
        public string ComplianceReportDetails { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string VigilanceReport { get; set; }
       
        public string VigilanceReportDetails { get; set; }
        public string Comment { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string Remarks { get; set; }
        public string FiscalYear { get; set; }
        public string UserID { get; set; }
        public string VerifyID { get; set; }
        public string ComplianceID { get; set; }
        public string VigilanceID { get; set; }
        public string AuditID { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string TypeofGrants { get; set; }
        public Nullable<int> Flag { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string MissingDocuments { get; set; }
        public object DeathGrantsEdit { get; internal set; }
        public string ReportType { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string LastThreeYearsWorkingDeatails { get; set; }

    }
    public partial class RestDonationViewModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string PFEN { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string MobileNumber { get; set; }
        //[Required(ErrorMessage = "This Field is Required")]
        public string NID { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        [StringLength(15, MinimumLength = 13, ErrorMessage = "13 Digit Account Number")]
        public string AccNum { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string FileNum { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string LastWorkingPlace { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> AppDate { get; set; }
        //public string AppDate { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<decimal> ApplicationAmount { get; set; }
        //[Required(ErrorMessage = "This Field is Required")]
        public Nullable<decimal> ApprovedAmount { get; set; }
        public Nullable<decimal> TotalApprovedAmount { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string Reason { get; set; }
        public string NameofCause { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string HasCertificate { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string RelevantDocuments { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string HasAnnouncementLetters { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string SendFromProperMedium { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string HasHospitalBillMainCopy { get; set; }
        public string FiscalYear { get; set; }
        public string Comment { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string Remarks { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string LastThreeYearsWorkingDeatails { get; set; }
        public Nullable<int> Flag { get; set; }
        public string CheckStatus { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string TypeofGrants { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string OthersReason { get; set; }

    }
    public partial class DesignationViewModel
    {
        public int id { get; set; }
        public string Value { get; set; }
        public string Label { get; set; }
        public string Field { get; set; }
        public Nullable<int> Flag { get; set; }
    }
    public partial class EmployeeInfoViewModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string EmpNameBN { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string EmpNameEN { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string PFBN { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string PFEN { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> DOB { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<System.DateTime> PRLDate { get; set; }
        public string NID { get; set; }
        public string CWorkPlace { get; set; }
        public string Gender { get; set; }
        public string BankAcc { get; set; }
  
        public string MobileNum { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> Flag { get; set; }
        public string DesignationBN { get; set; }
    }
   
}