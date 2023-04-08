using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// کاربر
    /// </summary>
    public partial class User
    {
        public User()
        {
            Id = new Guid();
            CreateDate = DateTime.Now;

            LoginHistories = new HashSet<LoginHistory>();
            UserProfiles = new HashSet<UserProfile>();
            UserRoles = new HashSet<UserRole>();
            UserContacts = new HashSet<UserContact>();
            Pages = new HashSet<Page>();
        }

        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool BuiltIn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEnable { get; set; }
        public DateTime CreateDate { get; set; }
        public bool MostChangePassword { get; set; }
        public int LoginRetryCount { get; set; }
        public Guid LanguageId { get; set; }

        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }
        public List<User> UserListCollction { get; set; }
        public virtual ICollection<LoginHistory> LoginHistories { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserContact> UserContacts { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
    }

    public partial class User
    {
        private static readonly string PasswordSalt = "993BA245D5E140F7AC59C95FDCBE4E22";

        public bool IsAdministratorUser
        {
            get
            {
                if (this.UserName != null)
                    return this.UserName.ToLower() == "administrator";
                else
                    return false;
            }
        }

        public bool IsPublicUser
        {
            get
            {
                if (this.UserName != null)
                    return this.UserName.ToLower() == "publicuser";
                else
                    return false;
            }
        }

        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        public bool CheckPassword(string password)
        {
            return this.PasswordHash == CalculatePasswordHash(password);
        }
        public void SetPassword(string password)
        {
            this.PasswordHash = CalculatePasswordHash(password);
        }


        #region Internal

        private static string CalculatePasswordHash(string password)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(password + PasswordSalt)));
        }

        //internal override void OnAdd()
        //{
        //    if (m_picture != null)
        //        BlobStorage.AddFile(this.Id, m_picture);

        //    Settings.UsersDataVersion++;
        //}
        //internal override void OnDelete()
        //{
        //    Settings.UsersDataVersion++;
        //}
        //internal override void OnModify()
        //{
        //    if (m_pictureIsUpdated)
        //    {
        //        if (m_picture == null)
        //            BlobStorage.RemoveFile(this.Id);
        //        else
        //            BlobStorage.AddFile(this.Id, m_picture);
        //    }

        //    Settings.UsersDataVersion++;
        //}

        #endregion
    }
}
