using DotNetClub.Core.Model.Category;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Service
{
    public class CategoryService
    {
        private IConfiguration Configuration { get; set; }

        public CategoryService(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public List<CategoryModel> All()
        {
            var categorySections = this.Configuration.GetSection("Categories").GetChildren();

            List<CategoryModel> categoryList = new List<CategoryModel>();

            foreach (var section in categorySections)
            {
                var category = new CategoryModel
                {
                    Key = section["Key"],
                    Name = section["Name"]
                };
                categoryList.Add(category);
            }

            return categoryList;
        }
    }
}
