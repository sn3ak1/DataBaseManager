using System;
using System.Linq;
using System.Text;
using DataBaseManager.Data;
using Microsoft.EntityFrameworkCore;

namespace DataBaseManager.Business
{
    public static class Controller
    {
        public static Category[] GetCategories()
        {
            using (var context = new Context())
            {
                return context.Categories
                    .Include(x => x.Children)
                    .Include(x => x.IntProperties)
                    .Include(x => x.StringProperties)
                    .Include(x => x.EnumProperties)
                    .ThenInclude(x => x.Flags).ToArray();
            }
        }

        public static Category[] GetCategoriesBasic()
        {
            using (var context = new Context())
            {
                return context.Categories.AsNoTracking().ToArray();
            }
        }

        public static void RemoveCategory(Category category)
        {
            using (var context = new Context())
            {
                context.Categories.Attach(category);
                context.Categories.Remove(category);
                context.SaveChanges();
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
        public static void EditCategory(Category category)
        {
            using (var context = new Context())
            {
                context.Update(category);
                context.SaveChanges();
            }
        }

        public static void RemoveProperty(int propertyId, PropertyType type, Category category) 
        {
            using (var context = new Context())
            {
                switch (type)
                {
                    case PropertyType.Int:
                        var p1 = context.IntProperties.First(x => x.Id == propertyId);
                        context.IntProperties.Where(x => x.ParentId == propertyId)
                            .ForEachAsync(x => x.ParentId = 0);
                        foreach (var child in category.Children)
                        {
                            var p = child.IntProperties.FirstOrDefault(x => x.ParentId == propertyId);
                            if(p!=null)
                                p.ParentId = 0;
                        }
                        context.IntProperties.Remove(p1);
                        break;
                    case PropertyType.String:
                        var p2 = context.StringProperties.First(x => x.Id == propertyId);
                        context.StringProperties.Where(x => x.ParentId == propertyId)
                            .ForEachAsync(x => x.ParentId = 0);
                        foreach (var child in category.Children)
                        {
                            var p = child.StringProperties.FirstOrDefault(x => x.ParentId == propertyId);
                            if(p!=null)
                                p.ParentId = 0;                        }
                        context.StringProperties.Remove(p2);
                        break;
                    case PropertyType.Enum:
                        var p3 = context.EnumProperties.First(x => x.Id == propertyId);
                        context.EnumProperties.Where(x => x.ParentId == propertyId)
                            .ForEachAsync(x => x.ParentId = 0);
                        foreach (var child in category.Children)
                        {
                            var p = child.EnumProperties.FirstOrDefault(x => x.ParentId == propertyId);
                            if(p!=null)
                                p.ParentId = 0;                        }
                        context.EnumProperties.Remove(p3);
                        break;
                }
        
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

        public static Category GetRootCategory()
        {
            return GetCategories().First(x => x.Id == 1);
        }

        
    }
}