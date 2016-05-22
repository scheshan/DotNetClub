using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewModels.My
{
    public class EditSettingsModel
    {
        [StringLength(200)]
        public string WebSite { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        [StringLength(500)]
        public string Signature { get; set; }
    }
}
