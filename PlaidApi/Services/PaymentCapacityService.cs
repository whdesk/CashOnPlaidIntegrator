using System;
using System.Collections.Generic;
using System.Linq;

namespace PlaidApi.Services
{
    public class PaymentCapacityService
    {
        public PaymentCapacityResult CalculatePaymentCapacity(List<PlaidTransaction> transactions)
        {
            // Filtrar transacciones de los últimos 90 días
            var recentTransactions = transactions
                .Where(t => t.date <= DateTime.UtcNow && t.date >= DateTime.UtcNow.AddDays(-90))
                .ToList();

            // Identificar ingresos recurrentes
            var recurringIncomes = GetRecurringIncomes(recentTransactions);
            var monthlyAverageIncome = recurringIncomes.Sum() / 3; // Promedio de 3 meses

            // Identificar egresos recurrentes
            var recurringExpenses = GetRecurringExpenses(recentTransactions);
            var monthlyAverageExpenses = recurringExpenses.Sum() / 3; // Promedio de 3 meses

            // Calcular ratio
            var ratio = monthlyAverageIncome / monthlyAverageExpenses;

            // Determinar resultado
            PaymentCapacityStatus status;
            string message;

            if (ratio >= 1.5)
            {
                status = PaymentCapacityStatus.Good;
                message = "Capacidad Buena → Aprobado";
            }
            else if (ratio >= 1.2)
            {
                status = PaymentCapacityStatus.Acceptable;
                message = "Capacidad Aceptable → Aprobado";
            }
            else
            {
                status = PaymentCapacityStatus.Low;
                message = "Capacidad Baja → Rechazado";
            }

            return new PaymentCapacityResult
            {
                Ratio = ratio,
                Status = status,
                Message = message,
                MonthlyAverageIncome = monthlyAverageIncome,
                MonthlyAverageExpenses = monthlyAverageExpenses
            };
        }

        private decimal[] GetRecurringIncomes(List<PlaidTransaction> transactions)
        {
            var incomes = new List<decimal>();
            
            // Identificar ingresos recurrentes
            var recurringIncomes = transactions
                .Where(t => t.amount > 0 && 
                           (t.description.Contains("SALARIO") || 
                            t.description.Contains("NÓMINA") || 
                            t.description.Contains("PAGO EMPRESA") || 
                            t.description.Contains("INGRESO FREELANCE")))
                .GroupBy(t => new { t.description, t.date.Month })
                .Where(g => g.Count() >= 3) // Al menos 3 meses de recurrencia
                .Select(g => g.Sum(t => t.amount))
                .ToList();

            return recurringIncomes.ToArray();
        }

        private decimal[] GetRecurringExpenses(List<PlaidTransaction> transactions)
        {
            var expenses = new List<decimal>();
            
            // Identificar egresos recurrentes
            var recurringExpenses = transactions
                .Where(t => t.amount < 0 && 
                           (t.description.Contains("ALQUILER") || 
                            t.description.Contains("HIPOTECA") || 
                            t.description.Contains("SERVICIO") || 
                            t.description.Contains("SUSCRIPCION") || 
                            t.description.Contains("SEGURO") || 
                            t.description.Contains("CUOTA") || 
                            t.description.Contains("PAGO TARJETA")))
                .GroupBy(t => new { t.description, t.date.Month })
                .Where(g => g.Count() >= 3) // Al menos 3 meses de recurrencia
                .Select(g => g.Sum(t => t.amount))
                .ToList();

            return recurringExpenses.ToArray();
        }
    }

    public class PaymentCapacityResult
    {
        public decimal Ratio { get; set; }
        public PaymentCapacityStatus Status { get; set; }
        public string Message { get; set; }
        public decimal MonthlyAverageIncome { get; set; }
        public decimal MonthlyAverageExpenses { get; set; }
    }

    public enum PaymentCapacityStatus
    {
        Good,
        Acceptable,
        Low
    }
}
