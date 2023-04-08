using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hadi.Cms.ApplicationService.Services
{
    public class MailService
    {
        private DataContext _dataContext;

        public MailService()
        {
            _dataContext = new DataContext();
        }

        public IMailDto Get(Expression<Func<Mail, bool>> filter)
        {
            var mail = _dataContext.MailRepository.Get(filter);
            return mail.MapToDto();
        }

        public IMailDto GetById(object Id)
        {
            var mail = _dataContext.MailRepository.GetByID(Id);
            return mail.MapToDto();
        }

        public List<IMailDto> GetList(Expression<Func<Mail, bool>> filter = null)
        {
            var mails = _dataContext.MailRepository.GetList(filter);
            return mails.MapToListDto();
        }

        public void Insert(Mail model)
        {
            _dataContext.MailRepository.Insert(model);
        }

        public void Update(Mail model)
        {
            _dataContext.MailRepository.Update(model);
        }

        public void Delete(Mail model)
        {
            _dataContext.MailRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.MailRepository.Delete(Id);
        }


        public void DeleteRange(IEnumerable<Mail> entities)
        {
            _dataContext.MailRepository.DeleteRange(entities);
        }

        public IQueryable<IMailDto> Where(Expression<Func<Mail, bool>> filter)
        {
            var mails = _dataContext.MailRepository.Where(filter);
            var mailsDto = mails.Select(q => q.MapToDto());
            return mailsDto;
        }

        public bool Any(Expression<Func<Mail, bool>> filter)
        {
            return _dataContext.MailRepository.Any(filter);
        }

        public int Count(Expression<Func<Mail, bool>> where = null)
        {
            return _dataContext.MailRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }



        public int GetAllInboxCount(Guid userId)
        {
            var mailUsers = _dataContext.MailUserRepository.GetList(m => m.ReceiverUserId == userId && m.DeletedByReceiver == false);
            return mailUsers.Count();
        }

        public int GetUnreadInboxCount(Guid userId)
        {
            var mailUsers = _dataContext.MailUserRepository.GetList(m => m.ReceiverUserId == userId && m.Unread && m.DeletedByReceiver == false);
            return mailUsers.Count();
        }

        public bool SendMail(Guid SenderUserId, List<Guid> ReceiverUserId, string Title, string Text, List<Guid> attachmentIds = null)
        {
            try
            {
                Guid mailId = Guid.Empty;
                var newMail = new Mail()
                {
                    Text = Text,
                    Title = Title
                };

                Insert(newMail);

                mailId = newMail.Id;

                if (attachmentIds != null)
                {
                    foreach (var id in attachmentIds)
                    {
                        var newMailAttachment = new MailAttachment()
                        {
                            AttachmentId = id,
                            MailId = mailId
                        };
                        _dataContext.MailAttachmentRepository.Insert(newMailAttachment);
                    }
                }

                foreach (var item in ReceiverUserId)
                {
                    var newMailUser = new MailUser()
                    {
                        DeletedByReceiver = false,
                        DeletedBySender = false,
                        Unread = true,
                        MailId = mailId,
                        ReceiverUserId = item,
                        SenderUserId = SenderUserId,
                        SendDateTime = DateTime.Now
                    };

                    _dataContext.MailUserRepository.Insert(newMailUser);
                }

                Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<IMailDto> GetOutbox(Guid senderUserId)
        {
            var result = new List<IMailDto>();
            var messages = _dataContext.MailUserRepository.GetList(q => q.SenderUserId == senderUserId && q.DeletedBySender == false).OrderBy(q => q.SendDateTime).ToList();

            var groupMailIds = _dataContext.MailUserRepository.GetList(q => q.SenderUserId == senderUserId && q.DeletedBySender == false).GroupBy(q => q.MailId).Select(q => q.Key).ToList();

            List<dynamic> messageList = new List<dynamic>();

            if (messages != null)
            {
                foreach (var mailId in groupMailIds)
                {
                    var groupMessage = messages.Where(x => x.MailId == mailId);

                    var newItem = new MailDto();
                    newItem.Id = groupMessage.FirstOrDefault().Id;
                    //newItem.Receivers = new List<User>();
                    foreach (var item in groupMessage)
                    {
                        var ReceiverUserID = Guid.Parse(item.ReceiverUserId.ToString());
                        var user = _dataContext.UserRepository.Get(u => u.Id == ReceiverUserID);
                        //newItem.Receivers.Add(user);
                    }
                    newItem.SendTime = groupMessage.FirstOrDefault().SendDateTime;
                    var mail = _dataContext.MailRepository.Get(m => m.Id == groupMessage.FirstOrDefault().MailId);
                    newItem.Text = mail.Text;
                    newItem.Title = mail.Title;
                    newItem.Unread = false;
                    newItem.WithAttachment = _dataContext.MailAttachmentRepository.Any(a => a.MailId == groupMessage.FirstOrDefault().MailId);

                    result.Add(newItem);
                }
            }
            return result.OrderByDescending(o => o.SendTime).ToList();
        }

        public List<IMailDto> GetInbox(Guid receiverUserId)
        {
            var result = new List<IMailDto>();

            var mails = _dataContext.MailUserRepository.GetList(m => m.ReceiverUserId == receiverUserId && m.DeletedByReceiver == false).OrderByDescending(o => o.SendDateTime);
            foreach (var item in mails)
            {
                var newItem = new MailDto();
                newItem.Id = item.Id;
                //newItem.Sender = _dataContext.UserRepository.Get(u => u.Id == item.SenderUserId);
                newItem.SendTime = item.SendDateTime;
                var mail = _dataContext.MailRepository.Get(m => m.Id == item.Id);
                newItem.Text = mail.Text;
                newItem.Title = mail.Title;
                newItem.Unread = item.Unread;
                newItem.WithAttachment = _dataContext.MailAttachmentRepository.Any(a => a.MailId == item.MailId);

                result.Add(newItem);
            }

            return result;
        }

        public List<MailDto> GetInboxTopN(Guid receiverUserId, int N)
        {
            var result = new List<MailDto>();

            var mails = _dataContext.MailUserRepository.GetList(m => m.ReceiverUserId == receiverUserId && m.DeletedByReceiver == false).OrderByDescending(o => o.SendDateTime).Take(N).ToList();
            foreach (var item in mails)
            {
                var newItem = new MailDto();
                newItem.Id = item.Id;
                //newItem.Sender = _dataContext.UserRepository.Get(u => u.Id == item.SenderUserId);
                newItem.SendTime = item.SendDateTime;
                var mail = _dataContext.MailRepository.Get(m => m.Id == item.Id);
                newItem.Text = mail.Text;
                newItem.Title = mail.Title;
                newItem.Unread = item.Unread;
                newItem.WithAttachment = _dataContext.MailAttachmentRepository.Any(a => a.MailId == item.MailId);

                result.Add(newItem);
            }

            return result;
        }

        public MailDto GetMessageDetail(Guid messageId, bool isReceiver)
        {
            var mail = _dataContext.MailUserRepository.Get(m => m.Id == messageId);

            //set as read
            if (mail.Unread && isReceiver)
            {
                mail.Unread = false;
                _dataContext.Save();
            }

            var result = new MailDto();
            result.Id = mail.Id;
            //result.Sender = _dataContext.UserRepository.Get(u => u.Id == mail.SenderUserId);

            if (!isReceiver) //ارسال کننده باید لیست گیرنندگان را دریافت کند
            {
                var ReceiverUserIDs = _dataContext.MailUserRepository.GetList(x => x.MailId == mail.MailId).Select(x => x.ReceiverUserId);
                result.Receivers = new List<IUserDto>();
                foreach (var item in ReceiverUserIDs)
                {
                    var user = _dataContext.UserRepository.Get(u => u.Id == item);
                    //result.Receivers.Add(user);
                }
            }

            result.SendTime = mail.SendDateTime;
            var mailContent = _dataContext.MailRepository.Get(m => m.Id == mail.Id);
            result.Text = mailContent.Text;//Html.Raw
            result.Title = mailContent.Title;
            result.Unread = mail.Unread;
            result.WithAttachment = _dataContext.MailAttachmentRepository.Any(a => a.MailId == mail.MailId);
            if (result.WithAttachment)
            {
                result.Attachments = new List<IAttachmentFileDto>();

                var attachmentIds = _dataContext.MailAttachmentRepository.GetList(a => a.MailId == mail.MailId).Select(x => x.AttachmentId);
                foreach (var id in attachmentIds)
                {
                    var attachment = _dataContext.AttachmentFileRepository.Get(af => af.Id == id);
                    if (attachment != null)
                    {
                        //result.Attachments.Add(attachment);
                    }
                }
            }
            return result;
        }


        public static MailDto MapToDto(Mail model)
        {
            var mailDto = new MailDto
            {
                Id = model.Id,
                Text = model.Text,
                Title = model.Title
            };
            return mailDto;
        }
    }
}
