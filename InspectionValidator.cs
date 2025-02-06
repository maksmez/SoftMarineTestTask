using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SoftMarine
{
    public static class InspectionValidator
    {
        public static bool ValidateInspection(Inspection inspection, out List<string> errorMessages)
        {
            var listErrors = new List<string>();
            bool isValid = true;

            var validationContext = new ValidationContext(inspection);
            var validationResults = new List<ValidationResult>();

            // Проверяем саму инспекцию
            if (!Validator.TryValidateObject(inspection, validationContext, validationResults, true))
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

                if (!Validator.TryValidateObject(remarksList[i], remarkContext, remarkResults, true))
                {
                    listErrors.AddRange(remarkResults.Select(x => $"Ошибка в {i + 1} замечании: " + x.ErrorMessage));
                }
            }

            if (listErrors.Count > 0)
                isValid = false;

            errorMessages = listErrors;
            return isValid;
        }
    }
}
