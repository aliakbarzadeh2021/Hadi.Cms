using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;

namespace Hadi.Cms.ApplicationService.Services
{
    public class ServiceReceiverServiceService
    {
        private readonly DataContext _dataContext;

        public ServiceReceiverServiceService()
        {
            _dataContext = new DataContext();
        }

        ~ServiceReceiverServiceService()
        {
            _dataContext?.Dispose();
        }


        public List<IServiceReceiverServiceDto> GetList(
            Expression<Func<Model.Entities.ServiceReceiverService, bool>> filter = null,
            Func<IQueryable<Model.Entities.ServiceReceiverService>,
                IOrderedQueryable<Model.Entities.ServiceReceiverService>> orderBy = null,
            params Expression<Func<Model.Entities.ServiceReceiverService, object>>[] includes)
        {
            return _dataContext.ServiceReceiverServiceRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// نسبت دادن سرویس به سرویس گیرنده
        /// </summary>
        /// <param name="serviceReceiverId"></param>
        /// <param name="servicesId"></param>
        /// <param name="userId"></param>
        public void AssignServiceToServiceReceiver(Guid serviceReceiverId, List<Guid> servicesId, Guid userId)
        {
            var serviceReceiverServices = GetList(s => s.ServiceReceiverId == serviceReceiverId);
            foreach (var serviceReceiverService in serviceReceiverServices)
            {
                Delete(serviceReceiverService.Id);
            }

            if (servicesId != null && servicesId.Count > 0)
            {
                foreach (var serviceId in servicesId)
                {
                    var newServiceReceiverService = new Hadi.Cms.Model.Entities.ServiceReceiverService()
                    {
                        ServiceId = serviceId,
                        ServiceReceiverId = serviceReceiverId,
                        CreatedBy = userId
                    };
                    Insert(newServiceReceiverService);
                }
            }
            Save();
        }

        public void Insert(Model.Entities.ServiceReceiverService entity)
        {
            _dataContext.ServiceReceiverServiceRepository.Insert(entity);
        }

        public void Delete(Guid serviceReceiverServiceId)
        {
            _dataContext.ServiceReceiverServiceRepository.Delete(serviceReceiverServiceId);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
