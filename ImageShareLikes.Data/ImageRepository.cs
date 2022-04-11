using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageShareLikes.Data
{
    public class ImageRepository
    {
        private string _connectionString;
        public ImageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddImage(Image image)
        {
            var context = new ImageDataContext(_connectionString);
            image.Date = DateTime.Now;
            context.Images.Add(image);
            context.SaveChanges();
        }

        public List<Image> GetImages()
        {
            var context = new ImageDataContext(_connectionString);
            return context.Images.OrderByDescending(i => i.Date).ToList();
        }

        public Image GetById(int id)
        {
            using var context = new ImageDataContext(_connectionString);
            return context.Images.FirstOrDefault(i => i.ID == id);
        }

        public void IncrementImageLikes(int id)
        {
            using var context = new ImageDataContext(_connectionString);
            context.Database.ExecuteSqlInterpolated($"UPDATE Images SET Likes += 1 WHERE ID = {id}");
        }

        public int GetIamgeLikes(int id)
        {
            using var context = new ImageDataContext(_connectionString);
            return context.Images.FirstOrDefault(i => i.ID == id).Likes;
        }
    }
}
