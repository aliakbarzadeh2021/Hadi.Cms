using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Reflection;

namespace Hadi.Cms.Model.Mappings.Configuration
{
    public static class MapperConfig
    {
        public static void Config()
        {
            AutoMapper.Mapper.Initialize(i =>
                {
                    i.AddProfiles(Assembly.GetExecutingAssembly());
                    //i.CreateMap<User, IRoleDto>()
                    //    .ForMember(dest => dest.RoleIds, opt => opt.MapFrom(src => src.UserRoles.Select(q => q.RoleId)));

                    i.CreateMap<Author, IAuthorDto>().ForMember(dest => dest.ArticleDto , opt => opt.MapFrom(src => src.Articles));
                    i.CreateMap<IAuthorDto, Author>().ForMember(dest => dest.Articles, opt => opt.MapFrom(src => src.ArticleDto));


                    i.CreateMap<AttachmentFile, IAttachmentFileDto>()
                        .ForMember(dest => dest.ArticlesDto, opt => opt.MapFrom(src => src.Articles))
                        .ForMember(dest => dest.MailAttachmentsDto, opt => opt.MapFrom(src => src.MailAttachments))
                        .ForMember(dest => dest.AttachmentFileTagDto, opt => opt.MapFrom(src => src.AttachmentFileTags))
                        .ForMember(dest => dest.ProjectAttachmentFilesDto,
                            opt => opt.MapFrom(src => src.ProjectAttachmentFiles))
                        .ForMember(dest => dest.CourseAttachmentFileDto,
                            opt => opt.MapFrom(src => src.CourseAttachmentFiles));

                    i.CreateMap<IAttachmentFileDto, AttachmentFile>()
                        .ForMember(dest => dest.Articles, opt => opt.MapFrom(src => src.ArticlesDto))
                        .ForMember(dest => dest.MailAttachments, opt => opt.MapFrom(src => src.MailAttachmentsDto))
                        .ForMember(dest => dest.AttachmentFileTags, opt => opt.MapFrom(src => src.AttachmentFileTagDto))
                        .ForMember(dest => dest.ProjectAttachmentFiles,
                            opt => opt.MapFrom(src => src.ProjectAttachmentFilesDto))
                        .ForMember(dest => dest.CourseAttachmentFiles,
                            opt => opt.MapFrom(src => src.CourseAttachmentFileDto));


                    i.CreateMap<AttachmentFileTag, IAttachmentFileTagDto>()
                        .ForMember(dest => dest.AttachmentFileDto, opt => opt.MapFrom(src => src.AttachmentFile))
                        .ForMember(dest => dest.TagDto, opt => opt.MapFrom(src => src.Tag));

                    i.CreateMap<IAttachmentFileTagDto, AttachmentFileTag>()
                        .ForMember(dest => dest.AttachmentFile, opt => opt.MapFrom(src => src.AttachmentFileDto))
                        .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src.TagDto));

                    i.CreateMap<Employer, IEmployerDto>().ForMember(dest => dest.ProjectDto , opt => opt.MapFrom(src => src.Projects));

                    i.CreateMap<IEmployerDto, Employer>().ForMember(dest => dest.Projects, opt => opt.MapFrom(src => src.ProjectDto));

                    i.CreateMap<Employee, IEmployeeDto>().ForMember(dest => dest.DepartmentDto,
                        opt => opt.MapFrom(src => src.Department));
                    i.CreateMap<IEmployeeDto, Employee>().ForMember(dest => dest.Department,
                        opt => opt.MapFrom(src => src.DepartmentDto));

                    i.CreateMap<City, ICityDto>()
                        .ForMember(dest => dest.ProvinceDto, opt => opt.MapFrom(src => src.Province));

                    i.CreateMap<ICityDto, City>()
                        .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.ProvinceDto));

