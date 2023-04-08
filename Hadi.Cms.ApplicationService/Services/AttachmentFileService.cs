using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Configuration;
using Hadi.Cms.Infrastructure.Utilities;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Hosting;

namespace Hadi.Cms.ApplicationService.Services
{
    public class AttachmentFileService
    {
        private DataContext _dataContext;

        public AttachmentFileService()
        {
            _dataContext = new DataContext();
        }

        public IAttachmentFileDto Get(Expression<Func<AttachmentFile, bool>> filter = null, params Expression<Func<AttachmentFile, object>>[] includes)
        {
            var attachmentFile = _dataContext.AttachmentFileRepository.Get(filter, includes);
            return attachmentFile.MapToDto();
        }

        public IAttachmentFileDto GetById(object Id)
        {
            var attachmentFile = _dataContext.AttachmentFileRepository.GetByID(Id);
            return attachmentFile.MapToDto();
        }

        public List<IAttachmentFileDto> GetList(Expression<Func<AttachmentFile, bool>> filter = null, Func<IQueryable<AttachmentFile>, IOrderedQueryable<AttachmentFile>> orderBy = null, params Expression<Func<AttachmentFile, object>>[] includes)
        {
            var attachmentFiles = _dataContext.AttachmentFileRepository.GetList(filter, orderBy, includes);
            return attachmentFiles.MapToListDto();
        }

        public void Insert(AttachmentFile model)
        {
            _dataContext.AttachmentFileRepository.Insert(model);
        }

        public void Update(AttachmentFile model)
        {
            _dataContext.AttachmentFileRepository.Update(model);
        }

        public void Delete(AttachmentFile model)
        {
            _dataContext.AttachmentFileRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.AttachmentFileRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<AttachmentFile> attachmentFiles)
        {
            _dataContext.AttachmentFileRepository.DeleteRange(attachmentFiles);
        }


        public IQueryable<IAttachmentFileDto> Where(Expression<Func<AttachmentFile, bool>> filter)
        {
            var attachmentFiles = _dataContext.AttachmentFileRepository.Where(filter);
            var attachmentFilesDto = attachmentFiles.Select(q => q.MapToDto());
            return attachmentFilesDto;
        }

        public bool Any(Expression<Func<AttachmentFile, bool>> filter)
        {
            return _dataContext.AttachmentFileRepository.Any(filter);
        }

        public int Count(Expression<Func<AttachmentFile, bool>> where = null)
        {
            return _dataContext.AttachmentFileRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }



        private object s_lockObject = new object();

        public Guid AddAttachment(Guid userId, Guid attachmentGUID, string attachmentName, string attachmentExtension,
            int attachmentSize, string attachmentMimeType, int? attachmentImageWidth, int? attachmentImageHeight,
            string attachmentTitle, string attachmentDescription, byte[] content, DateTime lastModified, Guid? posterImageGuid = null)
        {
            lock (s_lockObject)
            {
                string filePath = "";
                if (Settings.Application_PhysicalStorage)
                {
                    filePath = GetVirtualPath(BlobStorage.AddFileToPhysicalStorage(attachmentGUID, content, attachmentExtension, true));
                }

                AttachmentFile newAttachment = new AttachmentFile()
                {
                    CreatedBy = userId,
                    LastModified = lastModified,
                    Description = attachmentDescription,
                    Extension = attachmentExtension,
                    ImageHeight = attachmentImageHeight,
                    ImageWidth = attachmentImageWidth,
                    IsBinaryStorage = Settings.Application_BinaryStorage,
                    IsPhysicalStorage = Settings.Application_PhysicalStorage,
                    MimeType = attachmentMimeType,
                    Name = attachmentName,
                    PhysicalPath = filePath,
                    Size = attachmentSize,
                    Title = attachmentTitle,
                    PosterImageGuid = posterImageGuid
                };

                if (Settings.Application_PhysicalStorage)
                {
                    newAttachment.Id = attachmentGUID;
                }

                if (Settings.Application_BinaryStorage)
                {
                    newAttachment.Binary = content;
                }

                Insert(newAttachment);
                Save();
                return newAttachment.Id;
            }
        }

