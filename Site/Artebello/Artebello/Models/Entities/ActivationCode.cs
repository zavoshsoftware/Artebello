using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace Models
{
    public class ActivationCode : BaseEntity
    {
        [Display(Name = "Code", ResourceType = typeof(Resources.Models.ActivationCode))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int Code { get; set; }
        [Display(Name = "ExpireDate", ResourceType = typeof(Resources.Models.ActivationCode))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public DateTime ExpireDate { get; set; }
        [Display(Name = "IsUsed", ResourceType = typeof(Resources.Models.ActivationCode))]
        public bool IsUsed { get; set; }
        [Display(Name = "UsingDate", ResourceType = typeof(Resources.Models.ActivationCode))]
        public DateTime? UsingDate { get; set; }

      

        public Guid UserId { get; set; }
        public virtual User User { get; set; }


        internal class Configuration : EntityTypeConfiguration<ActivationCode>
        {
            public Configuration()
            {
                HasRequired(p => p.User)
                    .WithMany(j => j.ActivationCodes)
                    .HasForeignKey(p => p.UserId);
            }
        }
    }
}
