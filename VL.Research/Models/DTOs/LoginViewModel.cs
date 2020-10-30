using System;
using System.ComponentModel.DataAnnotations;

namespace BBee.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// �û���
        /// </summary>
        [Required]
        [Display(Name = "�û���")]
        public string UserName { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "����")]
        public string Password { get; set; }

        /// <summary>
        /// ��ס��
        /// </summary>
        //[Display(Name = "��ס��?")]
        public bool RememberMe { get; set; }
    }
}
