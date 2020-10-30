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
        /// 用户名
        /// </summary>
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        /// <summary>
        /// 记住我
        /// </summary>
        //[Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }
}
