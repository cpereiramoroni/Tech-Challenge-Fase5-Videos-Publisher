using System;

namespace App.Domain.Validations
{
    public class AssertionConcern
    {


        /// <summary>
        /// Validação de string se esta vazia
        /// </summary>
        /// <param name="stringValue"></param>
        /// <param name="message"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AssertArgumentNotEmpty(string stringValue, string message)
        {
            if (stringValue == null || stringValue.Trim().Length == 0)
            {
                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// Validação se objeto é null
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="message"></param>
        /// <exception cref="DomainException"></exception>
        public static void AssertArgumentNotNull(object object1, string message)
        {
            if (object1 == null)
            {
                throw new ArgumentException(message);
            }
        }

        public static void AssertArgumentNotNullZero(decimal object1, string message)
        {
            AssertArgumentNotNull(object1, message);
            if (object1 == 0)
            {
                throw new ArgumentException(message);
            }
        }
    }

}
