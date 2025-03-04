using SoftMarine.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SoftMarine
{
    public static class Validator
    {
        public static bool ValidateInspection(Inspection inspection, out List<string> errorMessages)
        {
            var listErrors = new List<string>();
            bool isValid = true;

            var validationContext = new ValidationContext(inspection);
            var validationResults = new List<ValidationResult>();

            // Проверяем саму инспекцию
            if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(inspection, validationContext, validationResults, true))
            {
                listErrors.AddRange(validationResults.Select(x => x.ErrorMessage));
            }
            // Преобразуем ICollection<Remark> в List<Remark>, чтобы поддерживать индексирование
            var remarksList = inspection.Remarks.ToList();
            // Проверяем каждое замечание (Remark)
            for (int i = 0; i < inspection.Remarks.Count; i++)
            {
                var remarkContext = new ValidationContext(remarksList[i]);
                var remarkResults = new List<ValidationResult>();

                if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(remarksList[i], remarkContext, remarkResults, true))
                {
                    listErrors.AddRange(remarkResults.Select(x => $"Ошибка в {i + 1} замечании: " + x.ErrorMessage));
                }
            }

            if (listErrors.Count > 0)
                isValid = false;

            errorMessages = listErrors;
            return isValid;
        }

        public static bool ValidateInspector(Inspector inspector, out List<string> errorMessages)
        {
            var listErrors = new List<string>();
            bool isValid = true;

            var validationContext = new ValidationContext(inspector);
            var validationResults = new List<ValidationResult>();

            // Проверяем саму инспекцию
            if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(inspector, validationContext, validationResults, true))
            {
                listErrors.AddRange(validationResults.Select(x => x.ErrorMessage));
            }

            if (listErrors.Count > 0)
                isValid = false;

            errorMessages = listErrors;
            return isValid;
        }
    }
}
