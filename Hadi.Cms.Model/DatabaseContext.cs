using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Migration;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Hadi.Cms.Model
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=SqlConnectionString")
        {
        }

        static DatabaseContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Configuration>());
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<AttachmentFile> AttachmentFiles { get; set; }
        public DbSet<AttachmentFileTag> AttachmentFileTags { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<ProjectTechnology> ProjectTechnologies { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTag> ProjectTags { get; set; }
        public DbSet<ProjectAttachmentFile> ProjectAttachmentFiles { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ConfirmKey> ConfirmKeys { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseTeacher> CourseTeachers { get; set; }
        public DbSet<CourseAttachmentFile> CourseAttachmentFiles { get; set; }
        public DbSet<CourseTag> CourseTags { get; set; }
        public DbSet<CareerOpportunity> CareerOpportunities { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentSelectionReason> DepartmentSelectionReasons { get; set; }
        public DbSet<DepartmentService> DepartmentServices { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<FooterCategory> FooterCategories { get; set; }
        public DbSet<FooterCategoryLink> FooterCategoryLinks { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<FormFieldValue> FormFieldValues { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }
        public DbSet<IpBanned> IpBanneds { get; set; }
        public DbSet<IpRange> IpRanges { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }
        public DbSet<Mail> Mails { get; set; }
        public DbSet<MailAttachment> MailAttachments { get; set; }
        public DbSet<MailUser> MailUsers { get; set; }
        public DbSet<Methodology> Methodologies { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<NewsNewsCategory> NewsNewsCategories { get; set; }
        public DbSet<NlEmail> Newsletterses { get; set; }
        public DbSet<NlMessage> NewslettersHistories { get; set; }
        public DbSet<RoleFeature> RoleFeatures { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderItem> SliderItems { get; set; }

        public DbSet<UserContact> UserContacts { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Entities.Language> Languages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageStatistic> PageStatistics { get; set; }
        public DbSet<RelatedArticle> RelatedArticles { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<TemplateDetail> TemplateDetails { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceComment> ServiceComments { get; set; }
        public DbSet<ServiceTag> ServiceTags { get; set; }
        public DbSet<ServiceReceiver> ServiceReceivers { get; set; }
        public DbSet<ServiceReceiverService> ServiceReceiverServices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        private void Initialize()
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
            Configuration.ValidateOnSaveEnabled = true;
            Configuration.AutoDetectChangesEnabled = true;

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Configuration>());
        }

        public static void ExecuteMigration()
        {
            try
            {
                var dbContext = new DatabaseContext();
                dbContext.Database.Initialize(true);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
