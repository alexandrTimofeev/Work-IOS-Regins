using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Безопасный EventLoader для Unity с поддержкой кавычек, запятых и переносов строк
/// </summary>
public static class EventLoader
{
    /// <summary>
    /// Загружает события по имени файла в Resources (без расширения)
    /// </summary>
    public static CardEvent[] Load(string resourceName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(resourceName);
        if (csvFile == null)
        {
            Debug.LogError($"EventLoader: не найден файл Resources/{resourceName}.csv");
            return new CardEvent[0];
        }

        return ParseCSV(csvFile.text);
    }

    /// <summary>
    /// Парсит CSV текст в массив CardEvent
    /// </summary>
    public static CardEvent[] ParseCSV(string csvText)
    {
        List<CardEvent> events = new List<CardEvent>();

        if (string.IsNullOrWhiteSpace(csvText))
            return events.ToArray();

        string[] lines = csvText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length < 2)
            return events.ToArray(); // нет данных кроме заголовка

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] columns = SplitCsvLine(line);

            // Если колонок меньше, дополняем пустыми
            if (columns.Length < 19)
                Array.Resize(ref columns, 19);

            try
            {
                CardEvent ce = new CardEvent
                {
                    EventID = GetString(columns, 0),
                    EventImageID = GetString(columns, 1),
                    CharacterID = GetString(columns, 2),
                    Title = GetString(columns, 3),
                    Text = GetString(columns, 4),
                    LeftText = GetString(columns, 5),
                    RightText = GetString(columns, 6),
                    LeftResources = new int[4]
                    {
                        GetInt(columns, 7),
                        GetInt(columns, 8),
                        GetInt(columns, 9),
                        GetInt(columns, 10)
                    },
                    RightResources = new int[4]
                    {
                        GetInt(columns, 11),
                        GetInt(columns, 12),
                        GetInt(columns, 13),
                        GetInt(columns, 14)
                    },
                    NextLeft = GetString(columns, 15),
                    NextRight = GetString(columns, 16),
                    WaitToPlay = GetInt(columns, 17),
                    Weight = GetInt(columns, 18, 10),
                    Condition = GetString(columns, 19),
                    onlyChoiceThisCard = GetBool(columns, 20)
                };

                events.Add(ce);
            }
            catch (Exception ex)
            {
                Debug.LogError($"EventLoader: ошибка при разборе строки {i + 1}: {ex.Message}");
            }
        }

        return events.ToArray();
    }

    /// <summary>
    /// Безопасное получение строки
    /// </summary>
    private static string GetString(string[] cols, int index)
    {
        if (index >= cols.Length || string.IsNullOrWhiteSpace(cols[index]))
            return string.Empty;
        return cols[index].Trim();
    }

    /// <summary>
    /// Безопасное преобразование в int
    /// </summary>
    private static int GetInt(string[] cols, int index, int defaultValue = 0)
    {
        if (index >= cols.Length) return defaultValue;
        int result;
        if (!int.TryParse(cols[index].Trim(), out result))
            return defaultValue;
        return result;
    }

    /// <summary>
    /// Безопасное преобразование в bool
    /// </summary>
    private static bool GetBool(string[] cols, int index)
    {
        if (index >= cols.Length) return false;
        string val = cols[index]?.Trim().ToLower() ?? "";
        return val == "true" || val == "1";
    }

    /// <summary>
    /// Разделяет CSV-строку с учетом кавычек и запятых внутри текста
    /// </summary>
    private static string[] SplitCsvLine(string line)
    {
        List<string> result = new List<string>();
        bool inQuotes = false;
        string current = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                // Двойные кавычки внутри текста
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current += '"';
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(current);
                current = "";
            }
            else
            {
                current += c;
            }
        }

        result.Add(current);
        return result.ToArray();
    }
}