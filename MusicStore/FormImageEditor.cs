using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicStore
{
    public partial class FormImageEditor : Form
    {
        private NpgsqlConnection conn;
        private int recordId;
        private Image currentImage;
        private byte[] imageBytes;
        public FormImageEditor(int recordId, string recordTitle)
        {
            InitializeComponent();
            picCover.SizeMode = PictureBoxSizeMode.Zoom;
            this.recordId = recordId;
            this.Text = "Изображение обложки: " + recordTitle;
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);

            LoadCurrentImage();
        }

        private void LoadCurrentImage()
        {
            try
            {
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT cover_image FROM shem.record WHERE id_record = @id", conn);
                cmd.Parameters.AddWithValue("id", recordId);

                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    imageBytes = (byte[])result;

                    Image loadedImage = ByteArrayToImage(imageBytes);
                    picCover.Image = loadedImage;

                    if (currentImage != null)
                    {
                        currentImage.Dispose();
                    }
                    currentImage = loadedImage;

                    lblImageInfo.Text = "Изображение загружено";
                    butDel.Enabled = true;
                }
                else
                {
                    ClearImage();
                    lblImageInfo.Text = "Изображение отсутствует";
                    butDel.Enabled = false;
                    imageBytes = null;
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки изображения: " + ex.Message);
            }
        }

        private Image ByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;

            try
            {
                using (MemoryStream ms = new MemoryStream(byteArray))
                {
                    return Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка преобразования данных в изображение: " + ex.Message);
                return null;
            }
        }

        private void ClearImage()
        {
            if (currentImage != null)
            {
                currentImage.Dispose();
                currentImage = null;
            }
            picCover.Image = null;
            if (imageBytes != null)
            {
                imageBytes = null;
            }
        }

        private void FormImageEditor_Load(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = openFileDialog.FileName;
                        FileInfo fileInfo = new FileInfo(filePath);


                        long maxFileSize = 10 * 1024 * 1024; //10 МБ в байтах
                        if (fileInfo.Length > maxFileSize)
                        {
                            MessageBox.Show($"Файл слишком большой.\n" +
                                          $"Максимальный размер: 10 МБ\n",
                                          "Ошибка",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Error);
                            return;
                        }

                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                        string fileExtension = Path.GetExtension(filePath).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            MessageBox.Show($"Неподдерживаемый формат файла\n" +
                                          $"Поддерживаются: {string.Join(", ", allowedExtensions)}",
                                          "Ошибка",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Error);
                            return;
                        }

                        Image originalImage = null;
                        try
                        {
                            byte[] fileBytes = File.ReadAllBytes(filePath);
                            using (MemoryStream ms = new MemoryStream(fileBytes))
                            {
                                originalImage = Image.FromStream(ms);
                            }
                        }
                        catch (Exception imgEx)
                        {
                            MessageBox.Show($"Файл поврежден или не является изображением: {imgEx.Message}",
                                          "Ошибка",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Error);
                            return;
                        }

                        int minWidth = 100;
                        int minHeight = 100;
                        int maxWidth = 400;
                        int maxHeight = 400;

                        if (originalImage.Width < minWidth || originalImage.Height < minHeight)
                        {
                            originalImage.Dispose();
                            MessageBox.Show($"Изображение слишком маленькое.\n" +
                                          $"Минимальный размер: {minWidth}x{minHeight} пикселей\n" +
                                          $"Текущий размер: {originalImage.Width}x{originalImage.Height}",
                                          "Ошибка",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Error);
                            return;
                        }

                        if (originalImage.Width > maxWidth || originalImage.Height > maxHeight)
                        {
                            originalImage.Dispose();
                            MessageBox.Show($"Изображение слишком большое.\n" +
                                          $"Максимальный размер: {maxWidth}x{maxHeight} пикселей\n" +
                                          $"Текущий размер: {originalImage.Width}x{originalImage.Height}",
                                          "Ошибка",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Error);
                            return;
                        }

                        ClearImage();
                        imageBytes = File.ReadAllBytes(filePath);
                        currentImage = originalImage;
                        picCover.Image = originalImage;
                        lblImageInfo.Text = Path.GetFileName(filePath);
                        butDel.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка загрузки файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ClearImage();
                    }
                }
            }
        }

        private void butUpd_Click(object sender, EventArgs e)
        {
            if (!ValidateImage(imageBytes))
                return;

            try
            {
                conn.Open();

                NpgsqlCommand cmd;
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    cmd = new NpgsqlCommand("UPDATE shem.record SET cover_image = @image WHERE id_record = @id", conn);
                    cmd.Parameters.AddWithValue("image", imageBytes);
                }
                else
                {
                    cmd = new NpgsqlCommand("UPDATE shem.record SET cover_image = NULL WHERE id_record = @id", conn);
                }

                cmd.Parameters.AddWithValue("id", recordId);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Изменения сохранены", "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Пластинка не найдена");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private bool ValidateImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return true;

            const long maxSize = 10 * 1024 * 1024;
            if (imageData.Length > maxSize)
            {
                MessageBox.Show("Размер файла слишком большой. Максимально - 10 МБ", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    using (Image image = Image.FromStream(ms, false, false))
                    {
                        if (image.Width < 100 || image.Height < 100)
                        {
                            MessageBox.Show("Изображение слишком маленькое. Минимальный размер - 100x100 пикселей", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        if (image.Width > 400 || image.Height > 400)
                        {
                            MessageBox.Show("Изображение слишком большое. Максимальный размер - 400x400 пикселей", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        if (image.RawFormat.Guid != System.Drawing.Imaging.ImageFormat.Jpeg.Guid &&
                            image.RawFormat.Guid != System.Drawing.Imaging.ImageFormat.Png.Guid)
                        {
                            MessageBox.Show("Неподдерживаемый формат изображения. Допустимы: JPEG, PNG", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        try
                        {
                            using (Bitmap bitmap = new Bitmap(image))
                            {
                                return true;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Некорректное изображение", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Файл не является изображением или поврежден", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Изображение слишком большое для обработки", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки изображения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void butDel_Click(object sender, EventArgs e)
        {
            ClearImage();
            lblImageInfo.Text = "Изображение удалено";
            butDel.Enabled = false;
        }

        private void butClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
