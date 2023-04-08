using Hadi.Cms.Model;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Repository.BaseRepository;
using System;
using System.Security.AccessControl;

namespace Hadi.Cms.Repository.UnitOfWork
{
    public class DataContext : IDisposable
    {
        private DatabaseContext _context;

        public DataContext()
        {
            _context = new DatabaseContext();

            /// Disable Lazy Loading
            //_context.Configuration.ProxyCreationEnabled = false;
        }

        private BaseRepository<Author> _authorRepository;
        public BaseRepository<Author> AuthorRepository => _authorRepository ?? new BaseRepository<Author>(_context);

        private BaseRepository<AttachmentFile> _attachmentFileRepository;
        public BaseRepository<AttachmentFile> AttachmentFileRepository
        {
            get
            {
                if (_attachmentFileRepository == null)
                    _attachmentFileRepository = new BaseRepository<AttachmentFile>(_context);

                return _attachmentFileRepository;
            }
        }

        private BaseRepository<AttachmentFileTag> _attachmentFileTag;

        public BaseRepository<AttachmentFileTag> AttachmentFileTagRepository =>
            _attachmentFileTag ?? new BaseRepository<AttachmentFileTag>(_context);

        private BaseRepository<City> _cityRepository;
        public BaseRepository<City> CityRepository
        {
            get
            {
                if (_cityRepository == null)
                    _cityRepository = new BaseRepository<City>(_context);

                return _cityRepository;
            }
        }

        private BaseRepository<Challenge> _challengeRepository;

        public BaseRepository<Challenge> ChallengeRepository =>
            _challengeRepository ?? new BaseRepository<Challenge>(_context);

        private BaseRepository<ChallengeProject> _challengeProjectRepository;

        public BaseRepository<ChallengeProject> ChallengeProjectRepository =>
            _challengeProjectRepository ?? new BaseRepository<ChallengeProject>(_context);

        private BaseRepository<ConfirmKey> _confirmKeyRepository;
        public BaseRepository<ConfirmKey> ConfirmKeyRepository
        {
            get
            {
                if (_confirmKeyRepository == null)
                    _confirmKeyRepository = new BaseRepository<ConfirmKey>(_context);

                return _confirmKeyRepository;
            }
        }

        private BaseRepository<ContactUs> _contactUsRepository;
        public BaseRepository<ContactUs> ContactUsRepository
        {
            get
            {
                if (_contactUsRepository == null)
                    _contactUsRepository = new BaseRepository<ContactUs>(_context);

                return _contactUsRepository;
            }
        }

        private BaseRepository<Course> _courseRepository;
        public BaseRepository<Course> CourseRepository => _courseRepository ?? new BaseRepository<Course>(_context);

        private BaseRepository<CourseTag> _courseTagRepository;

        public BaseRepository<CourseTag> CourseTagRepository =>
            _courseTagRepository ?? new BaseRepository<CourseTag>(_context);

        private BaseRepository<CourseAttachmentFile> _courseAttachmentFileRepository;

        public BaseRepository<CourseAttachmentFile> CourseAttachmentFileRepository =>
            _courseAttachmentFileRepository ?? new BaseRepository<CourseAttachmentFile>(_context);

        private BaseRepository<CourseTeacher> _courseTeacherRepository;

        public BaseRepository<CourseTeacher> CourseTeacherRepository =>
            _courseTeacherRepository ?? new BaseRepository<CourseTeacher>(_context);

        private BaseRepository<CareerOpportunity> _careerOpportunityRepository;

        public BaseRepository<CareerOpportunity> CareerOpportunityRepository =>
            _careerOpportunityRepository ?? new BaseRepository<CareerOpportunity>(_context);

        private BaseRepository<Department> _departmentRepository;

        public BaseRepository<Department> DepartmentRepository =>
            _departmentRepository ?? new BaseRepository<Department>(_context);

        private BaseRepository<DepartmentService> _departmentServiceRepository;

        public BaseRepository<DepartmentService> DepartmentServiceRepository =>
            _departmentServiceRepository ?? new BaseRepository<DepartmentService>(_context);

        private BaseRepository<DepartmentSelectionReason> _departmentSelectionReason;

        public BaseRepository<DepartmentSelectionReason> DepartmentSelectionReasonRepository =>
            _departmentSelectionReason ?? new BaseRepository<DepartmentSelectionReason>(_context);

        private BaseRepository<Education> _educationRepository;
        public BaseRepository<Education> EducationRepository
        {
            get
            {
                if (_educationRepository == null)
                    _educationRepository = new BaseRepository<Education>(_context);

                return _educationRepository;
            }
        }

        private BaseRepository<Employer> _employerRepository;
        public BaseRepository<Employer> EmployerRepository =>
            _employerRepository ?? new BaseRepository<Employer>(_context);