        public void AddAttachment(Guid userId, Guid attachmentGUID, string attachmentName, string attachmentExtension,
            int attachmentSize, string attachmentMimeType, bool isPhysicalStorage, bool isBinaryStorage, int? attachmentImageWidth,
            int? attachmentImageHeight, string attachmentTitle, string attachmentDescription, byte[] content, DateTime lastModified)
        {
            lock (s_lockObject)
            {
                string filePath = "";
                if (isPhysicalStorage)
                {
                    filePath = GetVirtualPath(BlobStorage.AddFileToPhysicalStorage(attachmentGUID, content, attachmentExtension, true));
                }

                AttachmentFile newAttachment = new AttachmentFile()
                {
                    CreatedBy = userId,
                    LastModified = lastModified,
                    Description = attachmentDescription,
                    Extension = attachmentExtension,
                    ImageHeight = attachmentImageHeight,
                    ImageWidth = attachmentImageWidth,
                    IsBinaryStorage = Settings.Application_BinaryStorage,
                    IsPhysicalStorage = Settings.Application_PhysicalStorage,
                    MimeType = attachmentMimeType,
                    Name = attachmentName,
                    PhysicalPath = filePath,
                    Size = attachmentSize,
                    Title = attachmentTitle
                };

                if (isBinaryStorage)
                {
                    newAttachment.Binary = content;
                }

                Insert(newAttachment);
                Save();
            }
        }

        public void RemoveAttachment(Guid attachmentId)
        {
            lock (s_lockObject)
            {

                var item = _dataContext.AttachmentFileRepository.GetByID(attachmentId);
                if (item != null)
                {
                    // delete physical storaged file
                    if (item.IsPhysicalStorage)
                    {
                        string filePath = Path.Combine(Settings.PhysicalStoragePath, item.Id.ToString() + item.Extension);

                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }

                    }
                    DeleteById(item.Id);
                    Save();
                }
            }
        }

