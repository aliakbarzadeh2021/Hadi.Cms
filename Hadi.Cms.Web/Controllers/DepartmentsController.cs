using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Hadi.Cms.ApplicationService.Services;

namespace Hadi.Cms.Web.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly DepartmentService _departmentService;
        private readonly DepartmentServiceService _departmentServiceService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly MethodologyService _methodologyService;
        private readonly DepartmentSelectionReasonService _departmentSelectionReasonService;
        private readonly EmployeeService _employeeService;

        public DepartmentsController()
        {
            _departmentService = new DepartmentService();
            _departmentServiceService = new DepartmentServiceService();
            _attachmentFileService = new AttachmentFileService();
            _methodologyService = new MethodologyService();
            _departmentSelectionReasonService = new DepartmentSelectionReasonService();
            _employeeService = new EmployeeService();
        }
        public ActionResult Details(Guid id)
        {
            var department = _departmentService.Get(d => d.Id == id && d.IsActive && !d.IsDeleted);
            if (department == null)
                return HttpNotFound("Department not found .");

            department.Source = _attachmentFileService.GetAttachmentSourceValue(department.AttachmentImageId);
            department.DepartmentBackgroundHeaderAttachmentImageSource =
                _attachmentFileService.GetAttachmentSourceValue(department.DepartmentBackgroundHeaderAttachmentImageId);
            department.JoinOurTeamImageAttachmentSource =
                _attachmentFileService.GetAttachmentSourceValue(department.JoinOurTeamImageAttachmentId);
            department.StructureAttachmentImageSource =
                _attachmentFileService.GetAttachmentSourceValue(department.StructureAttachmentImageId);


            TempData["DepartmentColor"] = department.Color;
            TempData.Keep("DepartmentColor");

            return View(department);
        }

        public ActionResult DepartmentServicePartial(Guid id)
        {
            var departmentServices = _departmentServiceService.GetList
                (d => d.DepartmentId == id && d.Service.IsActive && !d.IsDeleted, null, d => d.Service);

            var services = departmentServices.Select(d => d.ServiceDto).ToList();


            foreach (var service in services)
            {
                service.ServiceLogoSource = _attachmentFileService.GetAttachmentSourceValue(service.ServiceLogoId);
            }

            ViewBag.DepartmentColor = TempData["DepartmentColor"].ToString();

            return PartialView("_DepartmentServicePartial", services.OrderByDescending(s => s.CreatedDate).ToList());
        }

        public ActionResult DepartmentSelectionReasonPartial(Guid id)
        {
            var departmentSelectionReasons = _departmentSelectionReasonService.GetList(d => d.DepartmentId == id && d.IsActive && !d.IsDeleted);
            foreach (var departmentSelectionReason in departmentSelectionReasons)
            {
                if (departmentSelectionReason.AttachmentImageId.HasValue)
                    departmentSelectionReason.ImageSource =
                        _attachmentFileService.GetAttachmentSourceValue(departmentSelectionReason.AttachmentImageId);
            }

            ViewBag.DepartmentColor = TempData["DepartmentColor"].ToString();

            return PartialView("_DepartmentSelectionReasonPartial", departmentSelectionReasons.OrderByDescending(d => d.CreatedDate).ToList());
        }

        public ActionResult DepartmentMethodologyPartial(Guid id)
        {
            var departmentMethodologies = _methodologyService.GetList(
                m => m.DepartmentId == id && m.IsActive && !m.IsDeleted, o => o.OrderByDescending(m => m.CreatedDate)).Take(2).ToList();

            foreach (var departmentMethodology in departmentMethodologies)
            {
                if (departmentMethodology?.ImageAttachmentId != null)
                    departmentMethodology.ImageSource =
                        _attachmentFileService.GetAttachmentSourceValue(departmentMethodology.ImageAttachmentId);
            }

            return PartialView("_DepartmentMethodologyPartial", departmentMethodologies);
        }

        public ActionResult DepartmentEmployee(Guid id , string structureImageSource)
        {
            var employees = _employeeService.GetList(e => e.DepartmentId == id && e.IsActive && !e.IsDeleted);
            foreach (var employee in employees)
            {
                employee.ImageSource = _attachmentFileService.GetAttachmentSourceValue(employee.AttachmentImageId);
                employee.FullName = $"{employee.FirstName} {employee.LastName}";
            }

            ViewBag.StructureImageSource = structureImageSource;

            return PartialView("_DepartmentEmployeePartial", employees.OrderBy(e => e.OrderId).ToList());
        }
    }
}