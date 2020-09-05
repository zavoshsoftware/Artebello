
using System.Data.Entity.ModelConfiguration;
using Resources;

namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User : BaseEntity
    {
        public User()
        { 
            Orders=new List<Order>();
            Sellers=new List<Seller>();
            ActivationCodes = new List<ActivationCode>();
        }


        [Display(Name = "Password", ResourceType = typeof(Resources.Models.User))]
        [StringLength(150, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Password { get; set; }

        [Display(Name = "CellNum", ResourceType = typeof(Resources.Models.User))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(20, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        [RegularExpression(@"(^(09|9)[0123456789][0123456789]\d{7}$)|(^(09|9)[0123456789][0123456789]\d{7}$)", ErrorMessageResourceName = "MobilExpersionValidation", ErrorMessageResourceType = typeof(Messages))]
        public string CellNum { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(Resources.Models.User))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(250, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string FullName { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Resources.Models.User))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int Code { get; set; }

        [Display(Name = "نقش کاربر")]
        public Guid RoleId { get; set; }

        [Display(Name ="ایمیل")]
        public string   Email { get; set; }

        [Display(Name ="تاریخ تولد")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "محل تولد")]
        public string BirthLocation { get; set; }

        [Display(Name = "جنسیت")]
        public bool? Gender { get; set; }

        [Display(Name = "تلفن ثابت")]
        public string Phone { get; set; }

        [Display(Name = "کد پستی")]
        public string ZipCode { get; set; }

        [Display(Name = "استان محل سکونت")]
        public string CurrentProvince { get; set; }

        [Display(Name = "آدرس پستی")]
        public string Address { get; set; }

        public virtual Role Role { get; set; }
 
   
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Seller> Sellers { get; set; }
        public virtual ICollection<ActivationCode> ActivationCodes { get; set; }
    
        internal class configuration : EntityTypeConfiguration<User>
        {
            public configuration()
            {
                HasRequired(p => p.Role).WithMany(j => j.Users).HasForeignKey(p => p.RoleId);
            }
        }
    }
}

