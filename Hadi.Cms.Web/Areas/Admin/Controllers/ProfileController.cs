using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Infrastructure.Utilities;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Model.QueryModels;
using Hadi.Cms.Notification.Sms;
using Hadi.Cms.Web.Controllers;
using Hadi.Cms.Web.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly UserService _userService;
        private readonly UserProfileService _userProfileService;
        private readonly CityService _cityService;
        private readonly EducationService _educationService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly ConfirmKeyService _confirmKeyService;
        private readonly EventLoger _eventLoger;


        public ProfileController()
        {
            _userService = new UserService();
            _userProfileService = new UserProfileService();
            _cityService = new CityService();
            _educationService = new EducationService();
            _attachmentFileService = new AttachmentFileService();
            _confirmKeyService = new ConfirmKeyService();
            _eventLoger = new EventLoger();
        }

        public ActionResult Index(Guid? userId)
        {
            IUserProfileDto model = new UserProfileDto();

            var editableUser = _userService.Get(u => u.Id == userId);
            ViewBag.FullName = editableUser.FullName;
            ViewBag.UserId = userId;
            model.FirstName = editableUser.FirstName;
            model.LastName = editableUser.LastName;

            if (SessionData.Current.CurrentUserIsAdministrator || SessionData.Current.User.Id == userId)
            {
                ViewBag.CanView = true;
            }
            else
            {
                ViewBag.CanView = false;
            }

            var userProfile = _userProfileService.Get(up => up.UserId == userId);
            if (userProfile != null)
            {
                var city = _cityService.Get(c => c.Id == userProfile.CityId);
                ViewBag.CityName = city != null ? city.Name : "";
                ViewBag.ProvinceName = city != null && city.ProvinceDto != null ? city.ProvinceDto.Name : "";
                var education = _educationService.Get(c => c.Id == userProfile.EducationId);
                ViewBag.EducationName = education != null ? education.Title : "";

                model = new UserProfileDto()
                {
                    EmailAddress = userProfile.EmailAddress,
                    MobileNumber = userProfile.MobileNumber,
                    PhoneNumber = userProfile.PhoneNumber,
                    NationalCode = userProfile.NationalCode,
                    EducationId = userProfile.EducationId,
                    CityId = userProfile.CityId,
                    BirthDate = userProfile.BirthDate,
                    Gender = userProfile.Gender,
                    MobileNumberIsValid = userProfile.MobileNumberIsValid,
                    UserId = userId,
                    ImageAttachmentGuid = userProfile.ImageAttachmentGuid,
                    ImageBinary = userProfile.ImageBinary,
                    Id = userProfile.Id
                };
            }
            else
            {
                ViewBag.CityName = "";
                ViewBag.ProvinceName = "";
            }

            return View(model);
        }

        public ActionResult Edit(Guid? userId)
        {
            var editableUser = _userService.Get(u => u.Id == userId).MaptoEntity();
            ViewBag.FullName = editableUser.FullName;
            ViewBag.Educations = _educationService.GetList();
            ViewBag.Cities = _cityService.GetList().OrderBy(o => o.Name).ToList();

            var model = new ProfileModel();
            model.FirstName = editableUser.FirstName;
            model.LastName = editableUser.LastName;
            var userProfile = _userProfileService.Get(up => up.UserId == userId).MaptoEntity();
            if (userProfile != null)
            {
                ViewBag.CurrentEducationId = userProfile.EducationId;
                ViewBag.CurrentCityId = userProfile.CityId;

                var imgBinary = new byte[0];
                if (userProfile.ImageAttachmentGuid != null)
                {
                    imgBinary = _attachmentFileService.GeAttachmentContent(userProfile.ImageAttachmentGuid);
                }

                model.EmailAddress = userProfile.EmailAddress;
                model.MobileNumber = userProfile.MobileNumber;
                model.PhoneNumber = userProfile.PhoneNumber;
                model.NationalCode = userProfile.NationalCode;
                model.EducationId = userProfile.EducationId;
                model.CityId = userProfile.CityId;
                model.ImageBinary = imgBinary;
                model.Gender = userProfile.Gender.Value;
                model.MobileNumberIsValid = userProfile.MobileNumberIsValid;
                model.UserId = userId;
                model.ImageAttachmentGuid = userProfile.ImageAttachmentGuid;
                model.Id = userProfile.Id;

                // change date format to gregorian
                var gregorianBirthDate = new DateTime();

                var pCalendar = new System.Globalization.PersianCalendar();
                if (userProfile.BirthDate != null)
                {
                    gregorianBirthDate = pCalendar.ToDateTime(userProfile.BirthDate.Value.Year, userProfile.BirthDate.Value.Month,
                   userProfile.BirthDate.Value.Day, userProfile.BirthDate.Value.Hour, userProfile.BirthDate.Value.Minute, userProfile.BirthDate.Value.Second, 0);
                }

                model.BirthDate = gregorianBirthDate;
                SessionData.RefreshSessionDataFor(userProfile.User);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProfileModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (model.UserId != null)
                {
                    var editableUser = _userService.Get(u => u.Id == model.UserId).MaptoEntity();
                    var attachmentId = new Guid();
                    var attachmentFileId = Guid.Empty;
                    var editAttachmentFile = model.ImageAttachmentGuid;


                    if (file != null)
                    {
                        if (file.ContentLength > 0)
                        {
                            byte[] imageData = null;
                            var fileSize = file.ContentLength;
                            using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                            {
                                imageData = binaryReader.ReadBytes(file.ContentLength);
                            }

                            var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                            int imageWidth = img.Width;
                            int imageHeight = img.Height;

                            attachmentId = Guid.NewGuid();

                            // برای کاربر؛ عکس حتما باینری ذخیره می شود
                            attachmentFileId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, attachmentId, System.IO.Path.GetFileName(file.FileName),
                            System.IO.Path.GetExtension(file.FileName), fileSize, MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(file.FileName)),
                            imageWidth, imageHeight, "ProfileImage - " + model.Id, "", imageData, DateTime.Now);

                            model.ImageAttachmentGuid = attachmentFileId;
                        }
                    }

                    var userProfile = _userProfileService.Get(u => u.UserId == model.UserId).MaptoEntity();
                    if (userProfile == null)
                    {
                        var newUserProfile = new UserProfile()
                        {
                            EmailAddress = model.EmailAddress,
                            MobileNumber = model.MobileNumber,
                            PhoneNumber = model.PhoneNumber,
                            NationalCode = model.NationalCode,
                            EducationId = model.EducationId,
                            CityId = model.CityId,
                            BirthDate = DateTime.Parse(model.BirthDate.ToGregorian()),
                            Gender = model.Gender,
                            UserId = model.UserId.Value,
                            MobileNumberIsValid = false,
                            ImageAttachmentGuid = model.ImageAttachmentGuid
                        };

                        _userProfileService.Insert(newUserProfile);

                        _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "ProfileController", "Create", "Success Create UserProfile", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                    }
                    else
                    {
                        userProfile.EmailAddress = model.EmailAddress;
                        userProfile.MobileNumber = model.MobileNumber;
                        userProfile.PhoneNumber = model.PhoneNumber;
                        userProfile.NationalCode = model.NationalCode;
                        userProfile.EducationId = model.EducationId;
                        userProfile.CityId = model.CityId;
                        userProfile.ImageAttachmentGuid = model.ImageAttachmentGuid;
                        userProfile.BirthDate = DateTime.Parse(model.BirthDate.ToGregorian());
                        userProfile.Gender = model.Gender;
                        userProfile.MobileNumberIsValid = (userProfile.MobileNumber != model.MobileNumber) ? false : userProfile.MobileNumberIsValid;

                        _userProfileService.Update(userProfile);
                    }
                    _userProfileService.Save();

                    //update user fullname
                    editableUser.FirstName = model.FirstName;
                    editableUser.LastName = model.LastName;
                    _userService.Update(editableUser);
                    _userService.Save();

                    if (attachmentId != null && attachmentId != Guid.Empty) //فایل ضمیمه جدید ذخیره شده
                    {
                        try
                        {
                            if (editAttachmentFile != null && editAttachmentFile != Guid.Empty)
                            {
                                //فایل ضمیمه قبلی پاک میشود
                                _attachmentFileService.RemoveAttachment(editAttachmentFile.Value);
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
                return RedirectToAction("Index", new { userId = model.UserId });
            }
            ViewBag.Educations = _educationService.GetList();
            ViewBag.Cities = _cityService.GetList().OrderBy(o => o.Name).ToList();
            return View(model);
        }

        public ActionResult VerificationMobileNumber(string mobileNumber)
        {
            ViewBag.UserMobileNumber = mobileNumber;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerificationMobileNumberProccess(string mobileNumber, string smsKey)
        {
            try
            {
                if (!string.IsNullOrEmpty(smsKey))
                {
                    var item = _confirmKeyService.GetList().OrderByDescending(o => o.Id).FirstOrDefault(c => c.IsSms && c.SmsKey == smsKey && c.ExpireDate > DateTime.Now);
                    if (item != null)
                    {
                        if (item.UserMobileNumber == mobileNumber)
                        {
                            var profile = _userProfileService.Get(p => p.UserId == item.UserId).MaptoEntity();
                            profile.MobileNumberIsValid = true;
                            _userProfileService.Save();

                            return RedirectToAction("Index", "Profile", new { userId = SessionData.Current.User.Id });
                        }
                    }
                }
                ViewBag.Success = "0";
                ViewBag.Message = "کد وارد شده فاقد اعتبار است";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Success = "0";
                ViewBag.Message = "خطا در بررسی کد اعتبار سنجی";

                _eventLoger.LogEvent(EventType.Error, SessionData.Current.User.Id, SessionData.Current.User.UserName, "ProfileController", "SendRequestToMobileNumber", ex.Message.ToString(), HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

                return View();
            }
        }

        public ActionResult SendRequestToMobileNumber(string mobileNumber)
        {
            var result = "nok";

            if (!string.IsNullOrEmpty(mobileNumber))
            {
                try
                {
                    var newKey = new ConfirmKey()
                    {
                        UserId = SessionData.Current.User.Id,
                        IsEmail = false,
                        IsSms = true,
                        CreateDate = DateTime.Now,
                        ExpireDate = DateTime.Now.AddHours(12),
                        LinkGuid = null,
                        SmsKey = Utility.GenerateUniqueOrderId().ToString().Substring(0, 6),
                        UserEmailAddress = null,
                        UserMobileNumber = mobileNumber
                    };
                    _confirmKeyService.Insert(newKey);
                    _confirmKeyService.Save();

                    // send sms to user
                    string message = @"کد اعتبارسنجی : " + Environment.NewLine + newKey.SmsKey;
                    var sms = new SmsIr();
                    sms.SendMessage(mobileNumber, message);

                    result = "ok";

                    _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "ProfileController", "SendRequestToMobileNumber", "success send message to this mobile number : " + mobileNumber, HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                }
                catch (Exception ex)
                {
                    _eventLoger.LogEvent(EventType.Error, SessionData.Current.User.Id, SessionData.Current.User.UserName, "ProfileController", "SendRequestToMobileNumber", ex.Message.ToString(), HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}