        private BaseRepository<Employee> _employeeRepository;
        public BaseRepository<Employee> EmployeeRepository => _employeeRepository ?? new BaseRepository<Employee>(_context);

        private BaseRepository<EventLog> _eventLogRepository;
        public BaseRepository<EventLog> EventLogRepository
        {
            get
            {
                if (_eventLogRepository == null)
                    _eventLogRepository = new BaseRepository<EventLog>(_context);

                return _eventLogRepository;
            }
        }

        private BaseRepositoryWithSqlCommand<EventLog> _eventLogRepositoryWithSqlCommand;
        public BaseRepositoryWithSqlCommand<EventLog> EventLogRepositoryWithSqlCommand
        {
            get
            {
                if (_eventLogRepositoryWithSqlCommand == null)
                    _eventLogRepositoryWithSqlCommand = new BaseRepositoryWithSqlCommand<EventLog>(_context);

                return _eventLogRepositoryWithSqlCommand;
            }
        }

        private BaseRepository<Event> _eventRepository;
        public BaseRepository<Event> EventRepository => _eventRepository ?? new BaseRepository<Event>(_context);


        private BaseRepository<Feature> _featureRepository;
        public BaseRepository<Feature> FeatureRepository
        {
            get
            {
                if (_featureRepository == null)
                    _featureRepository = new BaseRepository<Feature>(_context);

                return _featureRepository;
            }
        }

        private BaseRepository<FooterCategory> _footerCategoryRepository;

        public BaseRepository<FooterCategory> FooterCategory =>
            _footerCategoryRepository ?? new BaseRepository<FooterCategory>(_context);

        private BaseRepository<FooterCategoryLink> _footerCatwgoryLinkRepository;

        public BaseRepository<FooterCategoryLink> FooterCategoryLinkRepository =>
            _footerCatwgoryLinkRepository ?? new BaseRepository<FooterCategoryLink>(_context);

        private BaseRepository<Form> _formRepository;
        public BaseRepository<Form> FormRepository => _formRepository ?? new BaseRepository<Form>(_context);

        private BaseRepository<FormFieldValue> _formFieldValue;

        public BaseRepository<FormFieldValue> FormFieldValueRepository =>
            _formFieldValue ?? new BaseRepository<FormFieldValue>(_context);

        private BaseRepository<FormField> _formFieldRepsitory;

        public BaseRepository<FormField> FormFieldRepository =>
            _formFieldRepsitory ?? new BaseRepository<FormField>(_context);


        private BaseRepository<IpBanned> _ipBannedRepository;
        public BaseRepository<IpBanned> IpBannedRepository
        {
            get
            {
                if (_ipBannedRepository == null)
                    _ipBannedRepository = new BaseRepository<IpBanned>(_context);

                return _ipBannedRepository;
            }
        }

        private BaseRepository<IpRange> _ipRangeRepository;
        public BaseRepository<IpRange> IpRangeRepository
        {
            get
            {
                if (_ipRangeRepository == null)
                    _ipRangeRepository = new BaseRepository<IpRange>(_context);

                return _ipRangeRepository;
            }
        }

        private BaseRepository<Partner> _partnerRepository;
        public BaseRepository<Partner> PartnerRepository => _partnerRepository ?? new BaseRepository<Partner>(_context);

        private BaseRepository<Language> _languageRepository;
        public BaseRepository<Language> LanguageRepository
        {
            get
            {
                if (_languageRepository == null)
                    _languageRepository = new BaseRepository<Language>(_context);

                return _languageRepository;
            }
        }

        private BaseRepository<LoginHistory> _loginHistoryRepository;
        public BaseRepository<LoginHistory> LoginHistoryRepository
        {
            get
            {
                if (_loginHistoryRepository == null)
                    _loginHistoryRepository = new BaseRepository<LoginHistory>(_context);

                return _loginHistoryRepository;
            }
        }

        private BaseRepository<Mail> _mailRepository;
        public BaseRepository<Mail> MailRepository
        {
            get
            {
                if (_mailRepository == null)
                    _mailRepository = new BaseRepository<Mail>(_context);

                return _mailRepository;
            }
        }

        private BaseRepository<MailAttachment> _mailAttachmentRepository;
        public BaseRepository<MailAttachment> MailAttachmentRepository
        {
            get
            {
                if (_mailAttachmentRepository == null)
                    _mailAttachmentRepository = new BaseRepository<MailAttachment>(_context);

                return _mailAttachmentRepository;
            }
        }


        private BaseRepository<MailUser> _mailUserRepository;
        public BaseRepository<MailUser> MailUserRepository
        {
            get
            {
                if (_mailUserRepository == null)
                    _mailUserRepository = new BaseRepository<MailUser>(_context);

                return _mailUserRepository;
            }
        }

        private BaseRepository<Methodology> _methodologyRepository;

