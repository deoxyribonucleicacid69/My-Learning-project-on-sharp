using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System;
using System.IO;
/// <summary>
/// Класс для создания отчета в PDF-файл
/// </summary>
public class MakePDFReport
{
    /// <summary>
    /// Метод для загрузки текста в PDF-ФАЙЛ 
    /// </summary>
    /// <param name="fragment"></param>
    /// <param name="nameFile"></param>
    [Obsolete]
    public void DownloadPdfFileWithReport(string fragment, string nameFile)
    {
        try
        {
            GlobalFontSettings.FontResolver = new CustomFontResolver();

   
            string filePath = $"..\\..\\..\\..\\..\\project\\WorkingFiles\\{nameFile}.pdf";

            Document document = new Document();

            Section section = document.AddSection();
            section.PageSetup.PageWidth = Unit.FromMillimeter(210); 
            section.PageSetup.PageHeight = Unit.FromMillimeter(297); 
            section.PageSetup.LeftMargin = Unit.FromMillimeter(20); 
            section.PageSetup.RightMargin = Unit.FromMillimeter(20); 
            section.PageSetup.TopMargin = Unit.FromMillimeter(20); 
            section.PageSetup.BottomMargin = Unit.FromMillimeter(20); 

            var style = document.Styles["Normal"];
            style.Font.Name = "SimSun"; // Используем кастомный шрифт
            style.Font.Size = 12;

            Paragraph pr = section.AddParagraph();
            pr.Format.Alignment = ParagraphAlignment.Left; 
            pr.Format.FirstLineIndent = 0; 
            pr.Format.SpaceBefore = 0; 
            pr.Format.SpaceAfter = 0;
            pr.Format.WidowControl = true; 
            pr.Format.KeepTogether = false; 
            pr.Format.KeepWithNext = false; 

            pr.AddText(fragment);

            // Рендерим документ в PDF
            var pdfRenderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };

            // Рендерим документ
            pdfRenderer.RenderDocument();

            // Сохраняем PDF-файл
            pdfRenderer.PdfDocument.Save(filePath);

            Console.WriteLine($"PDF-файл успешно создан: {nameFile}.pdf");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}

/// <summary>
/// Класс для разрешения и загрузки шрифтов, реализующий интерфейс <see cref="IFontResolver"/>.
/// </summary>
public class CustomFontResolver : IFontResolver
{
    /// <summary>
    /// Загружает шрифт из файла по указанному имени.
    /// </summary>
    /// <param name="faceName">Имя шрифта, который требуется загрузить.</param>
    /// <exception cref="FileNotFoundException">
    /// Выбрасывается, если файл шрифта не найден по указанному пути.
    /// </exception>
    /// <exception cref="Exception">
    /// Выбрасывается, если произошла ошибка при загрузке шрифта.
    /// </exception>
    public byte[] GetFont(string faceName)
    {
        try
        {
            // Формируем путь к файлу шрифта
            string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SimSun.ttf");

            // Проверяем, существует ли файл шрифта
            if (!File.Exists(fontPath))
            {
                throw new FileNotFoundException($"Шрифт не найден: {fontPath}");
            }

            // Читаем и возвращаем данные шрифта
            return File.ReadAllBytes(fontPath);
        }
        catch (Exception ex)
        {
            // Логируем ошибку и пробрасываем исключение дальше
            Console.WriteLine($"Ошибка при загрузке шрифта: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Разрешает тип шрифта на основе имени семейства шрифтов и его стиля (жирный, курсив).
    /// </summary>
    /// <param name="familyName">Имя семейства шрифтов.</param>
    /// <param name="isBold">Указывает, требуется ли жирный стиль шрифта.</param>
    /// <param name="isItalic">Указывает, требуется ли курсивный стиль шрифта.</param>
    /// <returns>
    /// Возвращает объект <see cref="FontResolverInfo"/>, содержащий информацию о разрешенном шрифте.
    /// Если шрифт не найден, возвращает <c>null</c>.
    /// </returns>
    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        // Проверяем, соответствует ли имя семейства шрифтов "SimSun"
        if (familyName.Equals("SimSun", StringComparison.OrdinalIgnoreCase))
            return new FontResolverInfo("SimSun");

        // Если шрифт не найден, возвращаем null
        return null;
    }
}