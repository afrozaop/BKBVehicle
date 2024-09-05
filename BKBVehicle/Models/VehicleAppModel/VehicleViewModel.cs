using BKBVehicle.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BKBVehicle.Models.VehicleAppModel
{
    public class VehicleViewModel
    {
    }
    public partial class CarInfoViewModel
    {
        public IEnumerable<SelectListItem> ColorList { get; set; }
        public IEnumerable<SelectListItem> VEHICLEList { get; set; }
        public int id { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string VehicleType { get; set; }
        [Required(ErrorMessage = "This Field is Required")]

        //[RegularExpression(@"[A-Za-z]+-Metro-[A-Za-z]+-[0-9]{2}+-[0-9]{4}", ErrorMessage = "Please input xxxxxx-Metro-xxx-xxxx in this specific pattern")]
        [RegularExpression(@"^[A-Za-z]+-Metro-[A-Za-z]+-\d{2}-\d{4}$", ErrorMessage = "Please input xxxxxx-Metro-xxx-dd-dddd in this specific pattern")]

        [Display(Name = "Registration Number")]
        public string RegistrationNo { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public string BrandName { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string Manufacturer { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Enter a valid 4 digit Year")]
        [Display(Name = "Year")]

        public Nullable<int> ProductionYear { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string Color { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string ChesisNo { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string EngineNo { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<decimal> PurchaseValue { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }
        public Nullable<int> Flag { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string OfficeName { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> PurchaseDate { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string ApprovedStatus { get; set; }
        public string BrCode { get; set; }

    }
    public partial class Car_DetailsViewModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "This Field is Required")]

        [RegularExpression(@"^[A-Za-z]+-Metro-[A-Za-z]+-\d{2}-\d{4}$", ErrorMessage = "Please input xxxxxx-Metro-xxx-dd-dddd in this specific pattern")]
        [Display(Name = "Registration Number")]
        public string RegistrationNo { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> TaxToken { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> Fitness { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> RoutePermit { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> Insurance { get; set; }

        public Nullable<decimal> Depriciation { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<decimal> BookValue { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }
        public Nullable<int> Flag { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> BookValueDate { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string ApprovedStatus { get; set; }
        public string BrCode { get; set; }
    }

    public partial class Fuel_DetailsViewModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "This Field is Required")]

        //[RegularExpression(@"^[A-Za-z]+-Metro-[A-Za-z]+-\d{2}-\d{4}$", ErrorMessage = "Please input xxxxxx-Metro-xxx-dd-dddd in this specific pattern")]
        //[Display(Name = "Registration Number")]
        public string RegistrationNo { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> FuelDate { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<decimal> FuelConsumptionCost { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }
        public Nullable<int> Flag { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string BillNo { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string FuelType { get; set; }
        public string ReportType { get; set; }

        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public SelectList RegList { get; internal set; }
        public Nullable<int> FuelComsumptionQ { get; set; }
        public string UsedBy { get; set; }
        public string Driver { get; set; }
        public string UsedByPF { get; set; }
        public string DriverPF { get; set; }
        public string Degination { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> FuelQuantity { get; set; }
        public string BrCode { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string ApprovedStatus { get; set; }

    }
    public partial class RepairViewModel
    {
        public RepairView rv { get; set; }
        public List<RepairDetailsViewModel> rdvm { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public int Repairid { get; set; }
      
        public int id { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string RegistrationNo { get; set; }
       
        public Nullable<System.DateTime> RepairDate { get; set; }

        public string RepairType { get; set; }


        public string Remarks { get; set; }
        public string Comments { get; set; }
        public Nullable<int> Flag { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string BillNo { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        //public List<System.DateTime> CreatedOn { get; set; }
        //public List<string> CreatedBy { get; set; }

        public string ReportType { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string OrganizationName { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public List<string> RepairParts { get; set; }
        [Required(ErrorMessage = "This Field is required.")]
        public string RepairPs { get; set; }
        public string RepairQs { get; set; }
        public Nullable<decimal> RepairCs { get; set; }
      
        public List<decimal?> RepairCost { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
       
        public List<string> Quantity { get; set; }
        public List<RepairDetailsViewModel> RepairDetails { get; set; } = new List<RepairDetailsViewModel>();
        public List<RepairDetailsViewModel> RepairMore { get; set; }
       
        public string UsedByPF { get; set; }
        public string UsedBy { get; set; }
   
        public string DriverPF { get; set; }
        public string Driver { get; set; }
        public string Degination { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string ApprovedStatus { get; set; }
        public string BrCode { get; set; }

    }


    public partial class RepairView
    {

        public int Repairid { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string RegistrationNo { get; set; }
        public string RepairType { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string BillNo { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }
        public Nullable<int> Flag { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string OrganizationName { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> RepairDate { get; set; }
        public string UsedBy { get; set; }
        public string Driver { get; set; }
        public string UsedByPF { get; set; }
        public string DriverPF { get; set; }
        public string Degination { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string ApprovedStatus { get; set; }
        public string BrCode { get; set; }


    }




    public partial class RepairDetailsViewModel
    {
        public int id { get; set; }
        public int Repairid { get; set; }
        public string RepairParts { get; set; }
        public Nullable<decimal> RepairCost { get; set; }
        public string Quantity { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public virtual Repair Repair { get; set; }
    }







        public partial class MaintenanceViewModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string RegistrationNo { get; set; }
        public string MaintenanceType { get; set; }
        
        public string BillNo { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> MaintenanceDate { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string MaintenanceParts { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<decimal> MaintenanceCost { get; set; }

        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }
        public Nullable<int> Flag { get; set; }

        public string UsedBy { get; set; }
        public string Driver { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<int> UnitQ { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string UsedByPF { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string DriverPF { get; set; }
        public string Designation { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public string BrCode { get; set; }
    }
    public partial class DepreciationViewModel
    {
        public string RegistrationNo { get; set; }
        public string DateTo { get; set; }
        public double cBookValue { get; set; }
        public double cDepreciation { get; set; }
        public int cDay { get; set; }
    }
    public partial class ReportViewModel
    {
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
    }
    public partial class FuelMaintenanceViewModel
    {
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public int id { get; set; }

        public string RegistrationNo { get; set; }
        public string MaintenanceType { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string BillNo { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<System.DateTime> MaintenanceDate { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public string MaintenanceParts { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<decimal> MaintenanceCost { get; set; }

        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }
        public Nullable<int> Flag { get; set; }

        public string UsedBy { get; set; }
        public string Driver { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<int> UnitQ { get; set; }
        public string UsedByPF { get; set; }
        public string DriverPF { get; set; }
        public string Designation { get; set; }
    
        public Nullable<System.DateTime> FuelDate { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<decimal> FuelConsumptionCost { get; set; }
    
        public string FuelType { get; set; }
        public string ReportType { get; set; }

     
        public SelectList RegList { get; internal set; }
        public Nullable<int> FuelComsumptionQ { get; set; }
      
        public string Degination { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> FuelQuantity { get; set; }
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
        public Nullable<System.DateTime> DOB { get; set; }
        public string CWorkPlace { get; set; }
        public string Designation { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<System.DateTime> PRLDate { get; set; }
        public string Gender { get; set; }
        public string BankAcc { get; set; }
        public string NID { get; set; }
        public string MobileNum { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> Flag { get; set; }
        public Nullable<decimal> TotalTK { get; set; }
        public string DesignationBN { get; set; }
    }

    public partial class AddMoreViewModel
    {
        public List<RepairDetailsViewModel> RepairMore { get; set; }

        public RepairView RepairMain { get; set; }
    }
}