using System;
using System.IO; // Добавьте для Directory.GetCurrentDirectory()
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders; // !!! НУЖНЫЙ USING !!!

namespace ShopTARgv24.RealEstateTest
{
    public class TestHostEnvironment : IHostEnvironment // Если нужна полная реализация
    {
        public string EnvironmentName { get; set; } = "Testing";
        public string ApplicationName { get; set; } = "ShopTARgv24";
        public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();

        // !!! ИСПРАВЛЕНО: Тип должен быть IFileProvider !!!
        public IFileProvider ContentRootFileProvider { get; set; } = default!;

        // Эти свойства нужны, если FileServices на самом деле требует IWebHostEnvironment
        // public string WebRootPath { get; set; } = default!; 
        // public IFileProvider WebRootFileProvider { get; set; } = default!;
    }
}