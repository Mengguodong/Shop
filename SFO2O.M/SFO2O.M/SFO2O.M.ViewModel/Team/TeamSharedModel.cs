using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace SFO2O.Model.Team
{
    [Serializable]
    [DataContract]
    public class TeamSharedModel
    {

        /// <summary>
        /// AppId
        /// </summary>
        [DataMember(Name = "AppId")]
        [Display(Name = "AppId")]
        public string AppId { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [DataMember(Name = "Title")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// TeamCode
        /// </summary>
        [DataMember(Name = "TeamCode")]
        [Display(Name = "TeamCode")]
        public string TeamCode { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember(Name = "Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [DataMember(Name = "Url")]
        [Display(Name = "Url")]
        public string Url { get; set; }

        /// <summary>
        /// sku
        /// </summary>
        [DataMember(Name = "sku")]
        [Display(Name = "sku")]
        public string Sku { get; set; }

        /// <summary>
        /// ImagePath
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// ProductName
        /// </summary>
        [DataMember(Name = "ProductName")]
        [Display(Name = "ProductName")]
        public string ProductName { get; set; }

        /// <summary>
        /// IsOrderCodeInput
        /// </summary>
        [DataMember(Name = "IsOrderCodeInput")]
        [Display(Name = "IsOrderCodeInput")]
        public int IsOrderCodeInput { get; set; }

        /// <summary>
        /// RestTeamMemberNum
        /// </summary>
        [DataMember(Name = "RestTeamMemberNum")]
        [Display(Name = "RestTeamMemberNum")]
        public int RestTeamMemberNum { get; set; }

    }
}
