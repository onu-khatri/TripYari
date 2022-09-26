using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Travel.Core.Data.Extenstion
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder RegisterTypes(this ModelBuilder modelBuilder, IEnumerable<Type> entities,
            IEnumerable<Type> valueObjects, string entitySchema, string valueObjectSchema = "shared")
        {
            var entityTypes = new List<Type>();
            entityTypes.AddRange(entities);

            // temporary to concanate with s at the end, but need to have a way to translate it to a plural noun
            foreach (var type in entityTypes)
                modelBuilder.Entity(type).ToTable($"{type.Name}", entitySchema);

            var valueTypes = new List<Type>();
            if(valueObjects != null)
            valueTypes.AddRange(valueObjects);

            // temporary to concanate with s at the end, but need to have a way to translate it to a plural noun
            foreach (var type in valueTypes)
                modelBuilder.Entity(type).ToTable($"{type.Name}", valueObjectSchema);

            return modelBuilder;
        }

        public static ModelBuilder RegisterEntityTypes(this ModelBuilder modelBuilder, IEnumerable<Type> entities,
             string entitySchema)
        {
            var entityTypes = new List<Type>();
            entityTypes.AddRange(entities);

            // temporary to concanate with s at the end, but need to have a way to translate it to a plural noun
            foreach (var type in entityTypes)
            {
                var prop = type.GetProperty("Id");
                var builder = modelBuilder.Entity(type).ToTable($"{type.Name}", entitySchema);
                
                if(prop != null && prop.PropertyType == typeof(int))
                    builder.Property("Id").ValueGeneratedOnAddOrUpdate();

                //TODO: ignore Guid to create on update
            }
            
            return modelBuilder;
        }

        public static ModelBuilder RegisterValueTypes(this ModelBuilder modelBuilder,
            IEnumerable<Type> valueObjects, string valueObjectSchema = "shared")
        {
            var valueTypes = new List<Type>();
            if (valueObjects != null)
                valueTypes.AddRange(valueObjects);

            // temporary to concanate with s at the end, but need to have a way to translate it to a plural noun
            foreach (var type in valueTypes)
            {
                var prop = type.GetProperty("Id");
                var builder = modelBuilder.Entity(type).ToTable($"{type.Name}", valueObjectSchema);

                if (prop != null && prop.PropertyType == typeof(int))
                    builder.Property("Id").ValueGeneratedOnAddOrUpdate();
            }

            return modelBuilder;
        }
    }
}