                    i.CreateMap<Course, ICourseDto>().ForMember(dest => dest.CourseTeacherDto,
                            opt => opt.MapFrom(src => src.CourseTeachers))
                        .ForMember(dest => dest.CourseAttachmentFileDto,
                            opt => opt.MapFrom(src => src.CourseAttachmentFiles)).ForMember(dest => dest.CourseTagDto, opt => opt.MapFrom(src => src.CourseTags));

                    i.CreateMap<ICourseDto, Course>().ForMember(dest => dest.CourseTeachers,
                            opt => opt.MapFrom(src => src.CourseTeacherDto))
                        .ForMember(dest => dest.CourseAttachmentFiles,
                            opt => opt.MapFrom(src => src.CourseAttachmentFileDto))
                        .ForMember(dest => dest.CourseTags, opt => opt.MapFrom(src => src.CourseTagDto));

                    i.CreateMap<CourseTeacher, ICourseTeacherDto>()
                        .ForMember(dest => dest.TeacherDto, opt => opt.MapFrom(src => src.Teacher))
                        .ForMember(dest => dest.CourseDto, opt => opt.MapFrom(src => src.Course));

                    i.CreateMap<ICourseTeacherDto, CourseTeacher>()
                        .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.CourseDto))
                        .ForMember(dest => dest.Teacher, opt => opt.MapFrom(src => src.TeacherDto));

                    i.CreateMap<CourseTag, ICourseTagDto>()
                        .ForMember(dest => dest.CourseDto, opt => opt.MapFrom(src => src.Course))
                        .ForMember(dest => dest.TagDto, opt => opt.MapFrom(src => src.Tag));

                    i.CreateMap<ICourseTagDto, CourseTag>()
                        .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.CourseDto))
                        .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src.TagDto));

                    i.CreateMap<Challenge, IChallengeDto>().ForMember(dest => dest.ChallengeProjects,
                        opt => opt.MapFrom(src => src.ChallengeProjects));

                    i.CreateMap<IChallengeDto, Challenge>().ForMember(dest => dest.ChallengeProjects,
                        opt => opt.MapFrom(src => src.ChallengeProjects));

                    i.CreateMap<ChallengeProject, IChallengeProjectDto>().ForMember(dest => dest.Challenge,
                            opt => opt.MapFrom(src => src.Challenge))
                        .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.Project));

                    i.CreateMap<ICourseAttachmentFileDto, CourseAttachmentFile>()
                        .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.CourseDto))
                        .ForMember(dest => dest.AttachmentFile, opt => opt.MapFrom(src => src.AttachmentFileDto));

                    i.CreateMap<CourseAttachmentFile, ICourseAttachmentFileDto>()
                        .ForMember(dest => dest.CourseDto, opt => opt.MapFrom(src => src.Course))
                        .ForMember(dest => dest.AttachmentFileDto, opt => opt.MapFrom(src => src.AttachmentFile));

                    i.CreateMap<Department, IDepartmentDto>().ForMember(dest => dest.DepartmentServiceDto,
                            opt => opt.MapFrom(src => src.DepartmentServices))
                        .ForMember(dest => dest.DepartmentSelectionReasonDto,
                            opt => opt.MapFrom(src => src.DepartmentSelectionReasons))
                        .ForMember(dest => dest.EmployeeDto, opt => opt.MapFrom(src => src.Employees))
                        .ForMember(dest => dest.MethodologyDto, opt => opt.MapFrom(src => src.Methodologies));

                    i.CreateMap<IDepartmentDto, Department>().ForMember(dest => dest.DepartmentServices,
                            opt => opt.MapFrom(src => src.DepartmentServiceDto))
                        .ForMember(dest => dest.DepartmentSelectionReasons,
                            opt => opt.MapFrom(src => src.DepartmentSelectionReasonDto))
                        .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.EmployeeDto))
                        .ForMember(dest => dest.Methodologies, opt => opt.MapFrom(src => src.MethodologyDto));

                    i.CreateMap<DepartmentSelectionReason, IDepartmentSelectionReasonDto>()
                        .ForMember(dest => dest.DepartmentDto, opt => opt.MapFrom(src => src.Department));

                    i.CreateMap<IDepartmentSelectionReasonDto, DepartmentSelectionReason>()
                        .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.DepartmentDto));

                    i.CreateMap<DepartmentService, IDepartmentServiceDto>().ForMember(dest => dest.DepartmentDto,
                            opt => opt.MapFrom(src => src.Department))
                        .ForMember(dest => dest.ServiceDto, opt => opt.MapFrom(src => src.Service));

                    i.CreateMap<IDepartmentServiceDto, DepartmentService>().ForMember(dest => dest.Department,
                            opt => opt.MapFrom(src => src.DepartmentDto))
                        .ForMember(dest => dest.Service, opt => opt.MapFrom(src => src.ServiceDto));

                    i.CreateMap<Event, IEventDto>();
                    i.CreateMap<IEventDto, Event>();

                    i.CreateMap<FormFieldValue, IFormFieldValueDto>().ForMember(dest => dest.FormFieldDto,opt => opt.MapFrom(src => src.FormField));
                    i.CreateMap<IFormFieldValueDto, FormFieldValue>().ForMember(dest => dest.FormField,opt => opt.MapFrom(src => src.FormFieldDto));
                    
                    i.CreateMap<Project, IProjectDto>()
                        .ForMember(dest => dest.ProjectTags, opt => opt.MapFrom(src => src.ProjectTags))
                        .ForMember(dest => dest.ChallengeProject, opt => opt.MapFrom(src => src.ChallengeProjects))
                        .ForMember(dest => dest.ProjectTechnologiesDto,
                            opt => opt.MapFrom(src => src.ProjectTechnologies))
                        .ForMember(dest => dest.ProjectAttachmentFilesDto,
                            opt => opt.MapFrom(src => src.ProjectAttachmentFiles));

                    i.CreateMap<IProjectDto, Project>()
                        .ForMember(dest => dest.ProjectTags, opt => opt.MapFrom(src => src.ProjectTags))
                        .ForMember(dest => dest.ChallengeProjects, opt => opt.MapFrom(src => src.ChallengeProject))
                        .ForMember(dest => dest.ProjectTechnologies,
                            opt => opt.MapFrom(src => src.ProjectTechnologiesDto))
                        .ForMember(dest => dest.ProjectAttachmentFiles,
                            opt => opt.MapFrom(src => src.ProjectAttachmentFilesDto));

                    i.CreateMap<ProjectTag, IProjectTagDto>()
                        .ForMember(dest => dest.TagDto, opt => opt.MapFrom(src => src.Tag))
                        .ForMember(dest => dest.ProjectDto, opt => opt.MapFrom(src => src.Project));

                    i.CreateMap<IProjectTagDto, ProjectTag>()
                        .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src.TagDto))
                        .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.ProjectDto));


                    i.CreateMap<IProjectAttachmentFileDto, ProjectAttachmentFile>().ForMember(dest => dest.Project,
                            opt => opt.MapFrom(src => src.ProjectDto))
                        .ForMember(dest => dest.AttachmentFile, opt => opt.MapFrom(src => src.AttachmentFileDto));

                    i.CreateMap<ProjectAttachmentFile, IProjectAttachmentFileDto>().ForMember(dest => dest.ProjectDto,
                            opt => opt.MapFrom(src => src.Project))
                        .ForMember(dest => dest.AttachmentFileDto, opt => opt.MapFrom(src => src.AttachmentFile));

                    i.CreateMap<ConfirmKey, IConfirmKeyDto>();
                    i.CreateMap<ContactUs, IContactUsDto>();
                    i.CreateMap<Education, IEducationDto>();


                    i.CreateMap<EventLog, IEventLogDto>();

                    i.CreateMap<IEventLogDto, EventLog>();


                    i.CreateMap<Feature, IFeatureDto>()
                        .ForMember(dest => dest.RoleFeaturesDto, opt => opt.MapFrom(src => src.RoleFeatures));

                    i.CreateMap<IFeatureDto, Feature>()
                        .ForMember(dest => dest.RoleFeatures, opt => opt.MapFrom(src => src.RoleFeaturesDto));


                    i.CreateMap<FooterCategory, IFooterCategoryDto>().ForMember(dest => dest.FooterCategoryLinkDto,
                        opt => opt.MapFrom(src => src.FooterCategoryLinks));

                    i.CreateMap<IFooterCategoryDto, FooterCategory>().ForMember(dest => dest.FooterCategoryLinks,
                        opt => opt.MapFrom(src => src.FooterCategoryLinkDto));

                    i.CreateMap<FooterCategoryLink, IFooterCategoryLinkDto>().ForMember(dest => dest.FooterCategoryDto,
                        opt => opt.MapFrom(src => src.FooterCategory));

                    i.CreateMap<IFooterCategoryLinkDto, FooterCategoryLink>().ForMember(dest => dest.FooterCategory,
                        opt => opt.MapFrom(src => src.FooterCategoryDto));

                    i.CreateMap<Form, IFormDto>().ForMember(dest => dest.FormFieldDto,opt => opt.MapFrom(src => src.FormFields));
                    i.CreateMap<IFormDto, Form>().ForMember(dest => dest.FormFields,opt => opt.MapFrom(src => src.FormFieldDto));

                    i.CreateMap<FormField, IFormFieldDto>()
                        .ForMember(dest => dest.FormDto, opt => opt.MapFrom(src => src.Form));

                    i.CreateMap<IFormFieldDto, FormField>()
                        .ForMember(dest => dest.Form, opt => opt.MapFrom(src => src.FormDto));

                    i.CreateMap<IpBanned, IIpBannedDto>();
                    i.CreateMap<IpRange, IIpRangeDto>();

                    i.CreateMap<Partner, IPartnerDto>();
                    i.CreateMap<IPartnerDto, Partner>();

                    i.CreateMap<Entities.Language, ILanguageDto>();


                    i.CreateMap<LoginHistory, ILoginHistoryDto>()
                        .ForMember(dest => dest.UserDto, opt => opt.MapFrom(src => src.User));

                    i.CreateMap<ILoginHistoryDto, LoginHistory>()
                        .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserDto));


                    i.CreateMap<MailAttachment, IMailAttachmentDto>();
                    i.CreateMap<Mail, IMailDto>();
                    i.CreateMap<MailUser, IMailUserDto>();

                    i.CreateMap<Methodology, IMethodologyDto>().ForMember(dest => dest.DepartmentDto,
                        opt => opt.MapFrom(src => src.Department));

                    i.CreateMap<IMethodologyDto, Methodology>().ForMember(dest => dest.Department,
                        opt => opt.MapFrom(src => src.DepartmentDto));

                    i.CreateMap<News, INewsDto>().ForMember(dest => dest.NewsNewsCategories,
                        opt => opt.MapFrom(src => src.NewsNewsCategories));

                    i.CreateMap<INewsDto, News>().ForMember(dest => dest.NewsNewsCategories,
                        opt => opt.MapFrom(src => src.NewsNewsCategories));


                    i.CreateMap<NewsCategory, INewsCategory>().ForMember(dest => dest.NewsNewsCategories,
                        opt => opt.MapFrom(src => src.NewsNewsCategories));

                    i.CreateMap<INewsCategory, NewsCategory>().ForMember(dest => dest.NewsNewsCategories,
                        opt => opt.MapFrom(src => src.NewsNewsCategories));

                    i.CreateMap<NewsNewsCategory, INewsNewsCategory>()
                        .ForMember(dest => dest.NewsCategoryDto, opt => opt.MapFrom(src => src.NewsCategory))
                        .ForMember(dest => dest.NewsDto, opt => opt.MapFrom(src => src.News));

                    i.CreateMap<INewsNewsCategory, NewsNewsCategory>()
                        .ForMember(dest => dest.NewsCategory, opt => opt.MapFrom(src => src.NewsCategoryDto))
                        .ForMember(dest => dest.News, opt => opt.MapFrom(src => src.NewsDto));

                    i.CreateMap<NewsNewsCategory, INewsNewsCategory>()
                        .ForMember(dest => dest.NewsCategoryDto, opt => opt.MapFrom(src => src.NewsCategory))
                        .ForMember(dest => dest.NewsDto, opt => opt.MapFrom(src => src.News));
                    i.CreateMap<Province, IProvinceDto>()
                        .ForMember(dest => dest.CitiesDto, opt => opt.MapFrom(src => src.Cities));

                    i.CreateMap<NlMessage, INlMessageDto>().ForMember(dest => dest.NlMessageEmailDto,
                        opt => opt.MapFrom(src => src.NlMessageEmails));

                    i.CreateMap<NlEmail, INlEmailDto>().ForMember(dest => dest.NlMessageEmailDto,
                        opt => opt.MapFrom(src => src.NlMessageEmails));

                    i.CreateMap<INlEmailDto, NlEmail>().ForMember(dest => dest.NlMessageEmails,
                        opt => opt.MapFrom(src => src.NlMessageEmailDto));

                    i.CreateMap<INlMessageDto, NlMessage>().ForMember(dest => dest.NlMessageEmails,
                        opt => opt.MapFrom(src => src.NlMessageEmailDto));

                    i.CreateMap<INlMessageEmailDto, NlMessageEmail>().ForMember(dest => dest.NlMessage,
                        opt => opt.MapFrom(src => src.NlMessageDto))
                        .ForMember(dest => dest.NlEmail, opt => opt.MapFrom(src => src.NlEmailDto));

                    i.CreateMap<NlMessageEmail, INlMessageEmailDto>().ForMember(dest => dest.NlMessageDto,
                        opt => opt.MapFrom(src => src.NlMessage))
                        .ForMember(dest => dest.NlEmailDto, opt => opt.MapFrom(src => src.NlEmail));

                    i.CreateMap<IProvinceDto, Province>()
                     .ForMember(dest => dest.Cities, opt => opt.MapFrom(src => src.CitiesDto));

                    i.CreateMap<Role, IRoleDto>()
                    .ForMember(dest => dest.RoleFeaturesDto, opt => opt.MapFrom(src => src.RoleFeatures))
                    .ForMember(dest => dest.UserRolesDto, opt => opt.MapFrom(src => src.UserRoles));

                    i.CreateMap<IRoleDto, Role>()
                   .ForMember(dest => dest.RoleFeatures, opt => opt.MapFrom(src => src.RoleFeaturesDto))
                   .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRolesDto));

                    i.CreateMap<Resume, IResumeDto>().ForMember(dest => dest.CareerOpportunityDto,opt => opt.MapFrom(src => src.CareerOpportunity));
                    i.CreateMap<IResumeDto, Resume>().ForMember(dest => dest.CareerOpportunity,opt => opt.MapFrom(src => src.CareerOpportunityDto));

                    i.CreateMap<ICareerOpportunityDto, CareerOpportunity>().ForMember(dest => dest.Resumes,
                        opt => opt.MapFrom(src => src.ResumeDto));

                    i.CreateMap<CareerOpportunity, ICareerOpportunityDto>().ForMember(dest => dest.ResumeDto,
                        opt => opt.MapFrom(src => src.Resumes));

                    i.CreateMap<RoleFeature, IRoleFeatureDto>()
                    .ForMember(dest => dest.RoleDto, opt => opt.MapFrom(src => src.Role))
                    .ForMember(dest => dest.FeatureDto, opt => opt.MapFrom(src => src.Feature));

                    i.CreateMap<IRoleFeatureDto, RoleFeature>()
                   .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RoleDto))
                   .ForMember(dest => dest.Feature, opt => opt.MapFrom(src => src.FeatureDto));


                    i.CreateMap<Setting, ISettingDto>();


                    i.CreateMap<UserContact, IUserContactDto>()
                    .ForMember(dest => dest.ContactUserDto, opt => opt.MapFrom(src => src.ContactUser))
                    .ForMember(dest => dest.SelfUserDto, opt => opt.MapFrom(src => src.SelfUser));

                    i.CreateMap<IUserContactDto, UserContact>()
                    .ForMember(dest => dest.ContactUser, opt => opt.MapFrom(src => src.ContactUserDto))
                    .ForMember(dest => dest.SelfUser, opt => opt.MapFrom(src => src.SelfUserDto));


                    i.CreateMap<User, IUserDto>()
                        .ForMember(dest => dest.UserRolesDto, opt => opt.MapFrom(src => src.UserRoles))
                        .ForMember(dest => dest.UserContactsDto, opt => opt.MapFrom(src => src.UserContacts))
                        .ForMember(dest => dest.UserProfilesDto, opt => opt.MapFrom(src => src.UserProfiles))
                        .ForMember(dest => dest.UserListCollctionDto, opt => opt.MapFrom(src => src.UserListCollction))
                        .ForMember(dest => dest.LoginHistoriesDto, opt => opt.MapFrom(src => src.LoginHistories))
                        .ForMember(dest => dest.LanguageDto, opt => opt.MapFrom(src => src.Language))
                        .ForMember(dest => dest.PagesDto, opt => opt.MapFrom(src => src.Pages));

                    i.CreateMap<IUserDto, User>()
                      .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRolesDto))
                      .ForMember(dest => dest.UserContacts, opt => opt.MapFrom(src => src.UserContactsDto))
                      .ForMember(dest => dest.UserProfiles, opt => opt.MapFrom(src => src.UserProfilesDto))
                      .ForMember(dest => dest.UserListCollction, opt => opt.MapFrom(src => src.UserListCollctionDto))
                      .ForMember(dest => dest.LoginHistories, opt => opt.MapFrom(src => src.LoginHistoriesDto))
                      .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.LanguageDto))
                      .ForMember(dest => dest.Pages, opt => opt.MapFrom(src => src.PagesDto));


                    i.CreateMap<UserProfile, IUserProfileDto>()
                    .ForMember(dest => dest.UserDto, opt => opt.MapFrom(src => src.User));

                    i.CreateMap<IUserProfileDto, UserProfile>()
                    .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserDto));


                    i.CreateMap<UserRole, IUserRoleDto>()
                     .ForMember(dest => dest.UserDto, opt => opt.MapFrom(src => src.User))
                     .ForMember(dest => dest.RoleDto, opt => opt.MapFrom(src => src.Role));

                    i.CreateMap<IUserRoleDto, UserRole>()
                    .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserDto))
                    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RoleDto));


                    i.CreateMap<Page, IPageDto>();

                    i.CreateMap<IPageDto, Page>();


                    i.CreateMap<PageStatistic, IPageStatisticDto>();

                    i.CreateMap<IPageStatisticDto, PageStatistic>();


                    i.CreateMap<Menu, IMenuDto>();
                    i.CreateMap<IMenuDto, Menu>();


                    i.CreateMap<Notification, INotificationDto>()
                    .ForMember(dest => dest.SenderDto, opt => opt.MapFrom(src => src.Sender))
                    .ForMember(dest => dest.ReceiverDto, opt => opt.MapFrom(src => src.Receiver));

                    i.CreateMap<INotificationDto, Notification>()
                    .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.SenderDto))
                    .ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => src.ReceiverDto));


                    i.CreateMap<Article, IArticleDto>()
                   .ForMember(dest => dest.ArticleTagsDto, opt => opt.MapFrom(src => src.ArticleTags))
                   .ForMember(dest => dest.AuthorDto , opt => opt.MapFrom(src => src.Author));

                    i.CreateMap<IArticleDto, Article>()
                    .ForMember(dest => dest.ArticleTags, opt => opt.MapFrom(src => src.ArticleTagsDto))
                    .ForMember(dest => dest.Author , opt => opt.MapFrom(src => src.AuthorDto));


                    i.CreateMap<Tag, ITagDto>()
                   .ForMember(dest => dest.ArticleTagsDto, opt => opt.MapFrom(src => src.ArticleTags))
                   .ForMember(dest => dest.CourseTagDto, opt => opt.MapFrom(src => src.CourseTags))
                   .ForMember(dest => dest.ServiceTagDto, opt => opt.MapFrom(src => src.ServiceTags))
                   .ForMember(dest => dest.AttachmentFileTagDto,opt => opt.MapFrom(src => src.AttachmentFileTags));

                    i.CreateMap<ITagDto, Tag>()
                   .ForMember(dest => dest.ArticleTags, opt => opt.MapFrom(src => src.ArticleTagsDto))
                   .ForMember(dest => dest.CourseTags, opt => opt.MapFrom(src => src.CourseTagDto))
                   .ForMember(dest => dest.ServiceTags, opt => opt.MapFrom(src => src.ServiceTagDto))
                   .ForMember(dest => dest.AttachmentFileTags,opt => opt.MapFrom(src => src.AttachmentFileTagDto));


                    i.CreateMap<ArticleTag, IArticleTagDto>()
                   .ForMember(dest => dest.ArticleDto, opt => opt.MapFrom(src => src.Article))
                   .ForMember(dest => dest.TagDto, opt => opt.MapFrom(src => src.Tag));

                    i.CreateMap<IArticleTagDto, ArticleTag>()
                   .ForMember(dest => dest.Article, opt => opt.MapFrom(src => src.ArticleDto))
                   .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src.TagDto));


                    i.CreateMap<TemplateDetail, ITemplateDetailDto>()
                    .ForMember(dest => dest.TemplateDto, opt => opt.MapFrom(src => src.Template));

                    i.CreateMap<ITemplateDetailDto, TemplateDetail>()
                    .ForMember(dest => dest.Template, opt => opt.MapFrom(src => src.TemplateDto));

                    i.CreateMap<Technology, ITechnologyDto>().ForMember(dest => dest.ProjectTechnologiesDto,
                            opt => opt.MapFrom(src => src.ProjectTechnologies))
                        .ForMember(dest => dest.ProjectTechnologiesDto, opt => opt.MapFrom(src => src.ProjectTechnologies));


                    i.CreateMap<ITechnologyDto, Technology>().ForMember(dest => dest.ProjectTechnologies,
                        opt => opt.MapFrom(src => src.ProjectTechnologiesDto));

                    i.CreateMap<Teacher, ITeacherDto>().ForMember(dest => dest.CourseTeacherDto,
                        opt => opt.MapFrom(src => src.CourseTeachers));

                    i.CreateMap<ITeacherDto, Teacher>().ForMember(dest => dest.CourseTeachers,
                        opt => opt.MapFrom(src => src.CourseTeacherDto));

                    i.CreateMap<ProjectTechnology, IProjectTechnologyDto>()
                        .ForMember(dest => dest.ProjectDto, opt => opt.MapFrom(src => src.Project))
                        .ForMember(dest => dest.TechnologyDto, opt => opt.MapFrom(src => src.Technology));

                    i.CreateMap<IProjectTechnologyDto, ProjectTechnology>()
                        .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.ProjectDto))
                        .ForMember(dest => dest.Technology, opt => opt.MapFrom(src => src.TechnologyDto));

                    i.CreateMap<ISocialNetwork, SocialNetwork>();
                    i.CreateMap<SocialNetwork, ISocialNetwork>();

                    i.CreateMap<Service, IServiceDto>().ForMember(dest => dest.DepartmentServicesDto,
                        opt => opt.MapFrom(src => src.DepartmentServices))
                        .ForMember(dest => dest.ServiceTagDto, opt => opt.MapFrom(src => src.ServiceTags))
                        .ForMember(dest => dest.ServiceReceiverServiceDto, opt => opt.MapFrom(src => src.ServiceReceiverServices))
                        .ForMember(dest => dest.ServiceCommentDto, opt => opt.MapFrom(src => src.ServiceComments));

                    i.CreateMap<IServiceDto, Service>().ForMember(dest => dest.DepartmentServices,
                        opt => opt.MapFrom(src => src.DepartmentServicesDto))
                        .ForMember(dest => dest.ServiceTags, opt => opt.MapFrom(src => src.ServiceTagDto))
                        .ForMember(dest => dest.ServiceReceiverServices, opt => opt.MapFrom(src => src.ServiceReceiverServiceDto))
                        .ForMember(dest => dest.ServiceComments, opt => opt.MapFrom(src => src.ServiceCommentDto));

                    i.CreateMap<ServiceComment, IServiceCommentDto>().ForMember(dest => dest.ServiceDto,
                        opt => opt.MapFrom(src => src.Service));

                    i.CreateMap<IServiceCommentDto, ServiceComment>().ForMember(dest => dest.Service,
                        opt => opt.MapFrom(src => src.ServiceDto));


                    i.CreateMap<ServiceTag, IServiceTagDto>()
                        .ForMember(dest => dest.ServiceDto, opt => opt.MapFrom(src => src.Service))
                        .ForMember(dest => dest.TagDto, opt => opt.MapFrom(src => src.Tag));

                    i.CreateMap<IServiceTagDto, ServiceTag>()
                        .ForMember(dest => dest.Service, opt => opt.MapFrom(src => src.ServiceDto))
                        .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src.TagDto));

                    i.CreateMap<ServiceReceiver, IServiceReceiverDto>().ForMember(dest => dest.ServiceReceiverServiceDto,
                        opt => opt.MapFrom(src => src.ServiceReceiverServices));

                    i.CreateMap<IServiceReceiverDto, ServiceReceiver>().ForMember(dest => dest.ServiceReceiverServices,
                        opt => opt.MapFrom(src => src.ServiceReceiverServiceDto));

                    i.CreateMap<IServiceReceiverServiceDto, ServiceReceiverService>().ForMember(dest => dest.Service,
                            opt => opt.MapFrom(src => src.ServiceDto))
                        .ForMember(dest => dest.ServiceReceiver, opt => opt.MapFrom(src => src.ServiceReceiverDto));

                    i.CreateMap<ServiceReceiverService, IServiceReceiverServiceDto>().ForMember(dest => dest.ServiceDto,
                            opt => opt.MapFrom(src => src.Service))
                        .ForMember(dest => dest.ServiceReceiverDto, opt => opt.MapFrom(src => src.ServiceReceiver));

                    i.CreateMap<Slider, ISliderDto>().ForMember(dest => dest.SliderItemDto,
                        opt => opt.MapFrom(src => src.SliderItems));

                    i.CreateMap<ISliderDto, Slider>().ForMember(dest => dest.SliderItems,
                        opt => opt.MapFrom(src => src.SliderItemDto));

                    i.CreateMap<SliderItem, ISliderItemDto>()
                        .ForMember(dest => dest.SliderDto, opt => opt.MapFrom(src => src.Slider));

                    i.CreateMap<ISliderItemDto, SliderItem>()
                        .ForMember(dest => dest.Slider, opt => opt.MapFrom(src => src.SliderDto));

                });
        }
    }
}