        public BaseRepository<Methodology> MethodologyRepository =>
            _methodologyRepository ?? new BaseRepository<Methodology>(_context);

        private BaseRepository<News> _newsRepository;
        public BaseRepository<News> NewsRepository
        {
            get
            {
                if (_newsRepository == null)
                    _newsRepository = new BaseRepository<News>(_context);

                return _newsRepository;
            }
        }

        private BaseRepository<NewsCategory> _newsCategoryRepository;
        public BaseRepository<NewsCategory> NewsCategoryRepository =>
            _newsCategoryRepository ?? new BaseRepository<NewsCategory>(_context);

        private BaseRepository<NewsNewsCategory> _newsNewsCategoryRepository;
        public BaseRepository<NewsNewsCategory> NewsNewsCategoryRepository =>
            _newsNewsCategoryRepository ?? new BaseRepository<NewsNewsCategory>(_context);

        private BaseRepository<NlEmail> _nlEmailRepository;

        public BaseRepository<NlEmail> NlEmailRepository =>
            _nlEmailRepository ?? new BaseRepository<NlEmail>(_context);

        private BaseRepository<NlMessage> _nlMessageRepository;

        public BaseRepository<NlMessage> NlMessageRepository =>
            _nlMessageRepository ?? new BaseRepository<NlMessage>(_context);

        private BaseRepository<NlMessageEmail> _nlMessageEmailRepository;

        public BaseRepository<NlMessageEmail> NlMessageEmailRepository =>
            _nlMessageEmailRepository ?? new BaseRepository<NlMessageEmail>(_context);

        private BaseRepository<Province> _provinceRepository;
        public BaseRepository<Province> ProvinceRepository
        {
            get
            {
                if (_provinceRepository == null)
                    _provinceRepository = new BaseRepository<Province>(_context);

                return _provinceRepository;
            }
        }

        private BaseRepository<Project> _projectRepository;
        public BaseRepository<Project> ProjectRepository => _projectRepository ?? new BaseRepository<Project>(_context);

        private BaseRepository<ProjectTag> _projectTagRepository;

        public BaseRepository<ProjectTag> ProjectTagRepository =>
            _projectTagRepository ?? new BaseRepository<ProjectTag>(_context);

        private BaseRepository<ProjectTechnology> _projectTechnology;

        public BaseRepository<ProjectTechnology> ProjectTechnologyRepository =>
            _projectTechnology ?? new BaseRepository<ProjectTechnology>(_context);

        private BaseRepository<ProjectAttachmentFile> _projectAttachmentFile;

        public BaseRepository<ProjectAttachmentFile> ProjectAttachmentFileRepository =>
            _projectAttachmentFile ?? new BaseRepository<ProjectAttachmentFile>(_context);

        private BaseRepository<Role> _roleRepository;
        public BaseRepository<Role> RoleRepository
        {
            get
            {
                if (_roleRepository == null)
                    _roleRepository = new BaseRepository<Role>(_context);

                return _roleRepository;
            }
        }

        private BaseRepository<Resume> _resumeRepository;
        public BaseRepository<Resume> ResumeRepository => _resumeRepository = new BaseRepository<Resume>(_context);

        private BaseRepository<RoleFeature> _roleFeatureRepository;
        public BaseRepository<RoleFeature> RoleFeatureRepository
        {
            get
            {
                if (_roleFeatureRepository == null)
                    _roleFeatureRepository = new BaseRepository<RoleFeature>(_context);

                return _roleFeatureRepository;
            }
        }


        private BaseRepository<Setting> _settingRepository;
        public BaseRepository<Setting> SettingRepository
        {
            get
            {
                if (_settingRepository == null)
                    _settingRepository = new BaseRepository<Setting>(_context);

                return _settingRepository;
            }
        }


        private BaseRepository<User> _userRepository;
        public BaseRepository<User> UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new BaseRepository<User>(_context);

