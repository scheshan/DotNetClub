using DotNetClub.Core.Model.Category;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewModels.Topic
{
    public class NewViewModel
    {
        public SelectList CategoryList { get; set; }
        
        public NewModel Model { get; set; }

        public string ErrorMessage { get; set; }
    }
}
