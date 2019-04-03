using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Common
{
    /// <summary>
    /// Представляет базовый клас пользовательских ошибок Системы.
    /// </summary>
    public class SibException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SibException.
        /// </summary>
        public SibException()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса SibException.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        public SibException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса SibException.
        /// </summary>
        /// <param name="message">Текст сообщения об ошибке.</param>
        /// <param name="inner">Внутренняя ошибка.</param>
        public SibException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