        public void RemoveUserAttachments(Guid userId)
        {
            lock (s_lockObject)
            {
                var items = _dataContext.AttachmentFileRepository.GetList(q => q.CreatedBy == userId).ToList();
                DeleteRange(items);
                Save();

                // delete physical storaged file
                foreach (var item in items)
                {
                    if (item.IsPhysicalStorage)
                    {
                        string filePath = Path.Combine(Settings.PhysicalStoragePath, item.Id + item.Extension);

                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                }
            }
        }

        public int UserAttachmentsCount(Guid userId)
        {
            lock (s_lockObject)
            {
                int count = GetList(q => q.CreatedBy == userId).Count;
                return count;
            }
        }

        public bool Exists(Guid attachmentId)
        {
            lock (s_lockObject)
            {
                return Any(q => q.Id == attachmentId);
            }
        }

        public byte[] GetBinaryAttachmentContent(Guid attachmentId)
        {
            lock (s_lockObject)
            {
                var item = Get(q => q.Id == attachmentId);
                return item.Binary;
            }
        }

        public byte[] GeAttachmentContent(Guid? attachmentId)
        {
            lock (s_lockObject)
            {
                var item = Get(q => q.Id == attachmentId);
                if (item != null)
                {
                    if (item.IsBinaryStorage)
                        return item.Binary;
                    else if (item.IsPhysicalStorage)
                        return File.ReadAllBytes(HostingEnvironment.MapPath(item.PhysicalPath));
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        public bool UserPictureExists(Guid id)
        {
            lock (s_lockObject)
            {
                bool res = false;

                var userProfile = _dataContext.UserProfileRepository.Get(u => u.UserId == id && u.ImageAttachmentGuid != null && u.ImageAttachmentGuid != Guid.Empty);
                if (userProfile != null)
                    res = Any(a => a.Id == userProfile.ImageAttachmentGuid);
                return res;
            }
        }

        public byte[] GetUserPictureContent(Guid id)
        {
            lock (s_lockObject)
            {
                byte[] byteArray = null;
                var userProfile = _dataContext.UserProfileRepository.Get(u => u.UserId == id && u.ImageAttachmentGuid != null && u.ImageAttachmentGuid != Guid.Empty);
                if (userProfile != null)
                    byteArray = Get(a => a.Id == userProfile.ImageAttachmentGuid).Binary;

                return byteArray;
            }
        }

        //public string GetAttachmentPhysicalAddress(AttachmentFile attachment)
        //{
        //    lock (s_lockObject)
        //    {
        //        var address = "";

        //        string filePath = Path.Combine(Settings.PhysicalStoragePath, attachment.Id + attachment.Extension);

        //        if (File.Exists(filePath))
        //            address = GetVirtualPath(filePath);
        //        else
        //            address = GetVirtualPath(BlobStorage.AddFileToPhysicalStorage(attachment.Id, attachment.Binary, attachment.Extension, true));

        //        return address;
        //    }
        //}

        public static string GetVirtualPath(string physicalPath)
        {
            if (!physicalPath.StartsWith(HttpContext.Current.Request.PhysicalApplicationPath))
            {
                throw new InvalidOperationException("Physical path is not within the application root");
            }

            return "/" + physicalPath.Substring(HttpContext.Current.Request.PhysicalApplicationPath.Length)
                  .Replace("\\", "/");
        }

        public string GetAttachmentSourceValue(Guid? attachmentGuid, Guid? attachmentId = null, bool shouldBinary = false, string physicalApplicationPath = "")
        {
            lock (s_lockObject)
            {
                var source = "";
                var attachment = new AttachmentFile();

                if (attachmentGuid != null)
                    attachment = _dataContext.AttachmentFileRepository.Get(a => a.Id == attachmentGuid);
                else if (attachmentId != null)
                    attachment = _dataContext.AttachmentFileRepository.Get(a => a.Id == attachmentId);

                if (attachment != null)
                {
                    if (shouldBinary)
                    {
                        if (attachment.IsBinaryStorage)
                        {
                            var base64 = Convert.ToBase64String(attachment.Binary);
                            source = String.Format("data:" + attachment.MimeType + ";base64,{0}", base64);
                        }
                    }
                    else
                    {
                        if (attachment.IsPhysicalStorage)
                        {
                            var path = !string.IsNullOrEmpty(physicalApplicationPath)
                                ? physicalApplicationPath +
                                  attachment.PhysicalPath.Remove(0, 1).Replace("/", "\\")
                                : HttpContext.Current.Request.PhysicalApplicationPath +
                                  attachment.PhysicalPath.Replace("/", "\\");
                            if (File.Exists(path))
                            {
                                if (!attachment.PhysicalPath.StartsWith(!string.IsNullOrEmpty(physicalApplicationPath) ? physicalApplicationPath : HttpContext.Current.Request.PhysicalApplicationPath))
                                {
                                    source = attachment.PhysicalPath;

                                    if (!string.IsNullOrEmpty(physicalApplicationPath))
                                        source = source.Remove(0, 1).Replace("/", "\\");
                                }
                                else
                                {
                                    source = "/" + attachment.PhysicalPath.Substring(!string.IsNullOrEmpty(physicalApplicationPath) ? physicalApplicationPath.Length : HttpContext.Current.Request.PhysicalApplicationPath.Length).Replace("\\", "/");
                                }
                            }
                        }
                        else if (attachment.IsBinaryStorage)
                        {
                            source = "/Attachments/GetAttachment?Id=" + attachment.Id;
                        }
                    }
                }

                return source;
            }
        }


        public static AttachmentFileDto MapToDto(AttachmentFile model)
        {
            var attachmentFileDto = new AttachmentFileDto
            {
                Binary = model.Binary,
                Description = model.Description,
                Extension = model.Extension,
                Id = model.Id,
                ImageHeight = model.ImageHeight,
                ImageWidth = model.ImageWidth,
                IsBinaryStorage = model.IsBinaryStorage,
                IsPhysicalStorage = model.IsPhysicalStorage,
                LastModified = model.LastModified,
                MimeType = model.MimeType,
                Name = model.Name,
                PhysicalPath = model.PhysicalPath,
                Size = model.Size,
                Title = model.Title
            };
            return attachmentFileDto;
        }

    }
}
