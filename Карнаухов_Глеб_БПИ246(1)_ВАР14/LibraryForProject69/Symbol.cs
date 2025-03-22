using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForProject69
{
    /// <summary>
    /// Структура для работы Parser'а и работы с типами данных
    /// </summary>
    internal struct Symbol
    {
        public SymbolType Type { get; init; }
        public string Value { get; init; }

        public Symbol(SymbolType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
    /// <summary>
    /// Перечесление хранящие все виды типов данных с которыми в ходе своей работы программа
    /// </summary>
    internal enum SymbolType
    {
        LeftBrace,
        RightBrace,
        LeftBracket,
        RightBracket,
        Colon,
        Comma,
        String,
        Number,
        Bool,
        Null,
        Unfamiliar
    }
}
