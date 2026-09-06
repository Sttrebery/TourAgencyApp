using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourAgencyApp.Models;

namespace TourAgencyApp.Services
{
    public class ImageService
    {

        /// <summary>
        /// Загрузка нескольких фотографий
        /// </summary>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        public static async Task<List<Photo>> LoadPhotosAsync(string[] fileNames)
        {
            var photosToAdd = new List<Photo>();

            foreach (string filename in fileNames)
            {
                try
                {
                    byte[] imageData = await File.ReadAllBytesAsync(filename);

                    // Сжимаем изображение
                    byte[] compressedData = CompressImage(imageData);

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

        /// <summary>
        /// Загрузка одной фотографии
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static async Task<Photo> LoadPhotoAsync(string filename)
        {
            Photo photoToAdd = null; // = new Photo();
            try
            {
                byte[] imageData = await File.ReadAllBytesAsync(filename);

                // Сжимаем изображение
                byte[] compressedData = CompressImage(imageData);

                photoToAdd = new Photo() { PhotoValue = compressedData };
            }
            catch (Exception ex)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show($"Не удалось загрузить файл: {filename}\nОшибка: {ex.Message}", "Ошибка");
                });
            }

            return photoToAdd;
        }

        private static byte[] CompressImage(byte[] imageData, int maxWidth = 1920, int maxHeight = 1080, int quality = 80)
        {
            try
            {
                using (var ms = new MemoryStream(imageData))
                using (var originalImage = Image.FromStream(ms))
                {
                    // Вычисляем новые размеры
                    var ratio = Math.Min((double)maxWidth / originalImage.Width,
                                         (double)maxHeight / originalImage.Height);

                    if (ratio >= 1) return imageData;

                    int newWidth = (int)(originalImage.Width * ratio);
                    int newHeight = (int)(originalImage.Height * ratio);

                    using (var bitmap = new Bitmap(newWidth, newHeight))
                    using (var graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);

                        var encoder = GetEncoder(ImageFormat.Jpeg);
                        var encoderParams = new EncoderParameters(1);
                        encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                        using (var resultMs = new MemoryStream())
                        {
                            bitmap.Save(resultMs, encoder, encoderParams);
                            return resultMs.ToArray();
                        }
                    }
                }
            }
            catch
            {
                return imageData;
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.FirstOrDefault(c => c.FormatID == format.Guid)!;
        }
    }
}
