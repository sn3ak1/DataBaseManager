using System.Linq;
using System.Text;
using DataBaseManager.Data;
using Microsoft.EntityFrameworkCore;

namespace DataBaseManager.Business
{
    public class Controller
    {
        private Category[] _categories;
        
        public Controller()
        {
            using (var context = new Context())
            {
                _categories = context.Categories
                    .Include(x => x.Children)
                    .Include(x => x.IntProperties)
                    .Include(x => x.StringProperties)
                    .Include(x => x.EnumProperties)
                    .ThenInclude(x => x.Flags).ToArray();
            }
        }
        
        public static void AddCategory(Category category)
        {
            using (var context = new Context())
            {
                context.Database.EnsureCreated();
                if(category.Parent!=null)
                    context.Categories.Attach(category.Parent);
                context.Categories.Add(category);

                context.SaveChanges();
            }
        }
        
        public static string PrintData()
        {
            var data = new StringBuilder();
            using (var context = new Context())
            {
                var categories = context.Categories
                    .Include(o => o.IntProperties)
                    .Include(o => o.StringProperties)
                    .Include(o => o.EnumProperties)
                    .ThenInclude(o => o.Flags);
                foreach(var category in categories)
                {
                    data.AppendLine($"\nId: {category.Id}");
                    data.AppendLine($"Name: {category.Name}");
                    if(category.Parent != null)
                        data.AppendLine($"Parent: {category.Parent.Name}");
                    if (category.IntProperties.Any())
                    {
                        data.AppendLine($"IntProps: ");
                        foreach (var prop in category.IntProperties)
                        {
                            data.AppendLine($"     {prop.Name} : {prop.Value}");
                        }
                    }
                    if (category.StringProperties.Any())
                    {
                        data.AppendLine($"StringProps: ");
                        foreach (var prop in category.StringProperties)
                        {
                            data.AppendLine($"     {prop.Name} : {prop.Value}");
                        }
                    }
                    if (category.EnumProperties.Any())
                    {
                        data.AppendLine($"EnumProps: ");
                        foreach (var prop in category.EnumProperties)
                        {
                            data.Append($"     keys: ");
                            foreach (var flag in prop.Flags)
                            {
                                data.Append($"{flag.Name} ");
                            }
                            data.AppendLine($"\n     {prop.Name} : {prop.Value}");
                        }
                    }
                    data.AppendLine();
                }
            }
            return data.ToString();
        }
        
        public static string getCategoryString(Category category)
        {
            var data = new StringBuilder();
            data.AppendLine($"Name: {category.Name}");
            data.AppendLine($"Parent: {category.Parent}");
            data.Append($"IntProps: ");
            foreach (var property in category.IntProperties)
            {
                data.Append($"     {property.Name} : {property.Value}");
            }
            
            data.Append($"\nStringProps: ");
            foreach (var property in category.StringProperties)
            {
                data.Append($"     {property.Name} : {property.Value}");
            }

            return data.ToString();
        }

        public Category GetRootCategory()
        {
            return _categories.First(x => x.Id == 1);
        }
    }
}