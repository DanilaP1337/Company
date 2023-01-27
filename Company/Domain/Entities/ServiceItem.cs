using System.ComponentModel.DataAnnotations;

namespace Company.Domain.Entities
{
    public class ServiceItem : EntityBase
    {
        [Required(ErrorMessage = "Заполните название услуги")]
        [Display(Name = "Название услуги (заголовок)")]
        public override string? Title { get; set; }

        [Display(Name = "Краткое описание услуги")]
        public override string? SubTitle { get; set; }

        [Display(Name = "Полное описание услуги")]
        public override string? Text { get; set; }
    }
}