                return _userRepository;
            }
        }


        private BaseRepository<UserContact> _userContactRepository;
        public BaseRepository<UserContact> UserContactRepository
        {
            get
            {
                if (_userContactRepository == null)
                    _userContactRepository = new BaseRepository<UserContact>(_context);

                return _userContactRepository;
            }
        }


        private BaseRepository<UserProfile> _userProfileRepository;
        public BaseRepository<UserProfile> UserProfileRepository
        {
            get
            {
                if (_userProfileRepository == null)
                    _userProfileRepository = new BaseRepository<UserProfile>(_context);

                return _userProfileRepository;
            }
        }


        private BaseRepository<UserRole> _userRoleRepository;
        public BaseRepository<UserRole> UserRoleRepository
        {
            get
            {
                if (_userRoleRepository == null)
                    _userRoleRepository = new BaseRepository<UserRole>(_context);

                return _userRoleRepository;
            }
        }

        private BaseRepository<Notification> _notificationRepository;
        public BaseRepository<Notification> NotificationRepository
        {
            get
            {
                if (_notificationRepository == null)
                    _notificationRepository = new BaseRepository<Notification>(_context);

                return _notificationRepository;
            }
        }

        private BaseRepository<Page> _pageRepository;
        public BaseRepository<Page> PageRepository
        {
            get
            {
                if (_pageRepository == null)
                    _pageRepository = new BaseRepository<Page>(_context);

                return _pageRepository;
            }
        }

        private BaseRepository<PageStatistic> _pageStatisticRepository;
        public BaseRepository<PageStatistic> PageStatisticRepository
        {
            get
            {
                if (_pageStatisticRepository == null)
                    _pageStatisticRepository = new BaseRepository<PageStatistic>(_context);

                return _pageStatisticRepository;
            }
        }

        private BaseRepository<Menu> _menuRepository;
        public BaseRepository<Menu> MenuRepository
        {
            get
            {
                if (_menuRepository == null)
                    _menuRepository = new BaseRepository<Menu>(_context);

                return _menuRepository;
            }
        }

        private BaseRepository<Article> _articleRepository;
        public BaseRepository<Article> ArticleRepository
        {
            get
            {
                if (_articleRepository == null)
                    _articleRepository = new BaseRepository<Article>(_context);

                return _articleRepository;
            }
        }


        private BaseRepository<Tag> _tagRepository;
        public BaseRepository<Tag> TagRepository
        {
            get
            {
                if (_tagRepository == null)
                    _tagRepository = new BaseRepository<Tag>(_context);

                return _tagRepository;
            }
        }

        private BaseRepository<ArticleTag> _articleTagRepository;
        public BaseRepository<ArticleTag> ArticleTagRepository
        {
            get
            {
                if (_articleTagRepository == null)
                    _articleTagRepository = new BaseRepository<ArticleTag>(_context);

                return _articleTagRepository;
            }
        }

        private BaseRepository<Template> _templateRepository;
        public BaseRepository<Template> TemplateRepository
        {
            get
            {
                if (_templateRepository == null)
                    _templateRepository = new BaseRepository<Template>(_context);

                return _templateRepository;
            }
        }

        private BaseRepository<TemplateDetail> _templateDetailRepository;
        public BaseRepository<TemplateDetail> TemplateDetailRepository
        {
            get
            {
                if (_templateDetailRepository == null)
                    _templateDetailRepository = new BaseRepository<TemplateDetail>(_context);

                return _templateDetailRepository;
            }
        }

        private BaseRepository<Technology> _technologyRepository;

        public BaseRepository<Technology> TechnologyRepository =>
            _technologyRepository ?? new BaseRepository<Technology>(_context);

        private BaseRepository<Teacher> _teacherRepository;
        public BaseRepository<Teacher> TeacherRepository => _teacherRepository ?? new BaseRepository<Teacher>(_context);

        private BaseRepository<SocialNetwork> _socialNetworkRepository;
        public BaseRepository<SocialNetwork> SocialNetworkRepository => _socialNetworkRepository ?? new BaseRepository<SocialNetwork>(_context);


        private BaseRepository<Service> _serviceRepository;
        public BaseRepository<Service> ServiceRepository => _serviceRepository ?? new BaseRepository<Service>(_context);

        private BaseRepository<ServiceComment> _serviceCommentRepository;

        private BaseRepository<Slider> _sliderRepository;
        public BaseRepository<Slider> SliderRepository => _sliderRepository ?? new BaseRepository<Slider>(_context);

        private BaseRepository<SliderItem> _sliderItemRepository;

        public BaseRepository<SliderItem> SliderItemRepository =>
            _sliderItemRepository ?? new BaseRepository<SliderItem>(_context);

        public BaseRepository<ServiceComment> ServiceCommentRepository =>
            _serviceCommentRepository ?? new BaseRepository<ServiceComment>(_context);

        private BaseRepository<ServiceReceiver> _serviceReceiverRepository;

        public BaseRepository<ServiceReceiver> ServiceReceiverRepository =>
            _serviceReceiverRepository ?? new BaseRepository<ServiceReceiver>(_context);

        private BaseRepository<ServiceReceiverService> _serviceReceiverServiceRepository;

        public BaseRepository<ServiceReceiverService> ServiceReceiverServiceRepository =>
            _serviceReceiverServiceRepository ?? new BaseRepository<ServiceReceiverService>(_context);

        private BaseRepository<ServiceTag> _serviceTagRepository;

        public BaseRepository<ServiceTag> ServiceTagRepository =>
            _serviceTagRepository ?? new BaseRepository<ServiceTag>(_context);

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Reload(object obj)
        {
            _context.Entry(obj).Reload();
        }
    }
}
