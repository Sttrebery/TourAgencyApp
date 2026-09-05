using Microsoft.Win32;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using TourAgencyApp.Models;

namespace TourAgencyApp.Services
{
    public class ImageService
    {
        public static async Task<List<Photo>> LoadPhotosAsync(string[] fileNames)
        {
            var photosToAdd = new List<Photo>();

            // Параметры сжатия можно настроить
            const int maxWidth = 1920;
            const int maxHeight = 1080;
            const int quality = 80; // 1-100, где 100 - максимальное качество

            foreach (string filename in fileNames)
            {
                try
                {
                    byte[] imageData = await File.ReadAllBytesAsync(filename);

                    // Сжимаем изображение
                    byte[] compressedData = CompressImage(imageData, maxWidth, maxHeight, quality);

                    photosToAdd.Add(new Photo() { PhotoValue = compressedData });
                }
                catch (Exception ex)
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        MessageBox.Show($"Не удалось загрузить файл: {filename}\nОшибка: {ex.Message}", "Ошибка");
                    });
                }
            }

            return photosToAdd;
        }

        private static byte[] CompressImage(byte[] imageData, int maxWidth = 1920, int maxHeight = 1080, int quality = 80)
        {
            try
            {
                using (var inputStream = new MemoryStream(imageData))
                using (var image = Image.Load(inputStream))
                {
                    // Вычисляем новые размеры с сохранением пропорций
                    var ratio = Math.Min((double)maxWidth / image.Width, (double)maxHeight / image.Height);

                    // Если изображение меньше максимальных размеров, сжимаем только по качеству
                    if (ratio >= 1)
                    {
                        using (var outputStream = new MemoryStream())
                        {
                            var encoder = new JpegEncoder { Quality = quality };
                            image.Save(outputStream, encoder);
                            return outputStream.ToArray();
                        }
                    }

                    // Изменяем размер
                    int newWidth = (int)(image.Width * ratio);
                    int newHeight = (int)(image.Height * ratio);

                    image.Mutate(x => x.Resize(newWidth, newHeight));

                    using (var outputStream = new MemoryStream())
                    {
                        var encoder = new JpegEncoder { Quality = quality };
                        image.Save(outputStream, encoder);
                        return outputStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                // Если сжатие не удалось, возвращаем оригинал
                return imageData;
            }
        }
    }
}
